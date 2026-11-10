using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Dashboard()
        {
            var userID = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userID) || role != "Organiser")
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}