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
    public static class HashExtensions
    {
        private static int CombineHashCodesImpl(int a, int b)
        {
            return (a << 5) + a ^ b;
        }

        public static byte[] GetHashCode<THashAlgorithm>(this byte[] source, byte[] salt = null)
            where THashAlgorithm : HashAlgorithm, new()
        {
            Shield.ArgumentNotNull(source, nameof(source)).ThrowOnError();

            using (THashAlgorithm hash = new THashAlgorithm())
            {
                byte[] buffer;
                if (salt != null)
                {
                    buffer = new byte[source.Length + salt.Length];
                    source.CopyTo(buffer, 0);
                    salt.CopyTo(buffer, source.Length);
                }
                else
                {
                    buffer = source;
                }

                return hash.ComputeHash(buffer);
            }
        }

        public static byte[] GetHashCode<THashAlgorithm>(this object @object, byte[] salt = null)
            where THashAlgorithm : HashAlgorithm, new()
        {
            Shield.ArgumentNotNull(@object, nameof(@object));

            byte[] objAsArray = @object as byte[];
            if (objAsArray != null)
            {
                return objAsArray.GetHashCode<THashAlgorithm>(salt);
            }
            else using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, @object);
                if (salt != null)
                {
                    stream.Write(salt, 0, salt.Length);
                }
                return stream.ToArray().GetHashCode<THashAlgorithm>(salt);
            }
        }

        public static int CombineHashCodes(params object[] objects) // this is more or less copy-pasted from .NET's implementation of Tuple`3's GetHashCode
        {
            Shield.ArgumentNotNullOrEmpty(objects).ThrowOnError();
            Shield.ArgumentNotNull(objects[0], $"{nameof(objects)}[0]").ThrowOnError();

            int result = objects[0].GetHashCode();
            for (int i = 1; i < objects.Length; i++)
            {
                Shield.ArgumentNotNull(objects[i], $"{nameof(objects)}[{i}]").ThrowOnError();
                result = CombineHashCodesImpl(result, objects[i].GetHashCode());
            }

            return result;
        }
    }
}
