using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.ValueObjects;

public class Priority : ValueObject
{
    public static readonly Priority Low = new("Low", 1);
    public static readonly Priority Medium = new("Medium", 2);
    public static readonly Priority High = new("High", 3);

    private static readonly Priority[] AllPriorities = { Low, Medium, High };

    public string Value { get; private set; }
    public int NumericValue { get; private set; }

    private Priority(string value, int numericValue)
    {
        Value = value;
        NumericValue = numericValue;
    }

    public static Priority FromString(string value)
    {
        var priority = AllPriorities.FirstOrDefault(p => p.Value == value);
        if (priority == null)
            throw new ArgumentException($"Invalid priority: {value}");
        return priority;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return NumericValue;
    }

    public override string ToString() => Value;
}
