namespace ValveServerCommunicator.Model;

/// <summary>
/// Encapsulates information about a player in a Valve game server.
/// </summary>
public readonly struct Player
{
    /// <summary>
    /// The index of the player in the player list.
    /// </summary>
    public int PlayerIndex { get; init; }
    /// <summary>
    /// The in-game name of the player.
    /// </summary>
    public string PlayerName { get; init; }
    /// <summary>
    /// The score assigned to the player. The value is arbitrary and not tied to a specific metric (on some servers, it could be equal to the number of frags, something entirely different on others).
    /// </summary>
    public int Score { get; init; }
    /// <summary>
    /// The length of the current session of the player. A "session" begins whenever the player joins the server and ends when they leave.
    /// </summary>
    public TimeSpan OnlineTime { get; init; }
}
