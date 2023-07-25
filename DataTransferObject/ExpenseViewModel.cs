using System;
namespace FinanceChecker.DataTransferObject
{
    public class ExpenseViewModel
    {
       
        public decimal CurrentWeekTotal { get; set; }
        public decimal CurrentMonthTotal { get; set; }
        public decimal PreviousWeekTotal { get; set; }
        public decimal PreviousMonthTotal { get; set; }
        public Dictionary<string, decimal>? CategoryMonthlySpent { get; set; }
        public Dictionary<string, decimal>? CategoryWeeklySpent { get; set; }
        public Dictionary<string, decimal>? CategoryPreviousWeekSpent { get; set; }
        public Dictionary<string, decimal>? CategoryPreviousMonthSpent { get; set; }
        public Dictionary<string, decimal>? ComparePreviousWeeks { get; set; }
        public Dictionary<string, decimal>? ComparePreviousMonths { get; set; }
    }
}
