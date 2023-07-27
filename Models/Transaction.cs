﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Display(Name = "ApplicationUser")]
        public string UserID { get; set; }

        //[ForeignKey("Account")]
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Institution name is required.")]
        public string? InstitutionName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? Category { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        //public virtual ApplicationUser User { get; set; }

        //public virtual Account? Account { get; set; }
        //public virtual ApplicationUser? ApplicationUser { get; set;} 
    }
}

