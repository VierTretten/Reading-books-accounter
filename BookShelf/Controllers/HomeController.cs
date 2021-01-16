using BookShelf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShelf.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();

        public ActionResult Index()
        {
            return View(db.Books);
        }
        [HttpGet]
        public ActionResult Add()
        {
            SelectList authors = new SelectList(db.Authors, "Id", "Name");
            ViewBag.Authors = authors;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            Book book = db.Books.Find(id);
            if (book == null) return HttpNotFound();
            Author author = db.Authors.FirstOrDefault(p => p.AuthorId == book.AuthorId);
            //SelectList authors = new SelectList(db.Authors, "Id", "Name", book.AuthorId);
            //ViewBag.Authors = authors;
            if (author != null) ViewData["AuthorName"] = author.AuthorName;
            else ViewData["AuthorName"] = null; //!!!!!!!!!!!!!!!!!!!!!!!!!!
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (book.Mark > 10) book.Mark = 10;
            if (book.Mark < 0) book.Mark = 0;
            string authorName = Request.Form["AuthorName"]; // Найти АйДи по имени и изменить AuthorId у Book, удалив прошлые поля. Это если автор уже существует.
            //book.AuthorId = book.Author.AuthorId;
            var author = db.Authors.FirstOrDefault(p => p.AuthorName == authorName);
            var newBook = db.Books.Find(book.Id);
            newBook.Name = book.Name;
            newBook.Pages = book.Pages;
            newBook.Mark = book.Mark;
            newBook.StartDate = book.StartDate;
            newBook.EndDate = book.EndDate;
            newBook.Image = book.Image;
            newBook.Reread = book.Reread;
            if (author != null)
            {
                newBook.AuthorId = author.AuthorId;
                newBook.Author = author;   
            }
            else
            {
                var books = new List<Book>() {newBook} ;
                Author newAuthor = new Author { AuthorName = authorName, Books = books };
                newBook.AuthorId = newAuthor.AuthorId;
                newBook.Author = newAuthor;
                db.Authors.Add(newAuthor);
                db.Entry(newBook).State = EntityState.Modified;
                //db.Entry(book).State = EntityState.Modified;
            }
            
            db.SaveChanges();
            string view = "View/" + book.Id;
            return RedirectToAction(view);
        }
        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    Book b = db.Books.Find(id);
        //    if (b == null) return HttpNotFound();
        //    return View(b);
        //}
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Book b = db.Books.Find(id);
        //    if (b == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    db.Books.Remove(b);
        //    db.SaveChanges();
        //    string view = "View/" + b.Id;
        //    return RedirectToAction("Index");
        //}
        public ActionResult Delete(int id)
        {
            Book b = db.Books.Find(id);
            if (b != null)
            {
                db.Books.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult View(int? id)
        {
            if (id == null) return HttpNotFound();
            Book book = db.Books.Find(id);
            if (book == null) return HttpNotFound();
            return View(book);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}