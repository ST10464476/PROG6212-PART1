
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApiService _apiService;

        public CategoriesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // DISPLAY CATEGORIES
        [HttpGet]
        public async Task<IActionResult> Index(int? eventId)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var events = await _apiService.GetAsync<List<EventViewModel>>(
                "api/events");

            ViewBag.Events = events ?? new List<EventViewModel>();
            ViewBag.SelectedEventID = eventId;

            if (eventId == null)
            {
                return View(new List<CategoryViewModel>());
            }

            var categories = await _apiService.GetAsync<List<CategoryViewModel>>(
                $"api/categories/event/{eventId}");

            return View(categories ?? new List<CategoryViewModel>());
        }

        // CREATE CATEGORY - DISPLAY FORM
        [HttpGet]
        public IActionResult Create(int eventId)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new CategoryViewModel
            {
                EventID = eventId
            });
        }

        // CREATE CATEGORY - SAVE
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryData = new
            {
                EventID = model.EventID,
                CategoryName = model.CategoryName,
                Distance = model.Distance,
                AgeMin = model.AgeMin,
                AgeMax = model.AgeMax,
                EntryFee = model.EntryFee
            };

            var response = await _apiService.PostAsync(
                "api/categories", categoryData);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("",
                    $"API Error ({(int)response.StatusCode}): {errorDetails}");

                return View(model);
            }

            TempData["Success"] = "Category created successfully.";

            return RedirectToAction("Index", new { eventId = model.EventID });
        }

        // EDIT CATEGORY - DISPLAY FORM
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var category = await _apiService.GetAsync<CategoryViewModel>(
                $"api/categories/{id}");

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // EDIT CATEGORY - SAVE
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != model.CategoryID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _apiService.PutAsync(
                $"api/categories/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("",
                    $"API Error ({(int)response.StatusCode}): {errorDetails}");

                return View(model);
            }

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction("Index", new { eventId = model.EventID });
        }

        // DELETE CATEGORY
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int eventId)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await _apiService.DeleteAsync(
                $"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                TempData["Error"] =
                    $"Could not delete category. API Error ({(int)response.StatusCode}): {errorDetails}";
            }
            else
            {
                TempData["Success"] = "Category deleted successfully.";
            }

            return RedirectToAction("Index", new { eventId });
        }
    }
}
