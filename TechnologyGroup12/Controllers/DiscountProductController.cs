using System;
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
    public class DiscountProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Insert(string? id)
        {
            var lProduct = _unitOfWork.SP_Call.List<Product>("SP_GetAll_Product");
            DiscountProduct discountProduct = new DiscountProduct()
            {
                DiscountId = id, //id của Discount
                ProductList = lProduct.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(discountProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insert(DiscountProduct discountProduct)
        {
            var lProduct = _unitOfWork.SP_Call.List<Product>("SP_GetAll_Product");
            discountProduct.ProductList = lProduct.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@DiscountId", discountProduct.DiscountId);
                parameter.Add("@ProductId", discountProduct.ProductId);

                _unitOfWork.SP_Call.Excute("SP_Create_Discount_Product", parameter);
            }
            return RedirectToAction("Upsert", "Discount", new { id = discountProduct.DiscountId });
        }

        [HttpGet]
        public IActionResult GetAllDiscountProductOfDiscount(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@DiscountId", id);
            var allObj = _unitOfWork.SP_Call.List<DiscountProductListVM>("SP_GetAll_Discount_Product_Of_Discount", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? discountId, long productId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@DiscountId", discountId);
            parameter.Add("@ProductId", productId);

            _unitOfWork.SP_Call.Excute("SP_Delete_Discount_Product", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckProductNotDiscount(long ProductId)
        {
            bool check = _unitOfWork.SP_Call.ExecuteScalar<bool>(@"SELECT dbo.FUNC_CheckProduct_NotDiscount( @ProductId )", new object[] { ProductId });
            return Json(check);
        }
    }
}
