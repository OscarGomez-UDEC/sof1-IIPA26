using Microsoft.AspNetCore.Mvc;
using sof1_IIPA26.Dtos;

namespace sof1_IIPA26.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            UserDto userDto = new();
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserDto userDto)
        {
            return View();
        }
    }
}
