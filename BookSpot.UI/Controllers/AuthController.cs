using BookSpot.UI.Services.Interfaces;
using BookSpot.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookSpot.UI.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public IActionResult Login()
    {
        if (_authService.IsAuthenticated())
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        var (success, message) = await _authService.LoginAsync(loginVM);

        if (success)
        {
            if (_authService.GetUserPerfil() == "Administrador")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, message);
        return View(loginVM);
    }


    public IActionResult Registro()
    {
        if (_authService.IsAuthenticated())
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroVM registroDto)
    {
        if (!ModelState.IsValid)
            return View(registroDto);

        var (success, message) = await _authService.RegisterAsync(registroDto);

        if (success)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, message);
        return View(registroDto);
    }

    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index", "Home");
    }
}
