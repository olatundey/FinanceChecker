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
        public string UserID { get; set; }

        public int AccountNumber { get; set; }

        public string? AccountType { get; set; }

        public string? syncType { get; set; }

        public string? InstitutionName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Range(typeof(decimal), "-9999999.99", "9999999.99",
            ErrorMessage = "Balance must be between -9999999.99 and 9999999.99")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        //[ForeignKey("Id")]
        //public virtual ApplicationUser? ApplicationUser { get; set; }

        //public virtual ICollection<Transaction>? Transactions { get; set; }

    }
}

