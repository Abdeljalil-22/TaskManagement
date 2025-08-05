namespace TaskManagement.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public List<ProjectTaskDto> Tasks { get; set; }
    }

    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AssignedUserId { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
}
