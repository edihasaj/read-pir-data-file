namespace ConsoleApp1;

public class LineData
{
    public LineData(string time, string value)
    {
        Time = time;
        Value = value;
    }

    public string Time { get; set; }
    public string Value { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Difference { get; set; }
    public string? TotalPersons { get; set; }
}