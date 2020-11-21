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
    public class DependentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DependentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Update(string? id)
        {
            Dependents dependents = new Dependents();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            dependents = _unitOfWork.SP_Call.OneRecord<Dependents>("SP_Get_Dependents", parameters);
            return View("Upsert", dependents);
        }
        public IActionResult Insert(string? id)
        {
            Dependents dependents = new Dependents()
            {
                EmployeeId = Guid.Parse(id)
            };
            return View("Upsert", dependents);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Dependents dependents)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", dependents.Name);
                parameter.Add("@Birth", dependents.Birth);
                parameter.Add("@Gender", dependents.Gender);
                parameter.Add("@Phone", dependents.Phone);
                parameter.Add("@Relationship", dependents.Relationship);
                parameter.Add("@EmployeeId", dependents.EmployeeId);


                if (dependents.Id.ToString() == "00000000-0000-0000-0000-000000000000") // Do là dạng Guid nên nó sẽ như vậy
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Dependents", parameter);
                    return RedirectToAction("Upsert", "Employee", new { id = dependents.EmployeeId.ToString() });
                }
                else
                {
                    parameter.Add("@Id", dependents.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Dependents", parameter);
                    return View(dependents);
                }
            }
            return View(dependents);
        }

        [HttpGet]
        public IActionResult GetAllDependentsOfEmployee(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EmployeeId", id);
            var allObj = _unitOfWork.SP_Call.List<Dependents>("SP_GetAll_Dependents_Of_Employee", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Dependents", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
