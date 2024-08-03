using System.Runtime.InteropServices;

namespace ValveServerCommunicator.Model;

/// <summary>
/// Encapsulates information about a The Ship game server.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct TheShipInfo
{
    /// <summary>
    /// The game mode of the server.
    /// </summary>
    public TheShipGamemode GameMode { get; init; }
    /// <summary>
    /// The number of witnesses required to have a player arrested.
    /// </summary>
    public byte RequiredWitnesses { get; init; }
    /// <summary>
    /// The length of time before a player is arrested while being witnessed.
    /// </summary>
    public TimeSpan ArrestTime { get; init; }
}
