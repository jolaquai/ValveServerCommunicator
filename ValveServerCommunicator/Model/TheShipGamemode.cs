namespace ValveServerCommunicator.Model;

/// <summary>
/// Specifies the game mode of a The Ship game server.
/// </summary>
public enum TheShipGamemode : byte
{
    /// <summary>
    /// Specifies that the game mode is Hunt.
    /// </summary>
    Hunt = 0,
    /// <summary>
    /// Specifies that the game mode is Elimination.
    /// </summary>
    Elimination = 1,
    /// <summary>
    /// Specifies that the game mode is Duel.
    /// </summary>
    Duel = 2,
    /// <summary>
    /// Specifies that the game mode is Deathmatch.
    /// </summary>
    Deathmatch = 3,
    /// <summary>
    /// Specifies that the game mode is VIP Team.
    /// </summary>
    VipTeam = 4,
    /// <summary>
    /// Specifies that the game mode is Team Elimination.
    /// </summary>
    TeamElimination = 5,
}
