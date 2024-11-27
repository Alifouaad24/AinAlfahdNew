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
            var Categories = await dBContext.Categories.Where(c => c.MainCategoryId == null).ToListAsync();
            ViewBag.Categories = await dBContext.Categories.Where(c => c.MainCategoryId != null).ToListAsync();

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
            ViewBag.Categories = await dBContext.Categories.Where(c => c.MainCategoryId == null).ToListAsync();

            if (id != null)
            {
                var category = await dBContext.Categories.FindAsync(id);
                return View(category);
            }

            return View(new Category());
        }

        public async Task<IActionResult> delCategory()
        {
            var Categories = await dBContext.Categories.ToListAsync();

            return View(Categories);
        }


        public async Task<IActionResult> SaveCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                if (model.MainCategoryId == null)
                {
                    TempData["message"] = "حدث خطأ اثناء حفظ البيانات يرجى المحاولة مجددا";
                    return RedirectToAction("AddSubCategory");
                }

                TempData["message"] = "حدث خطأ اثناء حفظ البيانات يرجى المحاولة مجددا";
                return RedirectToAction("AddCategory");
            }

            if (model.CategoryId == 0)
            {
                var catego = new Category
                {
                    MainCategoryId = model.MainCategoryId,
                    CategoryName = model.CategoryName,
                };

                await dBContext.Categories.AddAsync(catego);
                await dBContext.SaveChangesAsync();

                TempData["message"] = "تم حفظ البيانات بنجاح";
                return RedirectToAction("Index");
            }

            else
            {
                var cate = await dBContext.Categories.FindAsync(model.CategoryId);

                cate.MainCategoryId = model.MainCategoryId;
                cate.CategoryName = model.CategoryName;

                dBContext.Categories.Update(cate);
                await dBContext.SaveChangesAsync();

                TempData["message"] = "تم تعديل البيانات بنجاح";
                return RedirectToAction("delCategory");


            }
        }


        public async Task<IActionResult> deleteCategory(int iidd)
        {
            var category = await dBContext.Categories.FindAsync(iidd);

            if (category != null)
            {

                dBContext.Categories.Remove(category);
                await dBContext.SaveChangesAsync();
                TempData["message"] = "تم حذف الفئة بنجاح";
                return RedirectToAction("delCategory");
            }

            TempData["message"] = "حدث خطأ اثناء حذف الفئة يرجى المحاولة مجددا ";
            return RedirectToAction("delCategory");
        }
    }
}
