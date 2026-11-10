using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApiService _apiService;

        public ResultsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // ORGANISER: VIEW EVENT RESULTS
        public async Task<IActionResult> Index(int? eventId)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            var events = await _apiService.GetAsync<List<EventViewModel>>(
                "api/events");

            ViewBag.Events = events ?? new List<EventViewModel>();
            ViewBag.SelectedEventID = eventId;

            if (eventId == null)
                return View(new List<ResultViewModel>());

            var results = await _apiService.GetAsync<List<ResultViewModel>>(
                $"api/results/event/{eventId}");

            return View(results ?? new List<ResultViewModel>());
        }

        // PARTICIPANT: VIEW OWN RESULTS
        public async Task<IActionResult> MyResults()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
                return RedirectToAction("Login", "Auth");

            var results = await _apiService.GetAsync<List<ResultViewModel>>(
                "api/results/my-results");

            return View(results ?? new List<ResultViewModel>());
        }

        // ORGANISER: ADD RESULT
        [HttpGet]
        public IActionResult Create(int enrolmentId, int eventId)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Organiser")
                return RedirectToAction("Login", "Auth");

            var model = new ResultViewModel
            {
                EnrolmentID = enrolmentId
            };

            ViewBag.EventID = eventId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ResultViewModel model, int eventId)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Organiser")
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                ViewBag.EventID = eventId;
                return View(model);
            }

            var response = await _apiService.PostAsync(
                "api/results", model);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);
                ViewBag.EventID = eventId;
                return View(model);
            }

            TempData["Success"] = "Result added successfully.";

            return RedirectToAction("Index",
                new { eventId = eventId });
        }
    }
}