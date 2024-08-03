namespace ValveServerCommunicator.Model;

/// <summary>
/// Specifies the operating system of a Valve game server.
/// </summary>
public enum ServerOperatingSystem : byte
{
    /// <summary>
    /// Specifies that the server is running on Linux.
    /// </summary>
    Linux = (byte)'l',
    /// <summary>
    /// Specifies that the server is running on Windows.
    /// </summary>
    Windows = (byte)'w',
    /// <summary>
    /// Specifies that the server is running on macOS.
    /// </summary>
    Mac = (byte)'m',
    /// <summary>
    /// Specifies that the server is running on macOS.
    /// </summary>
    MacLegacy = (byte)'o',
}
