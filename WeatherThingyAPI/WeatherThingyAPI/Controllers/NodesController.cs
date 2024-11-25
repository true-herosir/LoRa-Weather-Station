using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodesController : ControllerBase
{
    private readonly NodeContext _context;

    public NodesController(NodeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
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

    [HttpGet("pressure")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesPressure([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Pressure })
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

    [HttpGet("illumination")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesIllumination([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Illumination })
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

    [HttpGet("humidity")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesHumidity([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Humidity })
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


    [HttpGet("temperature-indoor")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesTempIndoor([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_indor })
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

    [HttpGet("temperature-outdoor")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesTempOutdoor([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_outdor })
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

    [HttpGet("temperature")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesTemp([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_indor, n.Temperature_outdor })
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

    // GET: api/Node/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
                                   .Where(n => n.Node_ID == id)
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

    [HttpGet("{id}/pressure")]
    public async Task<ActionResult<IEnumerable<Node>>> GetPressureByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Pressure })
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

    [HttpGet("{id}/illumination")]
    public async Task<ActionResult<IEnumerable<Node>>> GetIlluminationByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Illumination })
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

    [HttpGet("{id}/humidity")]

    public async Task<ActionResult<IEnumerable<Node>>> GetHumidityByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Humidity })
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


    [HttpGet("{id}/temperature-indoor")]
    public async Task<ActionResult<IEnumerable<Node>>> GetTempIndoorByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_indor })
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

    [HttpGet("{id}/temperature-outdoor")]
    public async Task<ActionResult<IEnumerable<Node>>> GetTempOutdoorByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_outdor })
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

    [HttpGet("{id}/temperature")]
    public async Task<ActionResult<IEnumerable<Node>>> GetTempByID(string id = "lht-gronau", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Node_ID == id).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
            .Where(n => n.Node_ID == id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new { n.Node_ID, n.Time, n.Location, n.Temperature_indor, n.Temperature_outdor })
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
    [HttpGet("{id}/{time}")]
    public async Task<ActionResult<Node>> GetNodeItems(string id, DateTime time)
    {
        var todoItem = await _context.Nodes.FindAsync(id, time);

        if (todoItem == null)
        {
            return NotFound();
        }

        return todoItem;
    }

    [HttpGet("location/{location}")]
    public async Task<ActionResult<IEnumerable<Node>>> GetNodesByLocation(string location = "Enschede", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Calculate the total number of items and pages
        var totalItems = await _context.Nodes.Where(n => n.Location == location).CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Ensure the requested page is within range
        if (page > totalPages && totalItems > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");
        }

        // Fetch the paginated data
        var nodes = await _context.Nodes
                                   .Where(n => n.Location == location)
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

