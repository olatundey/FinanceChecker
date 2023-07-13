using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinanceChecker.Models
{
	public class ApplicationUser : IdentityUser
    {
            [Required]
            public string? FirstName { get; set; }
            [Required]
            public string? LastName { get; set; }
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? PostalCode { get; set; }
            public string? Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}

