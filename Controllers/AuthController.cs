using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.Text.Json;

namespace RaceDay.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.PostAsync(
                "api/auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed. Please check your details.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.PostAsync(
                "api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                var apiCookie = cookieHeaders.FirstOrDefault()?.Split(';')[0];

                if (!string.IsNullOrEmpty(apiCookie))
                {
                    HttpContext.Session.SetString("ApiCookie", apiCookie);
                }
            }
            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            var root = document.RootElement;

            HttpContext.Session.SetString(
                "UserID",
                root.GetProperty("userID").ToString());

            HttpContext.Session.SetString(
                "Email",
                root.GetProperty("email").GetString() ?? "");

            HttpContext.Session.SetString(
                "Role",
                root.GetProperty("role").GetString() ?? "");

            if (HttpContext.Session.GetString("Role") == "Organiser")
                return RedirectToAction("Dashboard", "Organiser");

            return RedirectToAction("Dashboard", "Participant");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}