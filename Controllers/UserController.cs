using Microsoft.AspNetCore.Mvc;
using sof1_IIPA26.Dtos;
using sof1_IIPA26.Services;

namespace sof1_IIPA26.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: UserController/Dashboard
        public async Task<ActionResult> Index()
        {
            var usuarios = await _userService.ObtenerUsuarios();
            return View(usuarios);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            UserDto userDto = new();
            return View(userDto);
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto userDto)
        {
            try
            {
                // Validar que el modelo sea válido
                if (!ModelState.IsValid)
                {
                    // Mostrar errores de validación
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewData["Mensaje"] = string.Join("; ", errors.Select(e => e.ErrorMessage));
                    return View(userDto);
                }

                userDto = await _userService.CreateUser(userDto);
                if (userDto.Response == 1)
                {
                    TempData["Mensaje"] = userDto.Message;
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ViewData["Mensaje"] = userDto.Message;
                    return View(userDto);
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al crear el usuario: " + ex.Message;
                return View(userDto);
            }
        }

        // GET: UserController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: UserController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                // Validar que el modelo sea válido
                if (!ModelState.IsValid)
                {
                    // Mostrar errores de validación
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewData["Mensaje"] = string.Join("; ", errors.Select(e => e.ErrorMessage));
                    return View(loginDto);
                }

                // Validar que no sea nulo
                if (loginDto == null)
                {
                    ViewData["Mensaje"] = "Error: Los datos no se recibieron correctamente";
                    return View();
                }

                loginDto = await _userService.InicioSesion(loginDto);
                if (loginDto.Respuesta == 1)
                {
                    // Guardar en sesión
                    HttpContext.Session.SetString("Usuario", loginDto.NombreUsuario);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Mensaje"] = loginDto.Mensaje;
                    return View(loginDto);
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error en inicio de sesión: " + ex.Message;
                return View(loginDto ?? new LoginDto());
            }
        }

        // GET: UserController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
