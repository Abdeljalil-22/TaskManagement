using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Commands
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddTaskCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null) throw new Exception("Project not found");
            var task = project.AddTask(request.Title, request.Description, request.DueDate);
            await _unitOfWork.CommitTransactionAsync();
            return task.Id;
        }
    }
}
