namespace TaskManagement.Application.ReadModels.Tasks
{
    public record TaskDetails
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid ProjectId { get; init; }
        public string ProjectName { get; init; } = string.Empty;
        public Guid? AssignedUserId { get; init; }
        public string? AssignedUserName { get; init; }
        public DateTime? DueDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public IReadOnlyList<string> Labels { get; init; } = new List<string>();
        public DateTime CreatedAt { get; init; }
        public DateTime? LastUpdated { get; init; }
    }

    public record TaskSummary
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string ProjectName { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public DateTime? DueDate { get; init; }
    }
}
