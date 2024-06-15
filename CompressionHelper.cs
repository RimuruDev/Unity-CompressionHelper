// **************************************************************** //
//
//   Copyright (c) RimuruDev. All rights reserved.
//   Contact me: 
//          - Gmail:    rimuru.dev@gmail.com
//          - GitHub:   https://github.com/RimuruDev
//          - LinkedIn: https://www.linkedin.com/in/rimuru/
//
// **************************************************************** //

using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using UnityEngine;

namespace AbyssMoth.Internal.Codebase
{
    [HelpURL("https://github.com/RimuruDev/Unity-CompressionHelper")]
    public static class CompressionHelper
    {
        public enum CompressionType
        {
            Gzip = 0,
            Brotli = 1,
        }

        public static byte[] CompressGZip(string json)
        {
            const int WriteOffset = 0;

            var jsonBytes = Encoding.UTF8.GetBytes(json);

            using var memoryStream = new MemoryStream();

            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(jsonBytes, WriteOffset, jsonBytes.Length);
            }

            return memoryStream.ToArray();
        }

        public static byte[] CompressBrotli(string json)
        {
            const int WriteOffset = 0;

            var jsonBytes = Encoding.UTF8.GetBytes(json);

            using var memoryStream = new MemoryStream();

            using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress))
            {
                brotliStream.Write(jsonBytes, WriteOffset, jsonBytes.Length);
            }

            return memoryStream.ToArray();
        }

        public static string DecompressGZip(byte[] compressedJson)
        {
            using var memoryStream = new MemoryStream(compressedJson);
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzipStream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        public static string DecompressBrotli(byte[] compressedJson)
        {
            using var memoryStream = new MemoryStream(compressedJson);
            using var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress);
            using var reader = new StreamReader(brotliStream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        public static string CompressJson(string json, CompressionType compressionType)
        {
            var compressedJson = compressionType switch
            {
                CompressionType.Gzip => CompressGZip(json),
                CompressionType.Brotli => CompressBrotli(json),
                _ => throw new ArgumentException("Unsupported compression type")
            };

            return Convert.ToBase64String(compressedJson);
        }

        public static string DecompressJson(string compressedJsonBase64, CompressionType compressionType)
        {
            var compressedJson = Convert.FromBase64String(compressedJsonBase64);

            var decompressedJson = compressionType switch
            {
                CompressionType.Gzip => DecompressGZip(compressedJson),
                CompressionType.Brotli => DecompressBrotli(compressedJson),
                _ => throw new ArgumentException("Unsupported compression type")
            };

            return decompressedJson;
        }
    }
}
