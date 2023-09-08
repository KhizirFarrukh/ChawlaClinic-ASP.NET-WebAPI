using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic_API.Controller
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
