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

    /// <summary>
    /// The version of the server's protocol.
    /// </summary>
    public byte ProtocolVersion { get; init; }
    /// <summary>
    /// The name of the server.
    /// </summary>
    public string ServerName { get; init; }
    /// <summary>
    /// The name of the map the server is running.
    /// </summary>
    public string MapName { get; init; }
    /// <summary>
    /// The name of the folder containing the game files.
    /// </summary>
    public string FolderName { get; init; }
    /// <summary>
    /// The name of the game the server is running.
    /// </summary>
    public string GameName { get; init; }
    /// <summary>
    /// The Steam AppID of the game the server is running.
    /// If <see cref="ExtraDataFlag"/> is <see cref="ExtraDataKind.ExtraSteamAppId"/>, this field may contain a truncated AppID. In this case, the full AppID can be found in <see cref="ExtraSteamAppId"/>.
    /// </summary>
    public short AppId { get; init; }
    /// <summary>
    /// The number of players currently on the server.
    /// </summary>
    public byte PlayerCount { get; init; }
    /// <summary>
    /// The maximum number of players the server can have.
    /// </summary>
    public byte MaxPlayers { get; init; }
    /// <summary>
    /// The number of bots currently on the server.
    /// </summary>
    public byte BotCount { get; init; }
    /// <summary>
    /// The server type.
    /// </summary>
    public ServerType ServerType { get; init; }
    /// <summary>
    /// The server environment, i.e. the operating system the server is running on.
    /// </summary>
    public ServerOperatingSystem Environment { get; init; }
    /// <summary>
    /// The visibility of the server. If this is <see langword="true"/>, a password is required to join.
    /// </summary>
    public bool IsPrivate { get; init; }
    /// <summary>
    /// The server's VAC protection status. If this is <see langword="true"/>, the server has VAC protection enabled.
    /// </summary>
    public bool VAC { get; init; }

    /// <summary>
    /// Gets whether the server is running 'The Ship'. If this is <see langword="true"/>, <see cref="TheShipInfo"/> contains a <see cref="Model.TheShipInfo"/> instance with extra information about the game server.
    /// </summary>
    public readonly bool IsTheShip => AppId == 2400;
    /// <summary>
    /// Extra information about the game server if it is running 'The Ship'.
    /// </summary>
    public TheShipInfo? TheShipInfo { get; internal set; }

    /// <summary>
    /// The version of the game the server is running.
    /// </summary>
    public string GameVersion { get; internal set; }

    /// <summary>
    /// The server's extra data flag parsed into an <see cref="ExtraDataKind"/>.
    /// </summary>
    public ExtraDataKind ExtraDataFlag { get; internal set; }

    /// <summary>
    /// The server's game port.
    /// </summary>
    public ushort? Port { get; internal set; }
    /// <summary>
    /// The server's Steam ID. If the containing <see cref="ServerInfo"/> instance's <see cref="ServerInfo.ExtraDataFlag"/> is <see cref="ExtraDataKind.ServerSteamId"/>, this field contains the server's Steam ID.
    /// </summary>
    public long? ServerSteamId { get; internal set; }
    /// <summary>
    /// The server's SourceTV spectator port number.
    /// </summary>
    public short? SourceTVPort { get; internal set; }
    /// <summary>
    /// The server's SourceTV spectator name.
    /// </summary>
    public string? SourceTVName { get; internal set; }
    /// <summary>
    /// The server's keywords.
    /// </summary>
    public string? Keywords { get; internal set; }
    /// <summary>
    /// A 64-bit Steam AppID. If the containing <see cref="ServerInfo"/> instance's <see cref="ServerInfo.ExtraDataFlag"/> is <see cref="ExtraDataKind.ExtraSteamAppId"/>, the lower 24 bits of this field are the AppID. In this case, the containing instance's <see cref="ServerInfo.AppId"/> field may have been truncated due to being forced into a <see langword="short"/>.
    /// </summary>
    public long? ExtraSteamAppId { get; internal set; }
}
