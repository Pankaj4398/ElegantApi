using Microsoft.AspNetCore.Mvc;

namespace ShopServer.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
