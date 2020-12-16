using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Employee employee = new Employee()
            {
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
                return View(employee);
            }
            else
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                employee = _unitOfWork.SP_Call.OneRecord<Employee>("SP_Get_Employee", parameters);
                employee.GenderList = lGender.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                });
                employee.JobPositionList = lJobPosition.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Employee employee)
        {
            List<string> lGender = new List<string>();
            lGender.Add("Male");
            lGender.Add("Female");
            lGender.Add("Others");

            employee.GenderList = lGender.Select(i => new SelectListItem
            {
                Text = i,
                Value = i
            });

            var lJobPosition = _unitOfWork.SP_Call.List<JobPosition>("SP_GetAll_JobPosition");
            employee.JobPositionList = lJobPosition.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            var parameter = new DynamicParameters();
            parameter.Add("@Name", employee.Name);
            parameter.Add("@Birth", employee.Birth);
            parameter.Add("@Gender", employee.Gender);
            parameter.Add("@Phone", employee.Phone);
            parameter.Add("@Email", employee.Email);
            parameter.Add("@Address", employee.Address);
            parameter.Add("@JobPositionId", employee.JobPositionId);
            try
            {
                if (employee.Id.ToString() == "00000000-0000-0000-0000-000000000000") // Do là dạng Guid nên nó sẽ như vậy
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Employee", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", employee.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Employee", parameter);
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.
                    Replace("The transaction ended in the trigger. The batch has been aborted",""));
                return View(employee);
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<EmployeeListVM>("SP_GetAll_Employee_View");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpGet]
        public IActionResult SearchFor(string columnName, string searchFor)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ColumnName", columnName);
            parameter.Add("@SearchFor", searchFor);
            var allObj = _unitOfWork.SP_Call.List<EmployeeListVM>("SP_Search_Employee", parameter);
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
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                _unitOfWork.SP_Call.Excute("SP_Delete_Employee", parameter);
                return Json(new { success = true, message = "Delete successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckAge(DateTime Birth)
        {
            try
            {
                bool check = _unitOfWork.SP_Call.ExecuteScalar<bool>(@"SELECT dbo.FUNC_CheckAge( @Birth )", new object[] { Birth });
                return Json(check);
            }
            catch
            {
                return Json(0);
            }
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckEmail(string Email)
        {
            if (Email == null) Email = "";
            bool check = _unitOfWork.SP_Call.ExecuteScalar<bool>(@"SELECT dbo.FUNC_CheckEmail( @Email )", new object[] { Email });
            return Json(check);
        }
    }
}
