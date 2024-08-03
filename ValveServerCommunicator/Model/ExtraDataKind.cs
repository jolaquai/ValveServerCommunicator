namespace ValveServerCommunicator.Model;

/// <summary>
/// Specifies the kind of extra data in a Valve game server.
/// </summary>
public enum ExtraDataKind
{
    /// <summary>
    /// Specifies that the extra data is a Steam AppID.
    /// </summary>
    SteamAppId = 0x01,
    /// <summary>
    /// Specifies that the extra data is a server Steam ID.
    /// </summary>
    ServerSteamId = 0x10,
    /// <summary>
    /// Specifies that the extra data is keywords.
    /// </summary>
    Keywords = 0x20,
    /// <summary>
    /// Specifies that the extra data is SourceTV data.
    /// </summary>
    SourceTVData = 0x40,
    /// <summary>
    /// Specifies that the extra data is the server's game port.
    /// </summary>
    Port = 0x80
}
