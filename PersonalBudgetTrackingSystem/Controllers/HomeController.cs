using Microsoft.AspNetCore.Mvc;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;
using System.Diagnostics;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _expenseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\expenses.json");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetExpenseReportData()
        {
            var expenses = JsonFileManager.LoadData<Expense>(_expenseFilePath);

            // Monthly Totals
            var monthlyTotals = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderBy(m => m.Month)
                .ToList();

            // Category Totals
            var categoryTotals = expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key ?? "Uncategorized",  // Default to "Uncategorized" if null
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToList();

            return Json(new { monthlyTotals, categoryTotals });
        }

    }
}
