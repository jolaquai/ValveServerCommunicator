using System.Runtime.InteropServices;

namespace ValveServerCommunicator.Model;

/// <summary>
/// Encapsulates information about a rule on a Valve game server.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct ServerRule
{
    /// <summary>
    /// The name of the rule.
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// The value of the rule.
    /// </summary>
    public string Value { get; init; }
}
