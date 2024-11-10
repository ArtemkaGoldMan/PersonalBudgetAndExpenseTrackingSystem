using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PersonalBudgetTrackingSystem.Helper;
using PersonalBudgetTrackingSystem.Models;

namespace PersonalBudgetTrackingSystem.Controllers
{
    public class SavingsGoalsController : Controller
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\savingsGoals.json");

        public IActionResult Index()
        {
            var goals = JsonFileManager.LoadData<SavingsGoal>(_filePath);
            return View(goals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SavingsGoal goal)
        {
            var goals = JsonFileManager.LoadData<SavingsGoal>(_filePath);
            goal.Id = goals.Any() ? goals.Max(g => g.Id) + 1 : 1;
            goals.Add(goal);
            JsonFileManager.SaveData(_filePath, goals);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var goals = JsonFileManager.LoadData<SavingsGoal>(_filePath);
            var goal = goals.FirstOrDefault(g => g.Id == id);
            return View(goal);
        }

        [HttpPost]
        public IActionResult Edit(SavingsGoal goal)
        {
            var goals = JsonFileManager.LoadData<SavingsGoal>(_filePath);
            var existingGoal = goals.FirstOrDefault(g => g.Id == goal.Id);
            if (existingGoal != null)
            {
                existingGoal.Name = goal.Name;
                existingGoal.TargetAmount = goal.TargetAmount;
                existingGoal.CurrentAmount = goal.CurrentAmount;
                existingGoal.TargetDate = goal.TargetDate;
                JsonFileManager.SaveData(_filePath, goals);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var goals = JsonFileManager.LoadData<SavingsGoal>(_filePath);
            var goal = goals.FirstOrDefault(g => g.Id == id);
            if (goal != null)
            {
                goals.Remove(goal);
                JsonFileManager.SaveData(_filePath, goals);
            }
            return RedirectToAction("Index");
        }
    }
}
