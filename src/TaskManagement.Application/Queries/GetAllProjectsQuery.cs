using MediatR;
using TaskManagement.Domain.Entities;
using System.Collections.Generic;

namespace TaskManagement.Application.Queries
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<Project>>
    {
    }
}
