using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}