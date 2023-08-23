namespace DiffLib;

/// <summary>
/// This class is used to specify options to the diff algorithm.
/// </summary>
public class DiffOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the patience optimization is enabled. Default is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// For more information about the patience optimization, see this question on Stack Overflow:
    /// https://stackoverflow.com/questions/4045017/what-is-git-diff-patience-for
    /// </remarks>
#if NETSTANDARD1_0
    public bool EnablePatienceOptimization { get; set; } = true;
#endif
#if NETSTANDARD2_0
    public bool EnablePatienceOptimization { get; set; } = true;
#endif
#if NETSTANDARD2_1
    public bool EnablePatienceOptimization { get; set; } = true;
#endif
#if !NETSTANDARD1_0 && !NETSTANDARD2_0 && !NETSTANDARD2_1
    public bool EnablePatienceOptimization { get; init; } = true;
#endif
}