using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookShelf.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AuthorId { get; set; }
        public Author Author { get; set; }
        public int Pages { get; set; }  
        public int Mark { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Image { get; set; }
        public int Reread { get; set; }
    }
}