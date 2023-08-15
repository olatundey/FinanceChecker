using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{

    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Display(Name = "ApplicationUser")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Number(s) required for Account.")]
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "Account type is required.")]
        public string? AccountType { get; set; }

        public string? syncType { get; set; }

        [Required(ErrorMessage = "Institution name is required.")]
        public string? InstitutionName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Range(typeof(decimal), "-9999999.99", "9999999.99",
            ErrorMessage = "Balance must be between -9999999.99 and 9999999.99")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }


        [NotMapped] //property is not mapped to the database
        public Dictionary<DateTime, decimal>? BalanceForDay { get; set; }

    }
}

