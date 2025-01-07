using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodesController : ControllerBase
{
    private readonly NodeContext _context; 
    private readonly Node_locationContext _scontext;

    public NodesController(NodeContext context, Node_locationContext sensorContext)
    {
        _context = context;
        _scontext = sensorContext;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetNodes(
    [FromQuery] string? id,
    [FromQuery] string? gateway_location,
    [FromQuery] DateTime? start_time,
    [FromQuery] DateTime? end_time,
    [FromQuery] int page = 1,
    [FromQuery] int page_size = 10)
    {
        // Validate page and page_size
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Start building the query
        IQueryable<Node> query = _context.Nodes;

        // Apply filters if present
        if (!string.IsNullOrWhiteSpace(id))
        {
            query = query.Where(n => n.Node_ID == id);
        }

        if (!string.IsNullOrWhiteSpace(gateway_location))
        {
            query = query.Where(n => n.Gateway_Location == gateway_location);
        }

        if (start_time.HasValue && !end_time.HasValue)
        {
            // Include records for the entire day of start_time
            DateTime startOfDay = start_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            query = query.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (end_time.HasValue && !start_time.HasValue)
        {
            // Include records for the entire day of end_time
            DateTime startOfDay = end_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            query = query.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (start_time.HasValue && end_time.HasValue)
        {
            // Use the exact provided start_time and end_time
            query = query.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value);
        }

        // Get total items count
        int total_items = await query.CountAsync();
        if (total_items == 0)
        {
            return NotFound("No records found matching the given criteria.");
        }

        // Calculate total pages
        int total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        if (page > total_pages)
        {
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");
        }

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .OrderBy(n => n.Time)
            .Select(n => new
            {
                node_id = n.Node_ID,
                time = n.Time,
                pressure = n.Pressure,
                illumination = n.Illumination,
                humidity = n.Humidity,
                gateway_location = n.Gateway_Location,
                temperature_indoor = n.Temperature_indoor,
                temperature_outdoor = n.Temperature_outdoor
            })
            .ToListAsync();

        // Return paginated response with metadata
        return Ok(new
        {
            total_items,
            total_pages,
            current_page = page,
            page_size,
            data
        });
    }


    [HttpGet("node_location")]
    public async Task<ActionResult<IEnumerable<object>>> GetNodesWithSensorLocations(
        [FromQuery] string? id,
        [FromQuery] string? gateway_location,
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
        var nodesQuery = _context.Nodes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(id))
            nodesQuery = nodesQuery.Where(n => n.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(gateway_location))
            nodesQuery = nodesQuery.Where(n => n.Gateway_Location == gateway_location);

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
                             gateway_location = node.Gateway_Location,
                             temperature_indoor = node.Temperature_indoor,
                             temperature_outdoor = node.Temperature_outdoor,
                             location = sensor.Location
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

