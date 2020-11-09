using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using TechnologyGroup12.DataAccess.Repository.IRepository;
using TechnologyGroup12.Models.Models;
using TechnologyGroup12.Models.ViewModels;

namespace TechnologyGroup12.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(long? id)
        {

            var lCategory = _unitOfWork.SP_Call.List<Category>("SP_GetAll_Category");
            var lManufacturer = _unitOfWork.SP_Call.List<Manufacturer>("SP_GetAll_Manufacturer");
            Product product = new Product()
            {
                CategoryList = lCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ManufacturerList = lManufacturer.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                OnSale = true
            };
            if (id == null)
            {
                return View(product);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            product = _unitOfWork.SP_Call.OneRecord<Product>("SP_Get_Product", parameters);
            // Sau bước này sẽ Reset SelectListItem nên ta phải set lại cho nó.
            product.CategoryList = lCategory.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            product.ManufacturerList = lManufacturer.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"image\product");//"C:\\Users\\MayXauGiaCao\\Desktop\\Rynoz.EShop\\Rynoz.EShop\\wwwroot"
                    var extension = Path.GetExtension(files[0].FileName); // ".png"

                    if (product.ImgUrl != null)
                    {
                        //this is an edit and we need to remove old image
                        var imagePath = Path.Combine(webRootPath, product.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        //uploads: "C:\\Users\\MayXauGiaCao\\Desktop\\Rynoz.EShop\\Rynoz.EShop\\wwwroot\\images\\products"
                        //filename: "6a9c2297-0ef2-4edf-938f-c19971ce8e26"
                        //extension: ".png"
                        files[0].CopyTo(filesStreams);
                    }
                    product.ImgUrl = @"\image\product\" + fileName + extension;
                }

                var parameter = new DynamicParameters();

                parameter.Add("@Name", product.Name);
                parameter.Add("@Description", product.Description);
                parameter.Add("@ImgUrl", product.ImgUrl);
                parameter.Add("@NumberInStock", product.NumberInStock);
                parameter.Add("@Price", product.Price);
                parameter.Add("@ManufacturerId", product.ManufacturerId);
                parameter.Add("@CategoryId", product.CategoryId);

                var lCategory = _unitOfWork.SP_Call.List<Category>("SP_GetAll_Category");
                var lManufacturer = _unitOfWork.SP_Call.List<Manufacturer>("SP_GetAll_Manufacturer");
                product.CategoryList = lCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                product.ManufacturerList = lManufacturer.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if (product.Id == 0)
                {
                    _unitOfWork.SP_Call.Excute("SP_Create_Product", parameter);
                    return RedirectToAction("Index");
                }
                else
                {
                    parameter.Add("@Id", product.Id);
                    _unitOfWork.SP_Call.Excute("SP_Update_Product", parameter);
                    return View(product);
                }
            }
            return View(product);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<ProductListVM>("SP_GetAll_Product_View");
            return Json(new { data = allObj.AsEnumerable() });
        }

        [HttpDelete]
        public IActionResult Delete(long? id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            _unitOfWork.SP_Call.Excute("SP_Delete_Product", parameter);
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
