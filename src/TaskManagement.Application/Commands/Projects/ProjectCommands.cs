using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Commands.Projects
{
    public record CreateProjectCommand(string Name, string Description, Guid OwnerId) : IRequest<Guid>;
    
    public record UpdateProjectCommand(Guid ProjectId, string Name, string Description) : IRequest;
    
    public record DeleteProjectCommand(Guid ProjectId) : IRequest;

    public class ProjectCommandHandlers :
        IRequestHandler<CreateProjectCommand, Guid>,
        IRequestHandler<UpdateProjectCommand>,
        IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectCommandHandlers(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(request.Name, request.Description, request.OwnerId);
            await _projectRepository.AddAsync(project);
            await _unitOfWork.CommitAsync(cancellationToken);
            return project.Id;
        }

        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException($"Project with ID {request.ProjectId} not found");

            project.UpdateDetails(request.Name, request.Description);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException($"Project with ID {request.ProjectId} not found");

            _projectRepository.Delete(project);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
