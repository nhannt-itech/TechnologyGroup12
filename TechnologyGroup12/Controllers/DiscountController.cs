using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TechnologyGroup12.DataAccess.Repository.IRepository;
using TechnologyGroup12.Models.Models;

namespace TechnologyGroup12.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string? id)
        {
            Discount discount = new Discount();
            if (id == null)
            {
                discount.StartDate = DateTime.Now;
                discount.EndDate = DateTime.Now;
                return View(discount);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            discount = _unitOfWork.SP_Call.OneRecord<Discount>("SP_Get_Discount", parameters);
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Discount discount)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();

                parameter.Add("Id", discount.Id);
                parameter.Add("@Name", discount.Name);
                parameter.Add("@Description", discount.Description);
                parameter.Add("@DiscountValue", discount.DiscountValue);
                parameter.Add("@StartDate", discount.StartDate);
                parameter.Add("@EndDate", discount.EndDate);

                var lDiscount = _unitOfWork.SP_Call.List<Discount>("SP_GetAll_Discount");
                int count = lDiscount.Count(x => x.Id == discount.Id); //Kiểm tra đã có mã này chưa.

                if (count == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Discount", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", discount.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Discount", parameter);
                    return View(discount);
                }
            }
            return View(discount);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Discount>("SP_GetAll_Discount");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpGet]
        public IActionResult SearchFor(string columnName, string searchFor)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ColumnName", columnName);
            parameter.Add("@SearchFor", searchFor);
            var allObj = _unitOfWork.SP_Call.List<Discount>("SP_Search_Discount", parameter);
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Discount", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckLimitDiscount(float DiscountValue)
        {
            bool check = _unitOfWork.SP_Call.ExecuteScalar<bool>(@"SELECT dbo.FUNC_CheckLimitDicount( @DiscountValue )", new object[] { DiscountValue });
            return Json(check);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckDateDiscount(DateTime StartDate, DateTime EndDate)
        {
            bool check;
            try
            {
                check = _unitOfWork.SP_Call.ExecuteScalar<bool>(@"SELECT dbo.FUNC_CheckDateDiscount( @StartDate , @EndDate )", new object[] { StartDate, EndDate });
            }
            catch
            {
                check = false;
            }
            return Json(check);
        }
    }
}
