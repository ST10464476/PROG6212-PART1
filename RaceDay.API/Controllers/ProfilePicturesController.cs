using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RaceDay.API.Data;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers
{
    [Route("api/profile-pictures")]
    [ApiController]
    [Authorize(Roles = "Participant")]
    public class ProfilePicturesController : ControllerBase
    {
        private readonly RaceDayContext _context;
        private readonly AzureBlobService _blobService;

        public ProfilePicturesController(
            RaceDayContext context,
            AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Please select an image.");

            if (image.Length > 5 * 1024 * 1024)
                return BadRequest("Image must not exceed 5 MB.");

            var allowedTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!allowedTypes.Contains(image.ContentType.ToLowerInvariant()))
                return BadRequest("Only JPG, PNG and WEBP images are allowed.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int participantId))
                return Unauthorized();

            var participant = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.UserID == participantId && u.Role == "Participant");

            if (participant == null)
                return NotFound("Participant account not found.");

            using var stream = image.OpenReadStream();

            string blobName = await _blobService.UploadImageAsync(
                stream,
                image.FileName,
                image.ContentType);

            participant.ProfilePictureUrl = blobName;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile picture uploaded successfully."
            });
        }

        [HttpGet("my-picture")]
        public async Task<IActionResult> GetMyPicture()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int participantId))
                return Unauthorized();

            var participant = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.UserID == participantId && u.Role == "Participant");

            if (participant == null)
                return NotFound("Participant account not found.");

            if (string.IsNullOrEmpty(participant.ProfilePictureUrl))
                return NotFound("You have not uploaded a profile picture.");

            string? imageUrl = await _blobService.GetImageReadUrlAsync(
                participant.ProfilePictureUrl);

            if (imageUrl == null)
                return NotFound("Profile picture not found in Azure Storage.");

            return Ok(new
            {
                profilePictureUrl = imageUrl
            });
        }
    }
}