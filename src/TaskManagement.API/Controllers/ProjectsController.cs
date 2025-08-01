using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Commands.Projects;
using TaskManagement.Application.Commands.Tasks;
using TaskManagement.Application.Queries.Projects;
using TaskManagement.Application.Queries.Tasks;
using TaskManagement.Application.ReadModels.Projects;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Read Operations
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProjectSummary>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetails>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectDetailsQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IReadOnlyList<ProjectSummary>>> GetByOwner(Guid ownerId)
        {
            var result = await _mediator.Send(new GetProjectsByOwnerQuery(ownerId));
            return Ok(result);
        }

        // Write Operations
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateProjectCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectCommand command)
        {
            if (id != command.ProjectId) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }

        // Task Operations
        [HttpPost("{projectId}/tasks")]
        public async Task<ActionResult<Guid>> AddTask(Guid projectId, AddTaskCommand command)
        {
            if (projectId != command.ProjectId) return BadRequest();
            var taskId = await _mediator.Send(command);
            return Ok(taskId);
        }

        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<IReadOnlyList<TaskSummary>>> GetProjectTasks(Guid projectId)
        {
            var tasks = await _mediator.Send(new GetProjectTasksQuery(projectId));
            return Ok(tasks);
        }
    }
}
