using JetBrains.Annotations;

namespace DiffLib
{
    [PublicAPI]
    public enum DiffOperation
    {
        None,
        Insert,
        Delete,
        Replace,
        Modify
    }
}