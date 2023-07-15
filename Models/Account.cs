using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{
    public enum AccountType
    {
        Investment,
        CreditCard,
        BankAccount
    }

    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string InstitutionName { get; set; }

        public decimal Balance { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

