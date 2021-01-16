using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookShelf.Models
{
    public class BookContext:DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<BookContext>(null);
        //    base.OnModelCreating(modelBuilder);
        //}
        //Database.SetInitializer<BookContext>(null);
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

    }
}