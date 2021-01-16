using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookShelf.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; } //если переименовать переменную и возвратить её из вью?
        public ICollection<Book> Books { get; set; }
        public Author()
        {
            Books = new List<Book>();
        }
    }
}