using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Support.UI;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        MasterDBContext dBContext;
        public CategoryController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> Index()
        {
            var Categories = await dBContext.Categories.Where(c => c.CategoryId == null).ToListAsync();
            ViewBag.Categories = await dBContext.Categories.Where(c => c.CategoryId != null).ToListAsync();

            return View(Categories);
        }

        public async Task<IActionResult> AddCategory(int? id)
        {
            if (id != null)
            {
                var category = await dBContext.Categories.FindAsync(id);
                return View(category);
            }

            return View(new Category());
        }

        public async Task<IActionResult> AddSubCategory(int? id)
        {
            ViewBag.Categories = await dBContext.Categories.Where(c => c.CategoryId == null).ToListAsync();

            if (id != null)
            {
                var category = await dBContext.Categories.FindAsync(id);
                return View(category);
            }

            return View(new Category());
        }


        public async Task<IActionResult> SaveCategory(CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                if(model.CategoryId == null)
                {
                    TempData["message"] = "حدث خطأ اثناء حفظ البيانات يرجى المحاولة مجددا";
                    return RedirectToAction("AddSubCategory");
                }

                TempData["message"] = "حدث خطأ اثناء حفظ البيانات يرجى المحاولة مجددا";
                return RedirectToAction("AddCategory");
            }

            var catego = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
            };

            await dBContext.Categories.AddAsync(catego);    
            await dBContext.SaveChangesAsync();

            TempData["message"] = "تم حفظ البيانات بنجاح";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> deleteCategory(int iidd)
        {
            var category = await dBContext.Categories.FindAsync(iidd);

            if (category != null) {

                dBContext.Categories.Remove(category);
                await dBContext.SaveChangesAsync();
                TempData["message"] = "تم حذف الفئة بنجاح";
                return RedirectToAction("Index");
            }

            TempData["message"] = "حدث خطأ اثناء حذف الفئة يرجى المحاولة مجددا ";
            return RedirectToAction("Index");
        }
    }

    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
    }

}
