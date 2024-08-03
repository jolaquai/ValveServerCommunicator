using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

using ValveServerCommunicator.Model;

namespace ValveServerCommunicator;

/// <summary>
/// Represents a client for a Valve game server.
/// </summary>
public sealed class ValveServerClient : IDisposable
{
    private UdpClient _udpClient;

    private ValveServerClient() { }
    public static ValveServerClient Create(string ip, ushort port)
    {
        var client = new ValveServerClient();
        client._udpClient = new UdpClient();
        client._udpClient.Connect(IPAddress.Parse(ip), port);
        return client;
    }

    // Requests a challenge for the specified header and returns its value
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<long> QueryChallengeAsync(byte header)
    {
        byte[] challengeData = [0xff, 0xff, 0xff, 0xff, header, 0xff, 0xff, 0xff, 0xff];
        var result = await SendImplAsync(challengeData).ConfigureAwait(false);
        nint ptr = 5;
        return result.Read<long>(ref ptr);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<byte[]> SendImplAsync(byte[] data)
    {
        var sent = await _udpClient.Client.SendAsync(data).ConfigureAwait(false);
        ThrowHelpers.AssertBytesSent(data.Length, sent);

        var result = await _udpClient.ReceiveAsync().ConfigureAwait(false);
        return result.Buffer;
    }

    // A2S_PLAYER and A2S_RULES both need a challenge
    /// <summary>
    /// Sends an <c>A2S_INFO</c> query to the server.
    /// </summary>
    /// <returns>A <see cref="ServerInfo"/> instance detailing the server's information.</returns>
    public async Task<ServerInfo> QueryInfoAsync()
    {
        byte[] data = [0xff, 0xff, 0xff, 0xff, 0x54, .. Encoding.ASCII.GetBytes("Source Engine Query"), 0];
        var result = await SendImplAsync(data).ConfigureAwait(false);
        // In response to this, we may get the data OR a challenge number
        // If we get a challenge number, we need to send the same data with the challenge number appended to it
        // If we get the data, we can parse it
        if (result.Length == 9 && result[4] == 0x41)
        {
            // This is a challenge
            data = [.. data, .. result[5..]];
            result = await SendImplAsync(data).ConfigureAwait(false);
        }

        // At this point, we're sure to have the actual ServerInfo data in result
        var buf = result;
        nint ptr = 5;
        var info = new ServerInfo()
        {
            ProtocolVersion = buf.Read<byte>(ref ptr),
            ServerName = buf.Read<string>(ref ptr),
            MapName = buf.Read<string>(ref ptr),
            FolderName = buf.Read<string>(ref ptr),
            GameName = buf.Read<string>(ref ptr),
            AppId = buf.Read<short>(ref ptr),
            PlayerCount = buf.Read<byte>(ref ptr),
            MaxPlayers = buf.Read<byte>(ref ptr),
            BotCount = buf.Read<byte>(ref ptr),
            ServerType = (ServerType)buf.Read<byte>(ref ptr),
            Environment = (ServerOperatingSystem)buf.Read<byte>(ref ptr),
            IsPrivate = buf.Read<bool>(ref ptr),
            VAC = buf.Read<bool>(ref ptr),
        };

        // If running 'The Ship', there's three extra bytes right here
        if (info.IsTheShip)
        {
            info.TheShipInfo = new TheShipInfo()
            {
                GameMode = (TheShipGamemode)buf.Read<byte>(ref ptr),
                RequiredWitnesses = buf.Read<byte>(ref ptr),
                ArrestTime = TimeSpan.FromSeconds(buf.Read<byte>(ref ptr))
            };
        }

        info.GameVersion = buf.Read<string>(ref ptr);

        // Now parse and set extra data
        info.ExtraDataFlag = (ExtraDataKind)buf.Read<byte>(ref ptr);
        if (info.ExtraDataFlag.HasFlag(ExtraDataKind.ExtraSteamAppId))
        {
            var fullValue = buf.Read<long>(ref ptr);
            // 24-bit mask
            const long mask = (1 << 24) - 1;
            info.ExtraSteamAppId = fullValue & mask;
        }
        if (info.ExtraDataFlag.HasFlag(ExtraDataKind.ServerSteamId))
        {
            info.ServerSteamId = buf.Read<long>(ref ptr);
        }
        if (info.ExtraDataFlag.HasFlag(ExtraDataKind.Keywords))
        {
            info.Keywords = buf.Read<string>(ref ptr);
        }
        if (info.ExtraDataFlag.HasFlag(ExtraDataKind.SourceTVData))
        {
            info.SourceTVPort = buf.Read<short>(ref ptr);
            info.SourceTVName = buf.Read<string>(ref ptr);
        }
        if (info.ExtraDataFlag.HasFlag(ExtraDataKind.Port))
        {
            info.Port = buf.Read<ushort>(ref ptr);
        }

        return info;
    }
    /// <summary>
    /// Sends an <c>A2S_PLAYER</c> query to the server.
    /// </summary>
    /// <returns>An array of <see cref="Player"/> instances detailing the players on the server.</returns>
    public async Task<Player[]> QueryPlayersAsync()
    {
        var challenge = await QueryChallengeAsync(0x55).ConfigureAwait(false);
        byte[] a2s_playerData = [0xff, 0xff, 0xff, 0xff, 0x55, (byte)(challenge & 0xff), (byte)((challenge >>= 8) & 0xff), (byte)((challenge >>= 8) & 0xff), (byte)((challenge >>= 8) & 0xff)];

        var buf = await SendImplAsync(a2s_playerData).ConfigureAwait(false);

        nint ptr = 5;

        List<Player> players = [];
        while (ptr < buf.Length)
        {
            players.Add(new Player()
            {
                PlayerIndex = buf[ptr++],
                PlayerName = buf.Read<string>(ref ptr),
                Score = buf.Read<int>(ref ptr),
                OnlineTime = TimeSpan.FromSeconds((int)buf.Read<float>(ref ptr))
            });
        }

        return [.. players];
    }
    /// <summary>
    /// Sends an <c>A2S_RULES</c> query to the server.
    /// </summary>
    /// <returns>An array of <see cref="ServerRule"/> instances detailing the rules on the server.</returns>
    public async Task<ServerRule[]> QueryRulesAsync()
    {
        var challenge = await QueryChallengeAsync(0x56).ConfigureAwait(false);
        byte[] a2s_rulesData = [0xff, 0xff, 0xff, 0xff, 0x56, (byte)(challenge & 0xff), (byte)((challenge >>= 8) & 0xff), (byte)((challenge >>= 8) & 0xff), (byte)((challenge >>= 8) & 0xff)];

        var buf = await SendImplAsync(a2s_rulesData).ConfigureAwait(false);

        nint ptr = 5;

        List<ServerRule> players = [];
        while (ptr < buf.Length)
        {
            players.Add(new ServerRule()
            {
                Name = buf.Read<string>(ref ptr),
                Value = buf.Read<string>(ref ptr)
            });
        }
        return [.. players];
    }
    /// <summary>
    /// Sends an <c>A2S_PING</c> query to the server.
    /// This is considered to no longer work for CS:S and TF2 servers.
    /// </summary>
    /// <returns>The latency to the server in milliseconds.</returns>
    public async Task<long> QueryPingAsync()
    {
        byte[] data = [0xff, 0xff, 0xff, 0xff, 0x69];
        var sw = Stopwatch.StartNew();
        var result = await SendImplAsync(data).ConfigureAwait(false);
        sw.Stop();
        switch (result.Length)
        {
            case 6: // Goldsource server
                break;
            case 20: // Source server
                break;
        }
        // Latency == round-trip time
        return sw.ElapsedMilliseconds;
    }

    #region Dispose pattern
    private bool _disposed;
    /// <summary>
    /// Releases the unmanaged and optionally the managed resources used by this <see cref="ValveServerClient"/> instance.
    /// </summary>
    /// <param name="disposing">Whether to release the managed resources used by this <see cref="ValveServerClient"/> instance.</param>
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _udpClient.Dispose();
            }

            // Dispose of unmanaged resources (native allocated memory etc.)

            _disposed = true;
        }
    }
    /// <summary>
    /// Finalizes this <see cref="ValveServerClient"/> instance, releasing any unmanaged resources.
    /// </summary>
    ~ValveServerClient()
    {
        Dispose(false);
    }
    /// <summary>
    /// Releases the managed and unmanaged resources used by this <see cref="ValveServerClient"/> instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}