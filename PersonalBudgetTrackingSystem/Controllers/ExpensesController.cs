using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly string _expenseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\expenses.json");
        private readonly string _categoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\budgetCategories.json");
        public IActionResult Index(string searchString, string sortOrder)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);

            // Searching by description
            if (!string.IsNullOrEmpty(searchString))
            {
                expenses = expenses.Where(e => e.Description!.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sorting
            switch (sortOrder)
            {
                case "amount_asc":
                    expenses = expenses.OrderBy(e => e.Amount).ToList();
                    break;
                case "amount_desc":
                    expenses = expenses.OrderByDescending(e => e.Amount).ToList();
                    break;
                case "date_asc":
                    expenses = expenses.OrderBy(e => e.Date).ToList();
                    break;
                case "date_desc":
                    expenses = expenses.OrderByDescending(e => e.Date).ToList();
                    break;
                default:
                    expenses = expenses.OrderBy(e => e.Id).ToList();
                    break;
            }

            return View(expenses);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);
            expense.Id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1;
            expenses.Add(expense);
            JsonFileManager.SaveData(_expenseFilePath, expenses);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            ViewBag.Categories = GetCategories();
            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);
            var existingExpense = expenses.FirstOrDefault(e => e.Id == expense.Id);
            if (existingExpense != null)
            {
                existingExpense.Description = expense.Description;
                existingExpense.Amount = expense.Amount;
                existingExpense.Category = expense.Category;
                existingExpense.Date = expense.Date;
                JsonFileManager.SaveData(_expenseFilePath, expenses);
            }
            return RedirectToAction("Index");
        }

        private List<string> GetCategories()
        {
            var categories = JsonFileManager.LoadData<BudgetCategory>(_categoryFilePath);
            return categories.Select(c => c.Name).ToList()!;
        }

        public IActionResult Delete(int id)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
                JsonFileManager.SaveData(_expenseFilePath, expenses);
            }
            return RedirectToAction("Index");
        }
    }
}
