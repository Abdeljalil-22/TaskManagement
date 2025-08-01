using MediatR;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Commands.Tasks
{
    public record AddTaskCommand(
        Guid ProjectId,
        string Title,
        string Description,
        DateTime? DueDate,
        Guid? AssignedUserId,
        Priority Priority = Priority.Medium
    ) : IRequest<Guid>;

    public record UpdateTaskCommand(
        Guid TaskId,
        string Title,
        string Description,
        DateTime? DueDate,
        Priority Priority
    ) : IRequest;

    public record AssignTaskCommand(Guid TaskId, Guid UserId) : IRequest;
    
    public record UnassignTaskCommand(Guid TaskId) : IRequest;
    
    public record ChangeTaskStatusCommand(Guid TaskId, TaskStatus NewStatus) : IRequest;

    public class TaskCommandHandlers :
        IRequestHandler<AddTaskCommand, Guid>,
        IRequestHandler<UpdateTaskCommand>,
        IRequestHandler<AssignTaskCommand>,
        IRequestHandler<UnassignTaskCommand>,
        IRequestHandler<ChangeTaskStatusCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskCommandHandlers(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException($"Project with ID {request.ProjectId} not found");

            var task = project.AddTask(request.Title, request.Description, request.DueDate);
            if (request.AssignedUserId.HasValue)
            {
                task.AssignTo(request.AssignedUserId.Value);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return task.Id;
        }

        public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task with ID {request.TaskId} not found");

            task.UpdateDetails(request.Title, request.Description, request.DueDate);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task with ID {request.TaskId} not found");

            task.AssignTo(request.UserId);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task Handle(UnassignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task with ID {request.TaskId} not found");

            task.Unassign();
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task with ID {request.TaskId} not found");

            task.ChangeStatus(request.NewStatus);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
