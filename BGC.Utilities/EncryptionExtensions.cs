using CodeShield;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class EncryptionExtensions
    {
        private static string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("W3cHpHqAGgB0");
        private static readonly byte[] IVKey = Encoding.ASCII.GetBytes("azdCnywWjjCkjxAA");
        private static readonly int AesKeySize = 256;

        /// <summary>
        /// Convert a byte array
        /// </summary>
        /// <param name="original">Byte array</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this byte[] original)
        {
            StringBuilder sb = new StringBuilder();
            BitStream stream = new BitStream(original);         // Set up the BitStream
            byte[] read = new byte[1];                          // Only read 6-bit at a time
            while (true)
            {
                read[0] = 0;
                int length = stream.Read(read, 0, 6);           // Try to read 6 bits
                if (length == 6)                                // Not reaching the end
                {
                    if ((int)(read[0] >> 3) == 0x1f)            // First 5-bit is 11111
                    {
                        sb.Append(Base62CodingSpace[61]);
                        stream.Seek(-1, SeekOrigin.Current);    // Leave the 6th bit to next group
                    }
                    else if ((int)(read[0] >> 3) == 0x1e)       // First 5-bit is 11110
                    {
                        sb.Append(Base62CodingSpace[60]);
                        stream.Seek(-1, SeekOrigin.Current);
                    }
                    else                                        // Encode 6-bit
                    {
                        sb.Append(Base62CodingSpace[(int)(read[0] >> 2)]);
                    }
                }
                else if (length == 0)                           // Reached the end completely
                {
                    break;
                }
                else                                            // Reached the end with some bits left
                {
                    // Padding 0s to make the last bits to 6 bit
                    sb.Append(Base62CodingSpace[(int)(read[0] >> (int)(8 - length))]);
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert a Base62 string to byte array
        /// </summary>
        /// <param name="base62">Base62 string</param>
        /// <returns>Byte array</returns>
        public static byte[] FromBase62(this string base62)
        {
            // Character count
            int count = 0;

            // Set up the BitStream
            BitStream stream = new BitStream(base62.Length * 6 / 8);

            foreach (char c in base62)
            {
                // Look up coding table
                int index = Base62CodingSpace.IndexOf(c);

                // If end is reached
                if (count == base62.Length - 1)
                {
                    // Check if the ending is good
                    int mod = (int)(stream.Position % 8);
                    if (mod == 0)
                        throw new InvalidDataException("an extra character was found");

                    if ((index >> (8 - mod)) > 0)
                        throw new InvalidDataException("invalid ending character was found");

                    stream.Write(new byte[] { (byte)(index << mod) }, 0, 8 - mod);
                }
                else
                {
                    // If 60 or 61 then only write 5 bits to the stream, otherwise 6 bits.
                    if (index == 60)
                    {
                        stream.Write(new byte[] { 0xf0 }, 0, 5);
                    }
                    else if (index == 61)
                    {
                        stream.Write(new byte[] { 0xf8 }, 0, 5);
                    }
                    else
                    {
                        stream.Write(new byte[] { (byte)index }, 2, 6);
                    }
                }
                count++;
            }

            // Dump out the bytes
            byte[] result = new byte[stream.Position / 8];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length * 8);
            return result;
        }

        public static Task<byte[]> EncryptAsync(this byte[] source, string password) => Task.Run(() => Encrypt(source, password));

        public static byte[] Encrypt(this byte[] source, string password)
        {
            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, IV = IVKey })
            using (var key = new Rfc2898DeriveBytes(password, Salt))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, symmetricKey.CreateEncryptor(key.GetBytes(AesKeySize / 8), IVKey), CryptoStreamMode.Write))
            {
                cryptoStream.Write(source, 0, source.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }

        public static Task<byte[]> DecryptAsync(this byte[] source, string password) => Task.Run(() => Decrypt(source, password));

        public static byte[] Decrypt(this byte[] source, string password)
        {
            Shield.ArgumentNotNull(source, nameof(source));
            Shield.ArgumentNotNull(password, nameof(password));

            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            using (var key = new Rfc2898DeriveBytes(password, Salt))
            using (var memoryStream = new MemoryStream(source))
            using (var cryptoStream = new CryptoStream(memoryStream, symmetricKey.CreateDecryptor(key.GetBytes(AesKeySize / 8), IVKey), CryptoStreamMode.Read))
            {
                byte[] decryptedBytes = new byte[source.Length];
                int decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                return decryptedByteCount == decryptedBytes.Length
                    ? decryptedBytes
                    : decryptedBytes.Take(decryptedByteCount).ToArray(); // source bytes have had padding; skip it
            }
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}
