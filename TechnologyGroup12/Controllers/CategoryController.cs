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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(long? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            category = _unitOfWork.SP_Call.OneRecord<Category>("SP_Get_Category", parameters);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Name", category.Name);
            parameter.Add("@CategoryId", category.CategoryId);

            try
            {
                if (category.Id == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Category", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", category.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Category", parameter);
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(category);
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Category>("SP_GetAll_Category");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpGet]
        public IActionResult SearchFor(string columnName, string searchFor)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ColumnName", columnName);
            parameter.Add("@SearchFor", searchFor);
            var allObj = _unitOfWork.SP_Call.List<Category>("SP_Search_Category", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(long? id)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                _unitOfWork.SP_Call.Excute("SP_Delete_Category", parameter);
                return Json(new { success = true, message = "Delete successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
