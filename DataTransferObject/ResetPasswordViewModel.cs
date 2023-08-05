using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceChecker.DataTransferObject
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }
    }

}


