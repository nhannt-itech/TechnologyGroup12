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
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string? id)
        {
            List<string> lGender = new List<string>();
            lGender.Add("Male");
            lGender.Add("Female");
            lGender.Add("Others");

            var lJobPosition = _unitOfWork.SP_Call.List<JobPosition>("SP_GetAll_JobPosition");
            EmployeeVM employeeVM = new EmployeeVM()
            {
                Employee = new Employee(),
                JobPositionList = lJobPosition.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                GenderList = lGender.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
        };

            

            if (id == null)
            {
                return View(employeeVM);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            employeeVM.Employee = _unitOfWork.SP_Call.OneRecord<Employee>("SP_Get_Employee", parameters);
            return View(employeeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EmployeeVM employeeVM)
        {
                    List<string> lGender = new List<string>();
            lGender.Add("Male");
            lGender.Add("Female");
            lGender.Add("Others");

            employeeVM.GenderList = lGender.Select(i => new SelectListItem
            {
                Text = i,
                Value = i
            });

            var lJobPosition = _unitOfWork.SP_Call.List<JobPosition>("SP_GetAll_JobPosition");
            employeeVM.JobPositionList = lJobPosition.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", employeeVM.Employee.Name);
                parameter.Add("@Birth", employeeVM.Employee.Birth);
                parameter.Add("@Gender", employeeVM.Employee.Gender);
                parameter.Add("@Phone", employeeVM.Employee.Phone);
                parameter.Add("@Email", employeeVM.Employee.Email);
                parameter.Add("@Address", employeeVM.Employee.Address);
                parameter.Add("@JobPositionId", employeeVM.Employee.JobPositionId);

                if (employeeVM.Employee.Id.ToString() == "00000000-0000-0000-0000-000000000000") // Do là dạng Guid nên nó sẽ như vậy
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Employee", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", employeeVM.Employee.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Employee", parameter);
                    return View(employeeVM);
                }
            }
            return View(employeeVM);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<EmployeeListVM>("SP_GetAll_Employee_View");
            return Json(new { data = allObj.AsEnumerable() });
        }

        public IActionResult GetJob(string? id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var Obj = _unitOfWork.SP_Call.OneRecord<Employee>("SP_Get_Employee", parameters);
            return Json(new { data = Obj });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Employee", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
