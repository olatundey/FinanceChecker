using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [ForeignKey("Account")]
        public int AccountID { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Account Account { get; set; }
    }
}

