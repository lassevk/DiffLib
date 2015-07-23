using JetBrains.Annotations;

namespace DiffLib
{
    internal static class Assume
    {
        [ContractAnnotation("expression:false => halt")]
        internal static void That(bool expression)
        {
        }
    }
}