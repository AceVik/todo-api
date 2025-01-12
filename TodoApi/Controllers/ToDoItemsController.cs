using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly ToDoContext _context;

    public ToDoItemsController(ToDoContext context)
    {
        _context = context;
    }

    // GET: api/ToDoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems(
        [FromQuery] ItemStatusFilter? filter
    )
    {
        var query = _context.ToDoItems.AsQueryable();

        switch (filter)
        {
            case ItemStatusFilter.Todo:
                query = query.Where(x => !x.IsCompleted);
                break;
            case ItemStatusFilter.Completed:
                query = query.Where(x => x.IsCompleted);
                break;
            case ItemStatusFilter.All:
            default:
                break;
        }

        return await query.ToListAsync();
    }

    // GET: api/ToDoItems/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null)
        {
            return NotFound();
        }
        return toDoItem;
    }

    // POST: api/ToDoItems
    [HttpPost]
    public async Task<ActionResult<ToDoItem>> PostToDoItem([FromBody] CreateTodoItemDto dto)
    {
        var toDoItem = new ToDoItem
        {
            Title = dto.Title,
            IsCompleted = dto.IsCompleted ?? false
        };

        _context.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetToDoItem),
            new { id = toDoItem.Id },
            toDoItem
        );
    }

    // PATCH: api/ToDoItems/{id}
    [HttpPatch("{id}")]
    public async Task<ActionResult<ToDoItem>> PatchToDoItem(int id, [FromBody] PatchTodoItemDto dto)
    {
        var existingItem = await _context.ToDoItems.FindAsync(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        if (dto.Title != null)
        {
            existingItem.Title = dto.Title;
        }

        if (dto.IsCompleted.HasValue)
        {
            existingItem.IsCompleted = dto.IsCompleted.Value;
        }

        await _context.SaveChangesAsync();

        return existingItem;
    }

    // DELETE: api/ToDoItems/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(int id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null)
        {
            return NotFound();
        }

        _context.ToDoItems.Remove(toDoItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool ToDoItemExists(int id)
    {
        return _context.ToDoItems.Any(e => e.Id == id);
    }
}
