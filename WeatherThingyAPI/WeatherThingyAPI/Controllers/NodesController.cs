using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodesController : ControllerBase
{
    private readonly NodeContext _context; 
    private readonly SensorContext _scontext;

    //public NodesController(NodeContext context)
    //{
    //    _context = context;
    //}

    public NodesController(NodeContext context, SensorContext sensorContext)
    {
        _context = context;
        _scontext = sensorContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodes(
    [FromQuery] string? id,
    [FromQuery] string? gateway_location,
    [FromQuery] DateTime? start_time,
    [FromQuery] DateTime? end_time,
    [FromQuery] int page = 1,
    [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Build the query dynamically
        var query = _context.Nodes.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrWhiteSpace(id))
            query = query.Where(n => n.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(gateway_location))
            query = query.Where(n => n.Gateway_Location == gateway_location);

        if (start_time.HasValue && !end_time.HasValue)
            query = query.Where(n => n.Time >= start_time.Value && n.Time <= start_time.Value.AddHours(24));

        if (start_time.HasValue && end_time.HasValue)
            query = query.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value.AddHours(24));

        // Calculate total items and pages
        var total_items = await query.CountAsync();
        if (total_items == 0)
            return NotFound("No records found matching the given criteria.");

        var total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        if (page > total_pages)
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .ToListAsync();

        // Return data with pagination metadata
        return Ok(new
        {
            total_items,
            total_pages,
            current_page = page,
            page_size,
            data
        });
    }

    [HttpGet("sensor_location")]
    public async Task<ActionResult<IEnumerable<object>>> GetNodesWithSensorLocations(
        [FromQuery] string? id,
        [FromQuery] string? gateway_location,
        [FromQuery] string? sensor_location,
        [FromQuery] double? minBatteryStatus,
        [FromQuery] double? maxBatteryStatus,
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
        var nodesQuery = _context.Nodes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(id))
            nodesQuery = nodesQuery.Where(n => n.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(gateway_location))
            nodesQuery = nodesQuery.Where(n => n.Gateway_Location == gateway_location);

        if (start_time.HasValue && !end_time.HasValue)
            nodesQuery = nodesQuery.Where(n => n.Time >= start_time.Value);

        if (end_time.HasValue && !start_time.HasValue)
            nodesQuery = nodesQuery.Where(n => n.Time <= end_time.Value);

        if (start_time.HasValue && end_time.HasValue)
            nodesQuery = nodesQuery.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value);

        var nodes = await nodesQuery.ToListAsync();

        // Load sensor locations from SensorContext
        var sensorsQuery = _scontext.Sensor_locations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sensor_location))
            sensorsQuery = sensorsQuery.Where(s => s.Location == sensor_location);

        var sensors = await sensorsQuery.ToListAsync();

        // Join data in memory
        var joinedData = from node in nodes
                         join sensor in sensors
                         on node.Node_ID equals sensor.Node_ID
                         select new
                         {
                             Node_ID = node.Node_ID,
                             Time = node.Time,
                             Pressure = node.Pressure,
                             Illumination = node.Illumination,
                             Humidity = node.Humidity,
                             Gateway_Location = node.Gateway_Location,
                             Temperature_Indoor = node.Temperature_indoor,
                             Temperature_Outdoor = node.Temperature_outdoor,
                             Sensor_Location = sensor.Location
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

