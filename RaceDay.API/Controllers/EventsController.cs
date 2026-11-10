
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;
using System.Security.Claims;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;
        private readonly AzureBlobService _blobService;

        public EventsController(
            RaceDayContext context,
            AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            return Ok(eventItem);
        }

        // POST: api/events
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent(Event eventItem)
        {
            var userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organiserID))
                return Unauthorized();

            eventItem.OrganiserID = organiserID;

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventID },
                eventItem);
        }

        // PUT: api/events/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(
            int id, Event eventItem)
        {
            var userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organiserID))
                return Unauthorized();

            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
                return NotFound();

            if (existingEvent.OrganiserID != organiserID)
                return Forbid();

            if (id != eventItem.EventID)
                return BadRequest();

            existingEvent.EventName = eventItem.EventName;
            existingEvent.Description = eventItem.Description;
            existingEvent.EventDate = eventItem.EventDate;
            existingEvent.Location = eventItem.Location;
            existingEvent.Distance = eventItem.Distance;
            existingEvent.EntryFee = eventItem.EntryFee;
            existingEvent.Status = eventItem.Status;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organiserID))
                return Unauthorized();

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            if (eventItem.OrganiserID != organiserID)
                return Forbid();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/events/{id}/banner
        [HttpPost("{id}/banner")]
        [Authorize(Roles = "Organiser")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadBanner(
            int id, IFormFile image)
        {
            var userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organiserID))
                return Unauthorized();

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (eventItem.OrganiserID != organiserID)
                return Forbid();

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

            if (!allowedTypes.Contains(
                image.ContentType.ToLowerInvariant()))
            {
                return BadRequest(
                    "Only JPG, PNG and WEBP images are allowed.");
            }

            using var stream = image.OpenReadStream();

            string blobName = await _blobService.UploadImageAsync(
                stream,
                image.FileName,
                image.ContentType);

            eventItem.BannerImageUrl = blobName;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event banner uploaded successfully.",
                bannerImageUrl = blobName
            });
        }

        // GET: api/events/{id}/banner
        [HttpGet("{id}/banner")]
        public async Task<IActionResult> GetEventBanner(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (string.IsNullOrEmpty(eventItem.BannerImageUrl))
                return NotFound(
                    "No banner has been uploaded for this event.");

            string? imageUrl =
                await _blobService.GetImageReadUrlAsync(
                    eventItem.BannerImageUrl);

            if (imageUrl == null)
                return NotFound(
                    "Banner image could not be found in storage.");

            return Ok(new
            {
                bannerImageUrl = imageUrl
            });
        }
    }
}
