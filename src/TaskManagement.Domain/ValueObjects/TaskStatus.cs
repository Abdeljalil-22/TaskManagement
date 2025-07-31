using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.ValueObjects;

public class TaskStatus : ValueObject
{
    public static readonly TaskStatus ToDo = new("To Do");
    public static readonly TaskStatus InProgress = new("In Progress");
    public static readonly TaskStatus Done = new("Done");

    private static readonly TaskStatus[] AllStatuses = { ToDo, InProgress, Done };

    public string Value { get; private set; }

    private TaskStatus(string value)
    {
        Value = value;
    }

    public static TaskStatus FromString(string value)
    {
        var status = AllStatuses.FirstOrDefault(s => s.Value == value);
        if (status == null)
            throw new ArgumentException($"Invalid task status: {value}");
        return status;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
