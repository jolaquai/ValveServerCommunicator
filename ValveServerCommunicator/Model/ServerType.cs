namespace ValveServerCommunicator.Model;

/// <summary>
/// Specifies the type of a Valve game server.
/// </summary>
public enum ServerType : byte
{
    /// <summary>
    /// Specifies that the server is dedicated.
    /// </summary>
    Dedicated = (byte)'d',
    /// <summary>
    /// Specifies that the server is non-dedicated.
    /// </summary>
    NonDedicated = (byte)'l',
    /// <summary>
    /// Specifies that the server is a SourceTV relay (proxy).
    /// </summary>
    SourceTVRelay = (byte)'p',
}
