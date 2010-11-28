<Query Kind="Program">
  <Reference Relative="..\DiffLib\bin\Debug\DiffLib.dll">C:\dev\vs.net\difflib\DiffLib\bin\Debug\DiffLib.dll</Reference>
  <Namespace>DiffLib</Namespace>
</Query>

string[] textfile1 = new[] {
    "This line is the same",
    "This line is also the same",
    "This line has been deleted",
    "This line is yet another equal line",
    "This is also another equal line",
    "This line is changed",
    "This is the final equal line",
};

string[] textfile2 = new[] {
    "This line is the same",
    "This line is also the same",
    "This line is yet another equal line",
    "This line has been added",
    "This is also another equal line",
    "This line was changed to this",
    "This is the final equal line",
};

void Main()
{
	var diff = new AlignedDiff<string>(textfile1, textfile2, EqualityComparer<string>.Default, new StringSimilarityComparer());
    DumpDiff(diff.Generate());
}

static void DumpDiff(IEnumerable<AlignedDiffChange<string>> changes)
{
    var html = new StringBuilder();
    html.Append("<div style='font-family: courier;'>");
    int i1 = 0;
    int i2 = 0;
    foreach (var change in changes)
    {
        switch (change.Type)
        {
            case ChangeType.Same:
                html.Append("<div>\x00a0\x00a0" + change.Element1 + "</div>");
                break;
                
            case ChangeType.Added:
                html.Append("<div style='background-color: #ccffcc;'>+\x00a0" + change.Element2 + "</div>");
                break;

            case ChangeType.Deleted:
                html.Append("<div style='background-color: #ffcccc;'>-\x00a0" + change.Element1 + "</div>");
                break;
        }
    }
    html.Append("</div>");
    Util.RawHtml(html.ToString()).Dump();
}