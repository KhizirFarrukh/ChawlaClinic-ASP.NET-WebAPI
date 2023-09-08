using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic_API.Controller
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
