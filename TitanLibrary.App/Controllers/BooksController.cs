using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TitanLibrary.Business.Abstractions;

namespace TitanLibrary.App.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public ActionResult Index()
    {
        var books = _bookService.GetAll();

        ViewBag.Books = books;

        return View(books);
    }

    public ActionResult Details(int id)
    {
        var book = _bookService.GetById(id);
    
        return View(book);
    }

    public ActionResult Borrow(int id)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

        _bookService.Borrow(userId, id);

        return RedirectToAction("Index", "Books", null);
    }

    public ActionResult Return(int id)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

        _bookService.Return(userId, id);

        return RedirectToAction("Borrowed", "Books", null);
    }

    public ActionResult Borrowed()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

        var books = _bookService.GetAllBorrowed(userId);

        return View(books);
    }

    public ActionResult Search(string title, string author, string isbn)
    {
        var books = _bookService.Search(title, author, isbn);

        ViewBag.BookTitle = title;
        ViewBag.Author = author;
        ViewBag.Isbn = isbn;

        return View("Index", books);
    }
}
