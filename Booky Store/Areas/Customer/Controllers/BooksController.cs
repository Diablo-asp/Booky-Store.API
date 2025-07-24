using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Customer.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Books()
        {
            return View();
        }
    }
}
