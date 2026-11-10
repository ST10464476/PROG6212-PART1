using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly ApiService _apiService;

        public EnrolmentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // ORGANISER: VIEW EVENT ENROLMENTS
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
                return View(new List<EnrolmentViewModel>());
            }

            var enrolments = await _apiService.GetAsync<List<EnrolmentViewModel>>(
                $"api/enrolments/event/{eventId}");

            return View(enrolments ?? new List<EnrolmentViewModel>());
        }

        // PARTICIPANT: VIEW OWN ENROLMENTS
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
            {
                return RedirectToAction("Login", "Auth");
            }

            var enrolments = await _apiService.GetAsync<List<EnrolmentViewModel>>(
                $"api/enrolments/user/{userID}");

            return View(enrolments ?? new List<EnrolmentViewModel>());
        }

        // PARTICIPANT: ENROL IN AN EVENT CATEGORY
        [HttpPost]
        public async Task<IActionResult> Create(int eventId, int categoryId)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
            {
                return RedirectToAction("Login", "Auth");
            }

            var enrolmentData = new
            {
                EventID = eventId,
                CategoryID = categoryId
            };

            var response = await _apiService.PostAsync(
                "api/enrolments", enrolmentData);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                TempData["Error"] =
                    $"Enrolment failed ({(int)response.StatusCode}): {errorDetails}";

                return RedirectToAction("Details", "Events",
                    new { id = eventId });
            }

            TempData["Success"] =
                "You have successfully enrolled in the event.";

            return RedirectToAction("MyEnrolments");
        }
    }
}