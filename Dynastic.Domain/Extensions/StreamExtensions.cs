using System.Text;

namespace Dynastic.Domain.Extensions;

public static class StreamExtensions
{
    public static string ReadBytesToString(this Stream stream, int bufferLength)
    {
        var offset = 0;
        var buffer = new byte[bufferLength];
        while (offset < bufferLength)
        {
            var read = stream.Read(buffer, offset, bufferLength - offset);
            if (read == 0)
            {
                break;
            }

            offset += read;
        }
        return Encoding.UTF8.GetString(buffer, 0, bufferLength);
    }
}