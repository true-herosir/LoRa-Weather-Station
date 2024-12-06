using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Hours_AVGController : ControllerBase
{
    private readonly Hours_AVGContext _context;

    public Hours_AVGController(Hours_AVGContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hours_AVG>>> GetHoursAVG(
        [FromQuery] string? id,
        [FromQuery] string? location,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] byte? startHour,
        [FromQuery] byte? endHour,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Start building the query dynamically
        var query = _context.Hours_AVGs.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrEmpty(id))
        {
            query = query.Where(h => h.Node_ID == id);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(h => h.Location == location);
        }

        // Filter for a specific day
        if (startDate.HasValue && !endDate.HasValue)
        {
            query = query.Where(h => h.the_day == startDate.Value);
        }

        // Filter for a date range
        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(h => h.the_day >= startDate.Value && h.the_day <= endDate.Value);
        }

        if (startHour.HasValue && !endDate.HasValue)
        {
            query = query.Where(h => h.the_hour == startHour.Value);
        }

        if (startHour.HasValue &&  endHour.HasValue)
        {
            query = query.Where(h => h.the_hour >= startHour.Value && h.the_hour <= endHour.Value);
        }

        // Calculate total items and pages
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Return data with pagination metadata
        return Ok(new
        {
            totalItems,
            totalPages,
            currentPage = page,
            pageSize,
            data
        });
    }
}

