using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class FileExtensions
    {
        public static Stream ToStream(this byte[] input)
        {
            using var stream = new MemoryStream(input);
            return stream;
        }

        public static async Task<byte[]> ToByte(this Stream stream)
        {
            try
            {
                var length = stream.Length > int.MaxValue ? int.MaxValue : Convert.ToInt32(stream.Length);
                var buffer = new byte[length];
                await stream.ReadAsync(buffer, 0, length);
                return buffer;
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
