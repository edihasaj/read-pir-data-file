// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using ExportTable;
using static System.Int32;

var lines = File.ReadAllLines(@"C:\Users\Edi\Desktop\pir.txt");
Console.WriteLine("Contents of pir.txt = ");

var list = new List<LineData>();
DateTime? startTime = null;
var hasPersonLine = false;
var isStartTime = true;
var totalPersons = 0;

foreach (var line in lines)
{
    if (line.Contains("pir value", StringComparison.OrdinalIgnoreCase))
    {
        var time = line[..12];
        var value = line.Substring(31, 1);
        TryParse(value, out var hasPerson);
        var lineData = new LineData(time, hasPerson == 1 ? "Ka person" : "Nuk ka person");

        var writeFirstLine = false;
        if (isStartTime)
        {
            startTime = DateTime.Parse(time);
            isStartTime = false;
            hasPersonLine = false;
            writeFirstLine = true;
        }

        bool writeTheTime;
        DateTime? endTime;
        if (hasPersonLine != (hasPerson == 1))
        {
            endTime = DateTime.Parse(time);
            writeTheTime = true;
        }
        else
        {
            writeTheTime = false;
            endTime = null;
        }

        lineData.StartTime = endTime is null && !writeTheTime && !writeFirstLine ? string.Empty : 
            DateTime.Parse(startTime.ToString() ?? string.Empty).ToLongTimeString();
        lineData.EndTime = startTime is null || endTime is null ? string.Empty : 
            DateTime.Parse(endTime.ToString() ?? string.Empty).ToLongTimeString();
        lineData.Difference = startTime is not null && endTime is not null 
            ? (endTime - startTime).ToString() : string.Empty;
        list.Add(lineData);

        if (isStartTime)
        {
            startTime = null;
        }
        if (hasPersonLine != (hasPerson == 1))
        {
            if (hasPerson == 1) 
                totalPersons++;
            startTime = DateTime.Parse(time);
            hasPersonLine = hasPerson == 1;
        }

        Console.WriteLine("time \t" + time);
        Console.WriteLine("value \t" + value);
    }
}

list.Add(new LineData(string.Empty, string.Empty)
{
    TotalPersons = totalPersons.ToString()
});

list.ToTable()
    .AddColumn(customer => customer.Time)
    .AddColumn(customer => customer.Value)
    .AddColumn(customer => customer.StartTime)
    .AddColumn(customer => customer.EndTime)
    .AddColumn(customer => customer.Difference)
    .AddColumn(customer => customer.TotalPersons)
    .GenerateSpreadsheet(@"C:\Users\Edi\Desktop\pir.xlsx");

Console.WriteLine("Press any key to exit.");
Console.ReadKey();