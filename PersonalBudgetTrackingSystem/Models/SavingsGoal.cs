namespace PersonalBudgetTrackingSystem.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
    }

}
