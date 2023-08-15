using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class Savings
    {
        [Key]
        public int SavingsID { get; set; }

        public Guid UserID { get; set; }

        [Required]
        public string? SavingsName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Savings amount must be a non-negative value.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Goal { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Savings amount must be a non-negative value.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal CurrentSavings { get; set; }

        [Required]
        public decimal Progress { get; set; }

        //validation for future dates using the CustomValidation attribute
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        [CustomValidation(typeof(Savings), "ValidateFutureDate")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        public static ValidationResult ValidateFutureDate(DateTime date, ValidationContext context)
        {
            if (date.Date < DateTime.Now.Date)
            {
                return new ValidationResult("Due date must be a future date.");
            }
            return ValidationResult.Success;
        }
    }
}

