using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechnologyGroup12.DataAccess.Repository.IRepository;
using TechnologyGroup12.Models.Models;

namespace TechnologyGroup12.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string? id)
        {
            var lEmployee = _unitOfWork.SP_Call.List<Employee>("SP_GetAll_Employee");
            Account account = new Account()
            {
                EmployeeList = lEmployee.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null)
            {
                return View(account);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Username", id);
            account = _unitOfWork.SP_Call.OneRecord<Account>("SP_Get_Account", parameters);
            account.EmployeeList = lEmployee.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(account);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Account>("SP_GetAll_Account");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Username", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Account", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
