using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class BudgetCategoriesController : Controller
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\budgetCategories.json");
        public IActionResult Index()
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_filePath);
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BudgetCategory category)
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_filePath);
            category.Id = categories.Any() ? categories.Max(c => c.Id) + 1 : 1;
            categories.Add(category);
            JsonFileManager.SaveData(_filePath, categories);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_filePath);
            var category = categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(BudgetCategory category)
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_filePath);
            var existingCategory = categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.BudgetLimit = category.BudgetLimit;
                JsonFileManager.SaveData(_filePath, categories);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_filePath);
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                categories.Remove(category);
                JsonFileManager.SaveData(_filePath, categories);
            }
            return RedirectToAction("Index");
        }
    }
}
