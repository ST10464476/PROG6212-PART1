
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentsController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/enrolments/user/1
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetUserEnrolments(int userId)
        {
            var loggedInUser = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUser, out int participantID) ||
                participantID != userId)
            {
                return Forbid();
            }

            var enrolments = await _context.Enrolments
                .Where(e => e.ParticipantID == participantID)
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/enrolments/event/1
        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventEnrolments(int eventId)
        {
            var loggedInUser = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            var eventItem = await _context.Events.FindAsync(eventId);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            if (!int.TryParse(loggedInUser, out int organiserID) ||
                eventItem.OrganiserID != organiserID)
            {
                return Forbid();
            }

            var enrolments = await _context.Enrolments
                .Where(e => e.EventID == eventId)
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/enrolments/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
            {
                return NotFound();
            }

            var loggedInUser = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            var role = User.FindFirst(
                ClaimTypes.Role)?.Value;

            if (role == "Participant")
            {
                if (!int.TryParse(loggedInUser, out int participantID) ||
                    enrolment.ParticipantID != participantID)
                {
                    return Forbid();
                }
            }
            else if (role == "Organiser")
            {
                var eventItem = await _context.Events.FindAsync(
                    enrolment.EventID);

                if (!int.TryParse(loggedInUser, out int organiserID) ||
                    eventItem == null ||
                    eventItem.OrganiserID != organiserID)
                {
                    return Forbid();
                }
            }
            else
            {
                return Forbid();
            }

            return Ok(enrolment);
        }

        // POST: api/enrolments
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CreateEnrolment(
            [FromBody] Enrolment enrolment)
        {
            var loggedInUser = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(loggedInUser, out int participantID))
            {
                return Unauthorized();
            }

            var eventItem = await _context.Events.FindAsync(
                enrolment.EventID);

            if (eventItem == null)
            {
                return BadRequest("The selected event does not exist.");
            }

            var category = await _context.Categories.FindAsync(
                enrolment.CategoryID);

            if (category == null ||
                category.EventID != enrolment.EventID)
            {
                return BadRequest(
                    "The selected category does not belong to this event.");
            }

            var alreadyEnrolled = await _context.Enrolments.AnyAsync(
                e => e.EventID == enrolment.EventID &&
                     e.ParticipantID == participantID);

            if (alreadyEnrolled)
            {
                return BadRequest(
                    "You are already enrolled in this event.");
            }

            var lastRaceNumber = await _context.Enrolments
                .Where(e => e.EventID == enrolment.EventID)
                .Select(e => (int?)e.RaceNumber)
                .MaxAsync();

            enrolment.ParticipantID = participantID;
            enrolment.EnrolmentDate = DateTime.Now;
            enrolment.RaceNumber = (lastRaceNumber ?? 0) + 1;
            enrolment.PaymentStatus = "Pending";

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = enrolment.EnrolmentID },
                enrolment);
        }
    }
}
