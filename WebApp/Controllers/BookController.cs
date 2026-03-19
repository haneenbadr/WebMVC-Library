using EntityFramework2.Context;
using EntityFramework2.Entities;
using WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;  

public class BookController : Controller
{
    private readonly LibraryDB context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookController(IWebHostEnvironment webHostEnvironment)
    {
        context = new LibraryDB();
        _webHostEnvironment = webHostEnvironment;
    }

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
        
        var book = context.books.Include(b => b.BookFiles).FirstOrDefault(b => b.BookID == id);
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
    public async Task<IActionResult> Add(CreateBookViewModel model)
    {
        if (ModelState.IsValid)
        {
            var book = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                Price = model.Price,
                PublishYear = model.PublishYear,
                AuthorID = model.AuthorID,
                CategoryID = model.CategoryID,
                BookFiles = new List<BookAttachment>()
            };

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "attachments");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.Attachments)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    book.BookFiles.Add(new BookAttachment { FileName = fileName });
                }
            }

            context.books.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name");
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        
        var book = context.books.Include(b => b.BookFiles).FirstOrDefault(b => b.BookID == id);
        if (book == null) return NotFound();

        
        var viewModel = new EditBookViewModel
        {
            BookID = book.BookID,
            Title = book.Title,
            ISBN = book.ISBN,
            Price = book.Price,
            PublishYear = book.PublishYear,
            AuthorID = book.AuthorID,
            CategoryID = book.CategoryID,
            ExistingAttachments = book.BookFiles?.ToList() ?? new List<BookAttachment>()
        };

        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name", book.CategoryID);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditBookViewModel model)
    {
        if (id != model.BookID) return NotFound();

        if (ModelState.IsValid)
        {
     
            var book = context.books.Include(b => b.BookFiles).FirstOrDefault(b => b.BookID == id);

            if (book != null)
            {
                book.Title = model.Title;
                book.ISBN = model.ISBN;
                book.Price = model.Price;
                book.PublishYear = model.PublishYear;
                book.CategoryID = model.CategoryID;

                
                if (model.NewAttachments != null && model.NewAttachments.Count > 0)
                {
            
                    if (book.BookFiles != null)
                    {
                        foreach (var oldFile in book.BookFiles)
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "attachments", oldFile.FileName);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                       
                        book.BookFiles.Clear();
                    }
                    else
                    {
                        book.BookFiles = new List<BookAttachment>();
                    }

                  
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "attachments");
                    foreach (var file in model.NewAttachments)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        book.BookFiles.Add(new BookAttachment { FileName = fileName });
                    }
                }

                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        var categories = context.categories.ToList();
        ViewBag.CategoryList = new SelectList(categories, "CatID", "Name", model.CategoryID);
        return View(model);
    }

    public IActionResult Delete(int id)
    {
        var book = context.books.Include(b => b.BookFiles).FirstOrDefault(b => b.BookID == id);
        if (book != null)
        {
            
            if (book.BookFiles != null)
            {
                foreach (var file in book.BookFiles)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "attachments", file.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            context.books.Remove(book);
            context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}

