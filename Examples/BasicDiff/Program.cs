using DiffLib;

const string left = "This is a test of a basic diff operation";
const string right = "This is a test of a simple diff operation.";

IEnumerable<DiffSection> sections = Diff.CalculateSections(left.ToCharArray(), right.ToCharArray(), new DiffOptions
{
    EnablePatienceOptimization = false,
});

ReadOnlySpan<char> leftS = left.AsSpan();
ReadOnlySpan<char> rightS = right.AsSpan();
foreach (DiffSection section in sections)
{
    if (section.IsMatch)
    {
        Console.WriteLine($"= {leftS[..section.LengthInCollection1]}");
        leftS = leftS[section.LengthInCollection1..];
        rightS = rightS[section.LengthInCollection2..];
        continue;
    }

    if (section.LengthInCollection1 > 0)
    {
        Console.WriteLine($"< {leftS[..section.LengthInCollection1]}");
        leftS = leftS[section.LengthInCollection1..];
    }

    if (section.LengthInCollection2 > 0)
    {
        Console.WriteLine($"> {rightS[..section.LengthInCollection2]}");
        rightS = rightS[section.LengthInCollection2..];
    }
}