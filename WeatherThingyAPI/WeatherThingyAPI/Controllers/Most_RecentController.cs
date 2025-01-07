using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Most_RecentController : ControllerBase
{
    private readonly Most_RecentContext _context;
    private readonly Node_locationContext _scontext;


    public Most_RecentController(Most_RecentContext context, Node_locationContext sensorContext)
    {
        _context = context;
        _scontext = sensorContext;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetNodesWithSensorLocations(
        [FromQuery] string? id,
        [FromQuery] string? location,
        [FromQuery] DateTime? start_time,
        [FromQuery] DateTime? end_time,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Load nodes from NodeContext
        var nodesQuery = _context.Most_Recents.AsQueryable();

        if (!string.IsNullOrWhiteSpace(id))
            nodesQuery = nodesQuery.Where(n => n.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(location))
            nodesQuery = nodesQuery.Where(n => n.Location == location);

        if (start_time.HasValue && !end_time.HasValue)
        {
            // Include records for the entire day of start_time
            DateTime startOfDay = start_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            nodesQuery = nodesQuery.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (end_time.HasValue && !start_time.HasValue)
        {
            // Include records for the entire day of end_time
            DateTime startOfDay = end_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            nodesQuery = nodesQuery.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (start_time.HasValue && end_time.HasValue)
        {
            // Use the exact provided start_time and end_time
            nodesQuery = nodesQuery.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value);
        }



        var nodes = await nodesQuery.ToListAsync();

        // Load sensor locations from SensorContext
        var sensorsQuery = _scontext.Node_locations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(location))
            sensorsQuery = sensorsQuery.Where(s => s.Location == location);

        var sensors = await sensorsQuery.ToListAsync();

        // Join data in memory
        var joinedData = from node in nodes
                         join sensor in sensors
                         on node.Node_ID equals sensor.Node_ID
                         select new
                         {
                             node_id = node.Node_ID,
                             time = node.Time,
                             pressure = node.Pressure,
                             illumination = node.Illumination,
                             humidity = node.Humidity,
                             location = node.Location,
                             temperature_indoor = node.Temperature_indoor,
                             temperature_outdoor = node.Temperature_outdoor,
                             battery_status = sensor.Battery_status,
                             node.gateway_id,
                             node.lat,
                             node.lng,
                             node.alt
                         };

        var total_items = joinedData.Count();
        var total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        if (page > total_pages)
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");

        var data = joinedData
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .ToList();

        return Ok(new
        {
            total_items,
            total_pages,
            current_page = page,
            page_size,
            data
        });
    }

}

