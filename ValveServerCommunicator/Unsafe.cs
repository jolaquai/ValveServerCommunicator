using System.Globalization;
using System.Text;

namespace ValveServerCommunicator;

internal static unsafe class Unsafe
{
    public static readonly Encoding Encoding = Encoding.UTF8;

    public static byte[] FromAscii(string byteRepr)
    {
        var buf = new byte[byteRepr.Length / 2];

        var span = byteRepr.AsSpan();
        var start = 0;
        var i = 0;
        while (start < byteRepr.Length)
        {
            var slice = span[start..(start + 2)];
            buf[i++] = byte.Parse(slice, NumberStyles.HexNumber);
            start += 2;
        }
        return buf;
    }

    public static string StringFromBytes(this byte[] bytes, ref nint ptr)
    {
        // Read from bytes[readFrom] until encountering a \0
        var start = ptr;
        while (bytes[ptr++] != 0) { }
        fixed (byte* p = &bytes[start])
        {
            return Encoding.GetString(p, (int)(ptr - 1 - start));
        }
    }

    public static T Read<T>(this byte[] bytes, ref nint ptr)
    {
        var typeOfT = typeof(T);
        if (!typeOfT.IsValueType && typeOfT != typeof(string))
        {
            throw new ArgumentException("T must be a value type or string.");
        }

        if (typeOfT == typeof(string))
        {
            return (T)(object)bytes.StringFromBytes(ref ptr);
        }
        else if (typeOfT == typeof(byte))
        {
            return (T)(object)bytes[ptr++];
        }
        else if (typeOfT == typeof(bool))
        {
            return (T)(object)(bytes[ptr++] != 0);
        }

        T value;
        var incr = sizeof(T);
        fixed (void* p = &bytes[ptr])
        {
            var tPtr = (T*)p;
            value = *tPtr;
            ptr += incr;
        }

        return value;
    }
}