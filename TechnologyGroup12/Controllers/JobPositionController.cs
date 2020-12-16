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
    public class JobPositionController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public JobPositionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(long? id)
        {
            JobPosition jobPosition = new JobPosition();
            if (id == null)
            {
                return View(jobPosition);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            jobPosition = _unitOfWork.SP_Call.OneRecord<JobPosition>("SP_Get_JobPosition", parameters);
            return View(jobPosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(JobPosition jobPosition)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Name", jobPosition.Name);
            parameter.Add("@BasicSalary", jobPosition.BasicSalary);
            parameter.Add("@Salary", jobPosition.Salary);
            try
            {
                if (jobPosition.Id == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_JobPosition", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", jobPosition.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_JobPosition", parameter);
                    return View(jobPosition);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(jobPosition);
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<JobPosition>("SP_GetAll_JobPosition");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpGet]
        public IActionResult SearchFor(string columnName, string searchFor)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ColumnName", columnName);
            parameter.Add("@SearchFor", searchFor);
            var allObj = _unitOfWork.SP_Call.List<JobPosition>("SP_Search_JobPosition", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(long? id)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                _unitOfWork.SP_Call.Excute("SP_Delete_JobPosition", parameter);
                return Json(new { success = true, message = "Delete successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Delete False!" });
            }
        }
    }
}
