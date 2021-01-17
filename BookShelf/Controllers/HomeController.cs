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
        public ActionResult IndexAuthor()
        {
            var authors = db.Authors.Include(a => a.Books).ToList();
            return View(authors);
        }
        [HttpGet]
        public ActionResult AddBook()
        {
            SelectList authors = new SelectList(db.Authors, "Id", "Name");
            ViewBag.Authors = authors;
            return View();
        }
        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            var authorInDB = db.Authors
                .Include(a=>a.Books)
                .FirstOrDefault(a => a.AuthorName == book.Author.AuthorName);
            //при существовании автора с тем же именем, присваивает полученной модели автора существующую 
            if (authorInDB != null)
            {
                book.Author = authorInDB;
            }
            db.Books.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AddAuthor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAuthor(Author author, List<Book> books)
        {
            var authorInDB = db.Authors.FirstOrDefault(a => a.AuthorName == author.AuthorName);
            //при существовании автора с тем же именем, присваивает полученной модели автора существующую 
            if (authorInDB != null)
            {
                authorInDB.Books = author.Books;
                author = authorInDB;
            }
            foreach (var book in author.Books)
            {
                book.AuthorId = author.AuthorId;
                book.Author = author;
                db.Books.Add(book);

            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            //Book book = db.Books.Find(id);
            var book = db.Books
                .Include(b=>b.Author)
                .FirstOrDefault(b => b.Id == id);
            if (book == null) return HttpNotFound();
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (book.Mark > 10) book.Mark = 10;
            if (book.Mark < 0) book.Mark = 0;
            string authorName = Request.Form["AuthorName"];
            var author = db.Authors.FirstOrDefault(p => p.AuthorName == authorName);
            if (author != null)
            {
                book.AuthorId = author.AuthorId;
                book.Author = author;
            }
            else
            {
                book.Author = new Author { AuthorName = authorName };
                db.Authors.Add(book.Author);
            }
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            string view = "ViewBook/" + book.Id;
            return RedirectToAction(view);
        }
        public ActionResult DeleteBook(int id)
        {
            Book b = db.Books.Find(id);
            if (b != null)
            {
                db.Books.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAuthor(int id)
        {
            Author a = db.Authors.Find(id);
            if (a != null)
            {
                db.Authors.Remove(a);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult ViewBook(int? id)
        {
            if (id == null) return HttpNotFound();
            Book book = db.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);
            if (book == null) return HttpNotFound();
            return View(book);
        }
        public ActionResult ViewAuthor(int? id)
        {
            if (id == null) return HttpNotFound();
            Author author = db.Authors.Include(a => a.Books).FirstOrDefault(a => a.AuthorId == id);
            if (author == null) return HttpNotFound();
            return View(author);
        }
    }
}