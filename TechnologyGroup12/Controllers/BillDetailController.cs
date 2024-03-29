﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechnologyGroup12.DataAccess.Repository.IRepository;
using TechnologyGroup12.Models.Models;
using TechnologyGroup12.Models.ViewModels;

namespace TechnologyGroup12.Controllers
{
    public class BillDetailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Update(string? id)
        {
            var lProduct = _unitOfWork.SP_Call.List<Product>("SP_GetAll_Product");
            BillDetail billDetails = new BillDetail();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            billDetails = _unitOfWork.SP_Call.OneRecord<BillDetail>("SP_Get_BillDetail", parameters);
            billDetails.ProductList = lProduct.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View("Upsert", billDetails);
        }
        public IActionResult Insert(long? id)
        {
            var lProduct = _unitOfWork.SP_Call.List<Product>("SP_GetAll_Product");
            BillDetail billDetails = new BillDetail()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                BillId = id,
                ProductList = lProduct.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View("Upsert", billDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BillDetail billDetail)
        {
            var lProduct = _unitOfWork.SP_Call.List<Product>("SP_GetAll_Product");
            billDetail.ProductList = lProduct.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            var parameter = new DynamicParameters();
            parameter.Add("@ProductId", billDetail.ProductId);
            parameter.Add("@Quantity", billDetail.Quantity);
            parameter.Add("@BillId", billDetail.BillId);

            try
            {
                if (billDetail.Id.ToString() == "00000000-0000-0000-0000-000000000000") // Do là dạng Guid nên nó sẽ như vậy
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_BillDetail", parameter);
                    return RedirectToAction("Upsert", "Bill", new { id = billDetail.BillId.ToString() });
                }
                else
                {
                    parameter.Add("@Id", billDetail.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_BillDetail", parameter);
                    return View(billDetail);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.
                    Replace("The transaction ended in the trigger. The batch has been aborted", ""));
                return View(billDetail);
            }
        }

        [HttpGet]
        public IActionResult GetAllBillDetailOfBill(long? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@BillId", id);
            var allObj = _unitOfWork.SP_Call.List<BillDetailListVM>("SP_GetAll_BillDetail_Of_Bill", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                _unitOfWork.SP_Call.Excute("SP_Delete_BillDetail", parameter);
                return Json(new { success = true, message = "Delete successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
