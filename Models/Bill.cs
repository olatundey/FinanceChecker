using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class Bill
    {
        [Key]
        public int BillID { get; set; }

        //[ForeignKey("ApplicationUser")]
        public Guid UserID { get; set; }

        public string? BillName { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        //validation for future dates using the CustomValidation attribute
        //[DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        [CustomValidation(typeof(Bill), "ValidateFutureDate")]
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
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



