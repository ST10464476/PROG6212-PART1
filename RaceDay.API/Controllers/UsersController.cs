using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public UsersController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var currentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (currentUserId != id.ToString())
                return Forbid();

            var user = await _context.Users
                .Where(u => u.UserID == id)
                .Select(u => new
                {
                    u.UserID,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.PhoneNumber,
                    u.Role,
                    u.ProfilePictureUrl
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id, [FromBody] UpdateUserRequest request)
        {
            var currentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (currentUserId != id.ToString())
                return Forbid();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
                return NotFound("User not found.");

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                return BadRequest("All profile fields are required.");
            }

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.PhoneNumber = request.PhoneNumber.Trim();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }
    }

    public class UpdateUserRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }
}