using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData["UserDate"] = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

            return View();
        }
    }
}