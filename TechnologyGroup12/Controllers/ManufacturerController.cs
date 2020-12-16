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
    public class ManufacturerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManufacturerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(long? id)
        {
            Manufacturer manufacturer = new Manufacturer();
            if (id == null)
            {
                return View(manufacturer);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            manufacturer = _unitOfWork.SP_Call.OneRecord<Manufacturer>("SP_Get_Manufacturer", parameters);
            return View(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Manufacturer manufacturer)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", manufacturer.Name);
                parameter.Add("@Nation", manufacturer.Nation);
                parameter.Add("@Phone", manufacturer.Phone);
                parameter.Add("@Email", manufacturer.Email);

                if (manufacturer.Id == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Manufacturer", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", manufacturer.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Manufacturer", parameter);
                    return View(manufacturer);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(manufacturer);
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Manufacturer>("SP_GetAll_Manufacturer");

            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpGet]
        public IActionResult SearchFor(string columnName, string searchFor)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ColumnName", columnName);
            parameter.Add("@SearchFor", searchFor);
            var allObj = _unitOfWork.SP_Call.List<Manufacturer>("SP_Search_Manufacturer", parameter);
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
                return Json(new { success = false, message = "Delete False!" });
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
