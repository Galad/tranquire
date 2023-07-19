using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain;

namespace ToDoList.Controllers;

[Route("api/todoitem")]
[ApiController]
public class ToDoItemController : ControllerBase
{
    private readonly IToDoItemRepository toDoItemRepository;

    public ToDoItemController(IToDoItemRepository toDoItemRepository)
    {
        this.toDoItemRepository = toDoItemRepository;
    }

    [HttpPost]
    public IActionResult Post([FromBody] string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest("The title cannot be null or empty");
        }

        toDoItemRepository.Add(ToDoItem.Create(title));
        return Ok();
    }

    [HttpGet]
    public IEnumerable<ToDoItem> Get()
    {
        return toDoItemRepository.Get();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest();
        }
        toDoItemRepository.Delete(id);
        return Ok();
    }
}
