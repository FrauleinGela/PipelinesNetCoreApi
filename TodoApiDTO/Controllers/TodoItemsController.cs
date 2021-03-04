using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApiDTO.Models;

namespace TodoApiDTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        #region snippet
        [HttpGet]
        public IEnumerable<TodoItemDTO> GetTodoItems()
        {
            var todoItems = new List<TodoItem>()
            {
                new TodoItem { Name = "Finish task 1", Id = 123, IsComplete = false },
                new TodoItem { Name = "Finish task 2", Id = 456, IsComplete = false },
                new TodoItem { Name = "Finish task 3", Id = 678, IsComplete = true },
                new TodoItem { Name = "Finish task 4", Id = 8910, IsComplete = false },
                new TodoItem { Name = "Finish task 5", Id = 8911, IsComplete = false },
                new TodoItem { Name = "Finish task 6", Id = 8912, IsComplete = false },
                new TodoItem { Name = "Finish task 7", Id = 8913, IsComplete = false },
                new TodoItem { Name = "Finish task 8", Id = 8914, IsComplete = false }
            };
            return todoItems
                .Select(x => ItemToDTO(x));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
             _context.TodoItems.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };       
    }
    #endregion
}

/*  // This method is just for testing populating the secret field
        // POST: api/TodoItems/test
        [HttpPost("test")]
        public async Task<ActionResult<TodoItem>> PostTestTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // This method is just for testing
        // GET: api/TodoItems/test
        [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTestTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }
*/
