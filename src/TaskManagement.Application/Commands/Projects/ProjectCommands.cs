using MediatR;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Commands.Projects
{
    public record CreateProjectCommand(string Name, string Description, Guid OwnerId) : IRequest<Guid>;
    
    public record UpdateProjectCommand(Guid ProjectId, string Name, string Description) : IRequest;
    
    public record DeleteProjectCommand(Guid ProjectId) : IRequest;

    public class ProjectCommandHandlers :
        IRequestHandler<CreateProjectCommand, Guid>
        //,
        //IRequestHandler<UpdateProjectCommand>,
        //IRequestHandler<DeleteProjectCommand>
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

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _projectRepository.AddAsync(project);
                //await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
           
                return project.Id;
            }
            catch (Exception)
            {

                //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
           
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException("Project", request.ProjectId);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                project.UpdateDetails(request.Name, request.Description);
                await _projectRepository.UpdateAsync(project, cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return Unit.Value;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new NotFoundException("Project", request.ProjectId);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _projectRepository.DeleteAsync(project);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            
        }

    }
}
