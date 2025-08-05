using MediatR;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;


using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Application.Commands.Tasks
{
    public record AddTaskCommand(
        Guid ProjectId,
        string Title,
        string Description,
        DateTime? DueDate,
        Guid? AssignedUserId,
        Priority? Priority = null
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
    
    public record ChangeTaskStatusCommand(Guid TaskId, Domain.ValueObjects.TaskStatus NewStatus) : IRequest;

    public class TaskCommandHandlers :
        IRequestHandler<AddTaskCommand, Guid>
        //IRequestHandler<UpdateTaskCommand>,
        //IRequestHandler<AssignTaskCommand>,
        //IRequestHandler<UnassignTaskCommand>,
        //IRequestHandler<ChangeTaskStatusCommand>
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
                ?? throw new NotFoundException($"Project",request.ProjectId);

            var task = project.AddTask(request.Title, request.Description, request.DueDate);
            task.SetPriority(request.Priority ?? Priority.Medium);

            if (request.AssignedUserId.HasValue)
            {
                task.AssignTo(request.AssignedUserId.Value);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return task.Id;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task", request.TaskId);

            task.UpdateDetails(request.Title, request.Description, request.DueDate);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task", request.TaskId);

            task.AssignTo(request.UserId);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UnassignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task", request.TaskId);

            task.Unassign();
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId)
                ?? throw new NotFoundException($"Task", request.TaskId);

            task.ChangeStatus(request.NewStatus);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
