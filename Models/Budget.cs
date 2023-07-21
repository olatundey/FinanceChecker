using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class Budget
    {
        [Key]
        public int BudgetID { get; set; }

        //[ForeignKey("ApplicationUser")]
        public Guid UserID { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        //[ForeignKey("Category")]
        public int CategoryID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Budget amount must be a non-negative value.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        public decimal TotalTransactionsAmount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public decimal Progress { get; set; }


    }
}

