using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinanceChecker.Models
{
    public class Videos
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? VideoUrl { get; set; }

        //public string? VideoFilePath { get; set; }

        //public string? VideoMimeType { get; set; }
    }

}

