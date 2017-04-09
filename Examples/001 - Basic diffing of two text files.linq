<Query Kind="Program">
  <Reference Relative="..\DiffLib\bin\Debug\netstandard1.0\DiffLib.dll">D:\Dev\VS.NET\DiffLib\DiffLib\bin\Debug\netstandard1.0\DiffLib.dll</Reference>
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
    var sections = Diff.CalculateSections(textfile1, textfile2);
    var elements = Diff.AlignElements(textfile1, textfile2, sections, new StringSimilarityDiffElementAligner());
    DumpDiff(elements);
}

static void DumpDiff(IEnumerable<DiffElement<string>> elements)
{
    var html = new StringBuilder();
    html.Append("<div style='font-family: courier;'>");
    int i1 = 0;
    int i2 = 0;
    
    Func<string, string> filter = delegate(string input)
    {
        return input.Replace(" ", "\x00a0").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    };
    
    foreach (var element in elements)
    {
        switch (element.Operation)
        {
            case DiffOperation.Match:
                html.Append("<div>\x00a0\x00a0" + filter(element.ElementFromCollection1.Value) + "</div>");
                break;
                
            case DiffOperation.Insert:
                html.Append("<div style='background-color: #ccffcc;'>+\x00a0" + filter(element.ElementFromCollection2.Value) + "</div>");
                break;

            case DiffOperation.Delete:
                html.Append("<div style='background-color: #ffcccc;'>-\x00a0" + filter(element.ElementFromCollection1.Value) + "</div>");
                break;

			case DiffOperation.Replace:
			case DiffOperation.Modify:
				var sections = Diff.CalculateSections(element.ElementFromCollection1.Value.ToCharArray(), element.ElementFromCollection2.Value.ToCharArray()).ToArray();
                int ii1 = 0;
                int ii2 = 0;
                html.Append("<div>*\x00a0");
                foreach (var section in sections)
                {
                    if (section.IsMatch)
                        html.Append(filter(element.ElementFromCollection1.Value.Substring(ii1, section.LengthInCollection1)));
                    else
                    {
                        html.Append("<span style='background-color: #ff8080;'>" + filter(element.ElementFromCollection1.Value.Substring(ii1, section.LengthInCollection1)) + "</span>");
                        html.Append("<span style='background-color: #ccffcc;'>" + filter(element.ElementFromCollection2.Value.Substring(ii2, section.LengthInCollection2)) + "</span>");
                    }
                    
                    ii1 += section.LengthInCollection1;
                    ii2 += section.LengthInCollection2;
                }
                html.Append("</div>");
                break;
        }
    }
    html.Append("</div>");
    Util.RawHtml(html.ToString()).Dump();
}