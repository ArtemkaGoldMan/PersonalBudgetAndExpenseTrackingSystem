namespace PersonalBudgetTrackingSystem.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal BudgetLimit { get; set; }
    }

}
