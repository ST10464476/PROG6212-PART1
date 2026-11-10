
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public CategoriesController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/categories/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCategoriesByEvent(int eventId)
        {
            var categories = await _context.Categories
                .Where(c => c.EventID == eventId)
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categories/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            var eventItem = await _context.Events.FindAsync(category.EventID);

            if (eventItem == null)
            {
                return BadRequest("The selected event does not exist.");
            }

            var userID = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organiserID) ||
                eventItem.OrganiserID != organiserID)
            {
                return Forbid();
            }

            if (category.AgeMin < 0 || category.AgeMax < category.AgeMin ||
                category.Distance <= 0 || category.EntryFee < 0)
            {
                return BadRequest("Please provide valid category details.");
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryID },
                category);
        }

        // PUT: api/categories/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var userID = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var eventItem = await _context.Events.FindAsync(
                existingCategory.EventID);

            if (!int.TryParse(userID, out int organiserID) ||
                eventItem == null || eventItem.OrganiserID != organiserID)
            {
                return Forbid();
            }

            if (category.AgeMin < 0 || category.AgeMax < category.AgeMin ||
                category.Distance <= 0 || category.EntryFee < 0)
            {
                return BadRequest("Please provide valid category details.");
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Distance = category.Distance;
            existingCategory.AgeMin = category.AgeMin;
            existingCategory.AgeMax = category.AgeMax;
            existingCategory.EntryFee = category.EntryFee;

            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }

        // DELETE: api/categories/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var userID = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var eventItem = await _context.Events.FindAsync(category.EventID);

            if (!int.TryParse(userID, out int organiserID) ||
                eventItem == null || eventItem.OrganiserID != organiserID)
            {
                return Forbid();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
