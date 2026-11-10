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
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        // ORGANISER: VIEW RESULTS FOR AN EVENT
        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            var eventItem = await _context.Events.FindAsync(eventId);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (!int.TryParse(userId, out int organiserId) ||
                eventItem.OrganiserID != organiserId)
                return Forbid();

            var results = await _context.Results
                .Join(_context.Enrolments,
                    r => r.EnrolmentID,
                    e => e.EnrolmentID,
                    (r, e) => new { r, e })
                .Where(x => x.e.EventID == eventId)
                .Select(x => x.r)
                .ToListAsync();

            return Ok(results);
        }

        // PARTICIPANT: VIEW OWN RESULTS
        [HttpGet("my-results")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int participantId))
                return Unauthorized();

            var results = await _context.Results
                .Join(_context.Enrolments,
                    r => r.EnrolmentID,
                    e => e.EnrolmentID,
                    (r, e) => new { r, e })
                .Where(x => x.e.ParticipantID == participantId)
                .Select(x => x.r)
                .ToListAsync();

            return Ok(results);
        }

        // ORGANISER: ADD A RESULT
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> AddResult(Result result)
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int organiserId))
                return Unauthorized();

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e =>
                    e.EnrolmentID == result.EnrolmentID);

            if (enrolment == null)
                return BadRequest("Enrolment not found.");

            var eventItem = await _context.Events
                .FindAsync(enrolment.EventID);

            if (eventItem == null ||
                eventItem.OrganiserID != organiserId)
                return Forbid();

            var alreadyHasResult = await _context.Results
                .AnyAsync(r => r.EnrolmentID == result.EnrolmentID);

            if (alreadyHasResult)
                return BadRequest("This enrolment already has a result.");

            if (result.Position <= 0)
                return BadRequest("Position must be greater than zero.");

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        // ORGANISER: UPDATE A RESULT
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateResult(
            int id, Result updatedResult)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound("Result not found.");

            var enrolment = await _context.Enrolments
                .FindAsync(result.EnrolmentID);

            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (enrolment == null ||
                !int.TryParse(userId, out int organiserId))
                return Forbid();

            var eventItem = await _context.Events
                .FindAsync(enrolment.EventID);

            if (eventItem == null ||
                eventItem.OrganiserID != organiserId)
                return Forbid();

            result.FinishTime = updatedResult.FinishTime;
            result.Position = updatedResult.Position;
            result.AveragePace = updatedResult.AveragePace;
            result.ResultStatus = updatedResult.ResultStatus;

            await _context.SaveChangesAsync();

            return Ok(result);
        }
    }
}