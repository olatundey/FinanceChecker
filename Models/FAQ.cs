using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class FAQ
    {
        public int Id { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }

    }

}

