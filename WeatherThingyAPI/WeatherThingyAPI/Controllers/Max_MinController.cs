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
        [FromQuery] string? location,
        [FromQuery] DateOnly? start_date,
        [FromQuery] DateOnly? end_date,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
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

        if (end_date.HasValue && !start_date.HasValue)
        {
            query = query.Where(n => n.the_day == end_date.Value);
        }

        if (start_date.HasValue && end_date.HasValue)
        {
            query = query.Where(n => n.the_day >= start_date.Value && n.the_day <= end_date.Value);
        }


        // Calculate the total number of items and pages
        var total_items = await query.CountAsync();
        var total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        // Ensure the requested page is within range
        if (page > total_pages && total_items > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");
        }

        // Fetch the paginated data
        var nodes = await query
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .Select(n => new
            {
                node_id = n.Node_ID,
                location = n.Location,
                n.the_day,
                max_pressure = n.max_Pressure,
                min_pressure = n.min_Pressure,
                max_illumination = n.max_Illumination,
                min_illumination = n.min_Illumination,
                max_humidity = n.max_Humidity,
                min_humidity = n.min_Humidity,
                max_temperature_indoor = n.max_Temperature_indoor,
                min_temperature_indoor = n.min_Temperature_indoor,
                max_emperature_outdoor = n.max_Temperature_outdoor,
                min_temperature_outdoor = n.min_Temperature_outdoor

            })
            .ToListAsync();

        // Return data along with pagination metadata
        return Ok(new
        {
            total_items,
            total_pages,
            current_page = page,
            page_size,
            data = nodes
        });
    }

}

