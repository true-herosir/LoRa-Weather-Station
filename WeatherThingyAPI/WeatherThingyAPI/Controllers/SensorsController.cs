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

    //// GET: api/TodoItems
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Node>>> GetAllNodes()
    //{
    //    return await _context.Node.ToListAsync();
    //}
    // GET: api/Nodes?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor_location>>> GetGateways([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Gateway_location.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Gateway_location
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

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Sensor_location>> GetGatewayItem(string id)
    {
        var todoItem = await _context.Gateway_location.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return todoItem;
    }

}

