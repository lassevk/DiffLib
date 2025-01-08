using System;

namespace DiffLib;

/// <summary>
/// This class is used to specify options to the diff algorithm.
/// </summary>
public record struct DiffOptions()
{
    private readonly int _ContextSize = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the patience optimization is enabled. Default is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// For more information about the patience optimization, see this question on Stack Overflow:
    /// https://stackoverflow.com/questions/4045017/what-is-git-diff-patience-for
    /// </remarks>
    public bool EnablePatienceOptimization { get; init; } = true;

    /// <summary>
    /// Gets or sets the context size, which indicates how many consequtive equal items must be present before
    /// a new synchronization point is considered. The default value is 1.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// ContextSize must be equal to or greater than 1.
    /// </exception>
    public int ContextSize
    {
        get => _ContextSize;
        init
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), value, "ContextSize must be greater than 1.");
            _ContextSize = value;
        }
    }
}