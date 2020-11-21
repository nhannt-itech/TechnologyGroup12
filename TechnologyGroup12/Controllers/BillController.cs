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
    public class BillController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(long? id)
        {
            var lEmployee = _unitOfWork.SP_Call.List<Employee>("SP_GetAll_Employee");
            var lCustomer = _unitOfWork.SP_Call.List<Customer>("SP_GetAll_Customer");

            Bill bill = new Bill()
            {
                EmployeeList = lEmployee.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CustomerList = lCustomer.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null)
            {
                return View(bill);
            }
            else
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                bill = _unitOfWork.SP_Call.OneRecord<Bill>("SP_Get_Bill", parameters);
                bill.EmployeeList = lEmployee.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                bill.CustomerList = lCustomer.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(bill);
            }    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Bill bill)
        {
            var lEmployee = _unitOfWork.SP_Call.List<Employee>("SP_GetAll_Employee");
            var lCustomer = _unitOfWork.SP_Call.List<Customer>("SP_GetAll_Customer");

            bill.EmployeeList = lEmployee.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            bill.CustomerList = lCustomer.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@CustomerId", bill.CustomerId);
                parameter.Add("@EmployeeId", bill.EmployeeId);
                parameter.Add("@TotalPriceBill", bill.TotalPriceBill);

                if (bill.Id == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Bill", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", bill.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Bill", parameter);
                    return View(bill);
                }
            }
            return View(bill);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<BillListVM>("SP_GetAll_Bill_View");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(long? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Bill", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
