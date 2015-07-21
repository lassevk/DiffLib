using JetBrains.Annotations;

namespace DiffLib.OldImplementation
{
    internal static class Assume
    {
        [ContractAnnotation("expression:false => halt")]
        internal static void That(bool expression)
        {
        }
    }
}