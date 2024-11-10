using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "expenses.json");

        public IActionResult Index()
        {
            var expenses = JsonFileManager.LoadData<Expense>(_filePath);
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_filePath);
            expense.Id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1;
            expenses.Add(expense);
            JsonFileManager.SaveData(_filePath, expenses);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_filePath);
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_filePath);
            var existingExpense = expenses.FirstOrDefault(e => e.Id == expense.Id);
            if (existingExpense != null)
            {
                existingExpense.Description = expense.Description;
                existingExpense.Amount = expense.Amount;
                existingExpense.Category = expense.Category;
                existingExpense.Date = expense.Date;
                JsonFileManager.SaveData(_filePath, expenses);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var expenses = JsonFileManager.LoadData<Expense>(_filePath);
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
                JsonFileManager.SaveData(_filePath, expenses);
            }
            return RedirectToAction("Index");
        }
    }
}
