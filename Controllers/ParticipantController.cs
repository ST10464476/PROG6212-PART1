using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.Models;
using System.Net.Http;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // PARTICIPANT DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var picture = await _apiService
                    .GetAsync<ProfilePictureResponse>(
                        "api/profile-pictures/my-picture");

                ViewBag.ProfilePictureUrl = picture?.ProfilePictureUrl;
            }
            catch (HttpRequestException)
            {
                ViewBag.ProfilePictureUrl = null;
            }

            return View();
        }

        // UPLOAD PROFILE PICTURE
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile image)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
                return RedirectToAction("Login", "Auth");

            if (image == null || image.Length == 0)
            {
                TempData["ProfileMessage"] = "Please select an image.";
                return RedirectToAction("Dashboard");
            }

            try
            {
                var response = await _apiService.UploadFileAsync(
                    "api/profile-pictures/upload", image);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ProfileMessage"] =
                        "Profile picture uploaded successfully!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["ProfileMessage"] = "Upload failed: " + error;
                }
            }
            catch (Exception)
            {
                TempData["ProfileMessage"] =
                    "An error occurred while uploading your picture.";
            }

            return RedirectToAction("Dashboard");
        }

        // VIEW PARTICIPANT PROFILE
        public async Task<IActionResult> Profile()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
                return RedirectToAction("Login", "Auth");

            try
            {
                var profile = await _apiService
                    .GetAsync<ParticipantProfileViewModel>(
                        $"api/users/{userID}");

                if (profile == null)
                {
                    TempData["ProfileMessage"] = "Profile could not be loaded.";
                    return RedirectToAction("Dashboard");
                }

                return View(profile);
            }
            catch (HttpRequestException)
            {
                TempData["ProfileMessage"] =
                    "Unable to load your profile. Please try again.";

                return RedirectToAction("Dashboard");
            }
        }

        // UPDATE PARTICIPANT PROFILE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
            ParticipantProfileViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Participant")
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            try
            {
                var response = await _apiService.PutAsync(
                    $"api/users/{userID}",
                    new
                    {
                        model.FirstName,
                        model.LastName,
                        model.PhoneNumber
                    });

                if (response.IsSuccessStatusCode)
                {
                    TempData["ProfileMessage"] =
                        "Your profile was updated successfully!";

                    return RedirectToAction("Profile");
                }

                var error = await response.Content.ReadAsStringAsync();

                ViewBag.ErrorMessage = "Update failed: " + error;
                return View("Profile", model);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage =
                    "An error occurred while updating your profile.";

                return View("Profile", model);
            }
        }

        public class ProfilePictureResponse
        {
            public string? ProfilePictureUrl { get; set; }
        }
    }
}