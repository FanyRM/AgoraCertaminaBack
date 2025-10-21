using Microsoft.AspNetCore.Mvc;

namespace AgoraCertaminaBack.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
