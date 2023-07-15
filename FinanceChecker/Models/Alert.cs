using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

