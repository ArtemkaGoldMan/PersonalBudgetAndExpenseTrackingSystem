using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class IncomeController : Controller
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\income.json");

        public IActionResult Index()
        {
            var incomeList = JsonFileManager.LoadData<Income>(_filePath);

            // Group the income by month and year for chart data
            var monthlyIncome = incomeList
                .GroupBy(i => new { i.Date.Year, i.Date.Month })
                .Select(g => new
                {
                    YearMonth = $"{g.Key.Month}/{g.Key.Year}",
                    TotalAmount = g.Sum(i => i.Amount)
                })
                .OrderBy(i => i.YearMonth)
                .ToList();

            // Return both the income list for the table and the monthly income data for the chart
            var model = new
            {
                IncomeList = incomeList,
                MonthlyIncome = monthlyIncome
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Income income)
        {
            var incomeList = JsonFileManager.LoadData<Income>(_filePath);
            income.Id = incomeList.Any() ? incomeList.Max(i => i.Id) + 1 : 1;
            incomeList.Add(income);
            JsonFileManager.SaveData(_filePath, incomeList);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var incomeList = JsonFileManager.LoadData<Income>(_filePath);
            var income = incomeList.FirstOrDefault(i => i.Id == id);
            return View(income);
        }

        [HttpPost]
        public IActionResult Edit(Income income)
        {
            var incomeList = JsonFileManager.LoadData<Income>(_filePath);
            var existingIncome = incomeList.FirstOrDefault(i => i.Id == income.Id);
            if (existingIncome != null)
            {
                existingIncome.Source = income.Source;
                existingIncome.Amount = income.Amount;
                existingIncome.Date = income.Date;
                JsonFileManager.SaveData(_filePath, incomeList);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var incomeList = JsonFileManager.LoadData<Income>(_filePath);
            var income = incomeList.FirstOrDefault(i => i.Id == id);
            if (income != null)
            {
                incomeList.Remove(income);
                JsonFileManager.SaveData(_filePath, incomeList);
            }
            return RedirectToAction("Index");
        }

       
    }
}
