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

        [HttpPost]
        public IActionResult Upsert(Account account)
        {
            var lEmployee = _unitOfWork.SP_Call.List<Employee>("SP_GetAll_Employee");
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@LGNAME", account.Username);
                parameter.Add("@USERNAME", account.Username);
                parameter.Add("@PASS", account.Password);
                parameter.Add("@ROLE", account.Role.ToString());
                _unitOfWork.SP_Call.Excute("dbo.USP_CreateUser", parameter);

                var parameter1 = new DynamicParameters();
                parameter1.Add("@Username", account.Username);
                parameter1.Add("@Password", account.Password);
                parameter1.Add("@Role", account.Role);
                parameter1.Add("@EmployeeId", account.EmployeeId);
                _unitOfWork.SP_Call.Excute("SP_Create_Account", parameter1);

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                account.EmployeeList = lEmployee.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(account);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Account>("SP_GetAll_Account");
            return Json(new { data = allObj.AsEnumerable() });
        }

    }
}
