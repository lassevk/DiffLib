<Query Kind="Program">
  <Connection>
    <ID>9f392ba2-0c9a-4727-9538-5646bfcbbce3</ID>
    <Persist>true</Persist>
    <Driver>AstoriaAuto</Driver>
    <Server>https://odata.sqlazurelabs.com/OData.svc/v0.1/rp1uiewita/StackOverflow</Server>
  </Connection>
  <Reference Relative="..\DiffLib\bin\Debug\DiffLib.dll">C:\dev\VS.NET\DiffLib\DiffLib\bin\Debug\DiffLib.dll</Reference>
  <Namespace>DiffLib</Namespace>
</Query>

const int PostId = 424220;

void Main()
{
	var history =
        (from ph in PostHistories
         where ph.PostId == PostId
            && (ph.PostHistoryTypeId == 2 || ph.PostHistoryTypeId == 5)
         orderby ph.CreationDate
         select ph).ToArray();
    for (int index = 0; index < history.Length - 1; index++)
    {
        DumpTextDiff(
            history[index].CreationDate, history[index].Text,
            history[index + 1].CreationDate, history[index + 1].Text);
    }
}

static string[] SplitLines(string text)
{
    var lines = new List<string>();
    using (var reader = new StringReader(text))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
            lines.Add(line);
    }
    return lines.ToArray();
}

static void DumpTextDiff(DateTime? dt1, string body1, DateTime? dt2, string body2)
{
    var diff = new AlignedDiff<string>(
        SplitLines(body1),
        SplitLines(body2),
        EqualityComparer<string>.Default,
        new StringSimilarityComparer(),
        new StringAlignmentFilter());
    var caption = string.Format("Diff from " + dt1 + " to " + dt2 + " of post " + PostId);
    DumpDiff(diff, caption);
}

static void DumpDiff(IEnumerable<AlignedDiffChange<string>> changes, string caption)
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
    Util.RawHtml(html.ToString()).Dump(caption);
}