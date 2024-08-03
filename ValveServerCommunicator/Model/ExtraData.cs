using System.Runtime.InteropServices;

namespace ValveServerCommunicator.Model;

/// <summary>
/// Encapsulates information about extra data in a Valve game server.
/// Warning: this behaves like a C-style <c>union</c>. Only specific fields ever contain valid data at a given time as identified by a containing <see cref="ServerInfo"/>'s <see cref="ExtraDataFlag"/>.
/// Reading from unused fields is considered undefined behavior, especially when reading from <see langword="string"/> fields.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public readonly struct ExtraData
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Since this a union and both 'long' fields are at FieldOffset 0, just set one, no matter which of them will be used
    internal ExtraData(long data)
    {
        SteamAppId = data;
    }
    // The rest of the constructors are easier to implement because the fields are of different types
    // This also means that differing parameter names can be used to distinguish between them
    internal ExtraData(string keywords)
    {
        Keywords = keywords;
    }
    internal ExtraData(short sourceTvPort, string sourceTvName)
    {
        SourceTVPort = sourceTvPort;
        SourceTVName = sourceTvName;
    }
    internal ExtraData(ushort port)
    {
        Port = port;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    // ExtraDataFlag == SteamAppId
    /// <summary>
    /// A 64-bit Steam AppID. If the containing <see cref="ServerInfo"/> instance's <see cref="ServerInfo.ExtraDataFlag"/> is <see cref="ExtraDataKind.SteamAppId"/>, the lower 24 bits of this field are the AppID. In this case, the containing instance's <see cref="ServerInfo.AppId"/> field may have been truncated due to being forced into a <see langword="short"/>.
    /// </summary>
    [FieldOffset(0)]
    public readonly long SteamAppId;

    // ExtraDataFlag == ServerSteamId
    /// <summary>
    /// The server's Steam ID. If the containing <see cref="ServerInfo"/> instance's <see cref="ServerInfo.ExtraDataFlag"/> is <see cref="ExtraDataKind.ServerSteamId"/>, this field contains the server's Steam ID.
    /// </summary>
    [FieldOffset(0)]
    public readonly long ServerSteamId;

    // ExtraDataFlag == Keywords
    [FieldOffset(0)]
    public readonly string Keywords;

    // ExtraDataFlag == SourceTVData
    [FieldOffset(0)]
    public readonly short SourceTVPort;
    [FieldOffset(sizeof(short))]
    public readonly string SourceTVName;

    // ExtraDataFlag == Port
    [FieldOffset(0)]
    public readonly ushort Port;
}
