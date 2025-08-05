namespace TaskManagement.Application.ReadModels.Projects
{
    public record ProjectDetails
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid OwnerId { get; init; }
        public string OwnerName { get; init; } = string.Empty;
        public int TotalTasks { get; init; }
        public int CompletedTasks { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? LastUpdated { get; init; }
        public IReadOnlyList<TaskDetails> Tasks { get; init; } = new List<TaskDetails>();
    }

    public record TaskDetails
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid? AssignedUserId { get; init; }
        public string? AssignedUserName { get; init; }
        public DateTime? DueDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public IReadOnlyList<string> Labels { get; init; } = new List<string>();
    }

    public record ProjectSummary
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int TotalTasks { get; init; }
        public int CompletedTasks { get; init; }
        public DateTime LastUpdated { get; init; }
    }
}
