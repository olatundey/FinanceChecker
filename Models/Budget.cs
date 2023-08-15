using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class Budget
    {
        [Key]
        public int BudgetID { get; set; }

        public Guid UserID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please enter the amount.")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget amount must be a non-negative value.")]
        [DataType(DataType.Currency)] public decimal Amount { get; set; }
        public decimal TotalTransactionsAmount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public decimal Progress { get; set; }


    }
}

