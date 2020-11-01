using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TechnologyGroup12.DataAccess.Repository.IRepository;
using TechnologyGroup12.Models.Models;

namespace TechnologyGroup12.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;  
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string? id)
        {
            Customer customer = new Customer();
            if (id == null)
            {
                return View(customer);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            customer = _unitOfWork.SP_Call.OneRecord<Customer>("SP_Get_Customer", parameters);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", customer.Name);
                parameter.Add("@Birth", customer.Birth);
                parameter.Add("@Gender", customer.Gender);
                parameter.Add("@Phone", customer.Phone);
                parameter.Add("@Email", customer.Email);
                parameter.Add("@Address", customer.Address);

                if (customer.Id.ToString() == "00000000-0000-0000-0000-000000000000") // Do là dạng Guid nên nó sẽ như vậy
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Customer", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", customer.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Customer", parameter);
                    return View(customer);
                }
            }
            return View(customer);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Customer>("SP_GetAll_Customer");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Customer", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }


    }
}
