using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SensorsController : ControllerBase
{
    private readonly SensorContext _context;

    public SensorsController(SensorContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor_location>>> GetSensorLocations(
        [FromQuery] string? nodeId,
        [FromQuery] string? location,
        //[FromQuery] double? minBatteryStatus,
        //[FromQuery] double? maxBatteryStatus,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Build the query dynamically
        var query = _context.Sensor_locations.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrWhiteSpace(nodeId))
            query = query.Where(s => s.Node_ID == nodeId);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(s => s.Location == location);

        //if (minBatteryStatus.HasValue && !maxBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status >= minBatteryStatus.Value);

        //if (maxBatteryStatus.HasValue && !minBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status <= maxBatteryStatus.Value);

        //if (minBatteryStatus.HasValue && maxBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status >= minBatteryStatus.Value && s.Battery_status <= maxBatteryStatus.Value);

        // Calculate total items and pages
        var totalItems = await query.CountAsync();
        if (totalItems == 0)
            return NotFound("No records found matching the given criteria.");

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page > totalPages)
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");

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

