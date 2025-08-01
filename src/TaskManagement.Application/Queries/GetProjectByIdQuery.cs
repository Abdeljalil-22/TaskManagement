using MediatR;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Queries
{
    public class GetProjectByIdQuery : IRequest<Project>
    {
        public Guid ProjectId { get; set; }
        public GetProjectByIdQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
