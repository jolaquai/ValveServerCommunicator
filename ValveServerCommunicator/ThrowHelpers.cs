using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ValveServerCommunicator;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression(nameof(condition))] string expr = "")
    {
        if (!condition)
        {
            throw new InvalidOperationException($"Assertion failed by violating expression: '{expr}'");
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertBytesSent(int expected, int actual)
    {
        if (expected != actual)
        {
            throw new InvalidOperationException($"Bytes sent do not match the expected amount. Expected: {expected}, actual: {actual}");
        }
    }
}
