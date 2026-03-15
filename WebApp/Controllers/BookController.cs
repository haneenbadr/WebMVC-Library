using EntityFramework2.Context;
using EntityFramework2.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class BookController : Controller
{
    private LibraryDB context = new LibraryDB();

    public IActionResult Index()
    {
        if (TempData.ContainsKey("UserDate"))
        {
            ViewBag.Date = TempData["UserDate"];
            TempData.Keep("UserDate");
        }
         
        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name");

        var allBooks = context.books.ToList();
        return View(allBooks);
    }

    public IActionResult Details(int id)
    {
        var book = context.books.Find(id);
        if (book == null) return NotFound();

        if (TempData.ContainsKey("UserDate"))
        {
            ViewBag.SharedDate = TempData["UserDate"];
            TempData.Keep("UserDate");
        }
        return View(book);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Add(Book book)
    {
        context.books.Add(book);
        context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = context.books.Find(id);
        if (book == null) return NotFound();

 
        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name", book.CategoryID);

        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(int id, Book newbook)
    {
        var oldBook = context.books.Find(id);

        if (oldBook != null)
        {
            oldBook.Title = newbook.Title;
            oldBook.ISBN = newbook.ISBN;
            oldBook.Price = newbook.Price;
            oldBook.PublishYear = newbook.PublishYear;
            oldBook.CategoryID = newbook.CategoryID;  

            context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var book = context.books.Find(id);
        if (book != null)
        {
            context.books.Remove(book);
            context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}