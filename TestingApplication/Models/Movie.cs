using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestingApplication.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Range(1,5),Required]
        public int Rating { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}