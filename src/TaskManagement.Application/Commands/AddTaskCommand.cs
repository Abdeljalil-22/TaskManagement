using MediatR;

namespace TaskManagement.Application.Commands
{
    public class AddTaskCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AssignedUserId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
