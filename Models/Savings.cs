//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//namespace FinanceChecker.Models
//{
//	public class Savings
//	{
//            [Key]
//            public int SavingsID { get; set; }

//            [ForeignKey("ApplicationUser")]
//            public string UserID { get; set; }

//            public decimal Goal { get; set; }

//            public decimal Progress { get; set; }

//            [Required]
//            public DateTime CreatedAt { get; set; }

//            [Required]
//            public DateTime UpdatedAt { get; set; }

//            public virtual ApplicationUser ApplicationUser { get; set; }
//        }
//    }
