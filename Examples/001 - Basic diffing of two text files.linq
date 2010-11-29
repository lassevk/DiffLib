<Query Kind="Program">
  <Reference Relative="..\DiffLib\bin\Debug\DiffLib.dll">C:\Dev\VS.NET\DiffLib\DiffLib\bin\Debug\DiffLib.dll</Reference>
  <Namespace>DiffLib</Namespace>
</Query>

string[] textfile1 = new[] {
    "This line is the same",
    "This line is also the same",
    "This line has been deleted",
    "This line is yet another equal line",
    "This is also another equal line",
    "This line is changed",
    "This line is also changed",
    "This is the final equal line",
};

string[] textfile2 = new[] {
    "This line is the same",
    "This line is also the same",
    "This line is yet another equal line",
    "This line has been added",
    "This is also another equal line",
    "This line was changed to this",
    "And then this was added",
    "And this line was changed to this",
    "This is the final equal line",
};

void Main()
{
    DumpDiff(new AlignedDiff<string>(textfile1, textfile2, EqualityComparer<string>.Default, new StringSimilarityComparer()));
}

static void DumpDiff(IEnumerable<AlignedDiffChange<string>> changes)
{
    var html = new StringBuilder();
    html.Append("<div style='font-family: courier;'>");
    int i1 = 0;
    int i2 = 0;
    
    Func<string, string> filter = delegate(string input)
    {
        return input.Replace(" ", "\x00a0").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    };
    
    foreach (var change in changes)
    {
        switch (change.Change)
        {
            case ChangeType.Same:
                html.Append("<div>\x00a0\x00a0" + filter(change.Element1) + "</div>");
                break;
                
            case ChangeType.Added:
                html.Append("<div style='background-color: #ccffcc;'>+\x00a0" + filter(change.Element2) + "</div>");
                break;

            case ChangeType.Deleted:
                html.Append("<div style='background-color: #ffcccc;'>-\x00a0" + filter(change.Element1) + "</div>");
                break;

            case ChangeType.Changed:
                var diff = new Diff<char>(change.Element1, change.Element2).Generate().ToArray();
                int ii1 = 0;
                int ii2 = 0;
                html.Append("<div>*\x00a0");
                foreach (var section in diff)
                {
                    if (section.Equal)
                        html.Append(filter(change.Element1.Substring(ii1, section.Length1)));
                    else
                    {
                        html.Append("<span style='background-color: #ff8080;'>" + filter(change.Element1.Substring(ii1, section.Length1)) + "</span>");
                        html.Append("<span style='background-color: #ccffcc;'>" + filter(change.Element2.Substring(ii2, section.Length2)) + "</span>");
                    }
                    
                    ii1 += section.Length1;
                    ii2 += section.Length2;
                }
                html.Append("</div>");
                break;
        }
    }
    html.Append("</div>");
    Util.RawHtml(html.ToString()).Dump();
}