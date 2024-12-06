using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Max_MinController : ControllerBase
{
    private readonly Max_MinContext _context;

    public Max_MinController(Max_MinContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Max_Min>>> GetMaxMins(
        [FromQuery] string? id,
        [FromQuery] DateOnly? start_date,
        [FromQuery] DateOnly? end_date,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Start building the query dynamically
        var query = _context.Max_Mins.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrEmpty(id))
        {
            query = query.Where(n => n.Node_ID == id);
        }
        
        if (start_date.HasValue && !end_date.HasValue)
        {
            query = query.Where(n => n.the_day == start_date.Value);
        }

        if (start_date.HasValue && end_date.HasValue)
        {
            query = query.Where(n => n.the_day >= start_date.Value && n.the_day <= end_date.Value);
        }


        // Calculate the total number of items and pages
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Return data along with pagination metadata
        return Ok(new
        {
            totalItems,
            totalPages,
            currentPage = page,
            pageSize,
            data = nodes
        });
    }

}

