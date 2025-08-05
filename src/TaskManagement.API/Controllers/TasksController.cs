using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Commands.Tasks;
using TaskManagement.Application.Queries.Tasks;
using TaskManagement.Application.ReadModels.Tasks;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTaskDetailsQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IReadOnlyList<TaskSummary>>> GetUserTasks(Guid userId)
        {
            var result = await _mediator.Send(new GetUserTasksQuery(userId));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskCommand command)
        {
            if (id != command.TaskId) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> Assign(Guid id, AssignTaskCommand command)
        {
            if (id != command.TaskId) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}/unassign")]
        public async Task<IActionResult> Unassign(Guid id)
        {
            await _mediator.Send(new UnassignTaskCommand(id));
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, ChangeTaskStatusCommand command)
        {
            if (id != command.TaskId) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
