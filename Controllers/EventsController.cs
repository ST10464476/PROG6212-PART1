
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Response returned by the API banner endpoint
        private class BannerResponse
        {
            public string? BannerImageUrl { get; set; }
        }

        // VIEW ALL EVENTS
        public async Task<IActionResult> Index()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
                return RedirectToAction("Login", "Auth");

            var events = await _apiService.GetAsync<List<EventViewModel>>(
                "api/events");

            events ??= new List<EventViewModel>();

            // Retrieve secure Azure image links for uploaded banners
            foreach (var eventItem in events)
            {
                if (!string.IsNullOrEmpty(eventItem.BannerImageUrl))
                {
                    try
                    {
                        var banner = await _apiService.GetAsync<BannerResponse>(
                            $"api/events/{eventItem.EventID}/banner");

                        eventItem.BannerImageUrl = banner?.BannerImageUrl;
                    }
                    catch
                    {
                        eventItem.BannerImageUrl = null;
                    }
                }
            }

            return View(events);
        }

        // CREATE EVENT - Display form
        [HttpGet]
        public IActionResult Create()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // CREATE EVENT - Save event
        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var eventData = new
            {
                EventName = model.EventName,
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location,
                Distance = model.Distance,
                EntryFee = model.EntryFee,
                Status = "Upcoming"
            };

            var response = await _apiService.PostAsync(
                "api/events", eventData);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not create the event. Please try again.");

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // EDIT EVENT - Display form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            var eventItem = await _apiService.GetAsync<EventViewModel>(
                $"api/events/{id}");

            if (eventItem == null)
                return NotFound();

            if (eventItem.OrganiserID.ToString() != userID)
                return Forbid();

            return View(eventItem);
        }

        // EDIT EVENT - Save changes
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            if (id != model.EventID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var existingEvent = await _apiService.GetAsync<EventViewModel>(
                $"api/events/{id}");

            if (existingEvent == null)
                return NotFound();

            if (existingEvent.OrganiserID.ToString() != userID)
                return Forbid();

            var eventData = new
            {
                EventID = model.EventID,
                OrganiserID = existingEvent.OrganiserID,
                EventName = model.EventName,
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location,
                Distance = model.Distance,
                EntryFee = model.EntryFee,
                Status = model.Status
            };

            var response = await _apiService.PutAsync(
                $"api/events/{id}", eventData);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not update the event. Please try again.");

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // DELETE EVENT
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            var existingEvent = await _apiService.GetAsync<EventViewModel>(
                $"api/events/{id}");

            if (existingEvent == null)
                return NotFound();

            if (existingEvent.OrganiserID.ToString() != userID)
                return Forbid();

            var response = await _apiService.DeleteAsync(
                $"api/events/{id}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Could not delete the event.";
            else
                TempData["Success"] = "Event deleted successfully.";

            return RedirectToAction("Index");
        }

        // VIEW EVENT DETAILS AND CATEGORIES
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
                return RedirectToAction("Login", "Auth");

            var eventItem = await _apiService.GetAsync<EventViewModel>(
                $"api/events/{id}");

            if (eventItem == null)
                return NotFound();

            var categories = await _apiService.GetAsync<List<CategoryViewModel>>(
                $"api/categories/event/{id}");

            var model = new EventDetailsViewModel
            {
                Event = eventItem,
                Categories = categories ?? new List<CategoryViewModel>()
            };

            return View(model);
        }

        // UPLOAD EVENT BANNER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadBanner(int id, IFormFile image)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
                return RedirectToAction("Login", "Auth");

            var existingEvent = await _apiService.GetAsync<EventViewModel>(
                $"api/events/{id}");

            if (existingEvent == null)
                return NotFound();

            if (existingEvent.OrganiserID.ToString() != userID)
                return Forbid();

            if (image == null || image.Length == 0)
            {
                TempData["BannerMessage"] = "Please select an image.";
                return RedirectToAction("Edit", new { id });
            }

            try
            {
                var response = await _apiService.UploadFileAsync(
                    $"api/events/{id}/banner", image);

                if (response.IsSuccessStatusCode)
                {
                    TempData["BannerMessage"] =
                        "Event banner uploaded successfully!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    TempData["BannerMessage"] =
                        "Banner upload failed: " + error;
                }
            }
            catch (Exception)
            {
                TempData["BannerMessage"] =
                    "An error occurred while uploading the banner.";
            }

            return RedirectToAction("Edit", new { id });
        }
    }
}
