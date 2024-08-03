using System.Runtime.InteropServices;

namespace ValveServerCommunicator.Model;

/// <summary>
/// Encapsulates information about a Valve game server.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ServerInfo
{
    internal ServerInfo(byte extraDataFlag)
    {
        ExtraDataFlag = (ExtraDataKind)extraDataFlag;
    }

    public byte ProtocolVersion { get; init; }
    public string ServerName { get; init; }
    public string MapName { get; init; }
    public string FolderName { get; init; }
    public string GameName { get; init; }
    public short AppId { get; init; }
    public byte PlayerCount { get; init; }
    public byte MaxPlayers { get; init; }
    public byte BotCount { get; init; }
    public ServerType ServerType { get; init; }
    public ServerOperatingSystem Environment { get; init; }
    public bool IsPrivate { get; init; }
    public bool VAC { get; init; }

    /// <summary>
    /// Gets whether the server is running 'The Ship'. If this is <see langword="true"/>, <see cref="TheShipInfo"/> contains a <see cref="Model.TheShipInfo"/> instance with extra information about the game server.
    /// </summary>
    public readonly bool IsTheShip => AppId == 2400;
    /// <summary>
    /// Extra information about the game server if it is running 'The Ship'.
    /// </summary>
    public TheShipInfo? TheShipInfo { get; internal set; }

    public string GameVersion { get; internal set; }

    public ExtraDataKind ExtraDataFlag { get; internal set; }
    public ExtraData ExtraDataUnion { get; internal set; }
}
