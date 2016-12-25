using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BGC.Core
{
    public class SearchResult
    {
        /// <summary>
        /// An array that stores the ID of the entity or object that matched during in big-endian format.
        /// </summary>
        public byte[] Id { get; set; }

        public string Header { get; set; }

        private string _resultXml;
        public string ResultXml
        {
            get
            {
                return _resultXml;
            }

            set
            {
                _resultXml = value;
                _parsedResultXml = null;
            }
        }

        private XmlDocument _parsedResultXml;
        public XmlDocument ParsedResultXml
        {
            get
            {
                if (_parsedResultXml == null && ResultXml != null)
                {
                    _parsedResultXml = new XmlDocument();
                    _parsedResultXml.LoadXml(ResultXml);
                }

                return _parsedResultXml;
            }

            set
            {
                _parsedResultXml = value;
                _resultXml = _parsedResultXml?.OuterXml;
            }
        }

        public SearchResult(byte[] id)
        {
            Shield.ArgumentNotNull(id);
            Id = id;
        }

        public SearchResult()
        {
        }

        /// <summary>
        /// Creates a new <see cref="SearchResult"/> instance and sets its <see cref="Id"/> to the bytes of the given integer.
        /// </summary>
        /// <param name="id"></param>
        public SearchResult(int id)
        {
            Id = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i++)
            {
                Id[i] = (byte)((id >> (i * 8)) & 0xFF);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SearchResult"/> instance and sets its <see cref="Id"/> to the bytes of the given 64-bit integer.
        /// </summary>
        /// <param name="id"></param>
        public SearchResult(long id)
        {
            Id = new byte[sizeof(long)];
            for (int i = 0; i < sizeof(long); i++)
            {
                Id[i] = (byte)((id >> (i * 8)) & 0xFF);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SearchResult"/> instance and sets its <see cref="Id"/> to the bytes of the given <see cref="Guid"/>.
        /// </summary>
        /// <param name="id"></param>
        public SearchResult(Guid id)
        {
            Id = id.ToByteArray();
        }

        public int IdAsInt()
        {
            Shield.ValueNotNull(Id, nameof(Id)).GetValueOrThrow();
            Shield.AssertOperation(Id, Id.Length == sizeof(int), $"{nameof(Id)} must be exactly {sizeof(int)} bytes long.").ThrowOnError();

            int result = Id[0] | (Id[1] << 8) | (Id[2] << 16) | (Id[3] << 24);
            return result;
        }

        public long IdAsLong()
        {
            Shield.ValueNotNull(Id, nameof(Id)).GetValueOrThrow();
            Shield.AssertOperation(Id, Id.Length == sizeof(long), $"{nameof(Id)} must be exactly {sizeof(long)} bytes long.").ThrowOnError();

            long result =
                 (Id[0]) |
                ((long)(Id[1]) << 8) |
                ((long)(Id[2]) << 16) |
                ((long)(Id[3]) << 24) |
                ((long)(Id[4]) << 32) |
                ((long)(Id[5]) << 40) |
                ((long)(Id[6]) << 48) |
                ((long)(Id[7]) << 56);
            return result;
        }

        public Guid IdAsGuid()
        {
            Shield.ValueNotNull(Id, nameof(Id)).GetValueOrThrow();
            Shield.AssertOperation(Id, Id.Length == 16, $"{nameof(Id)} must be exactly {16} bytes long.").ThrowOnError();
            
            return new Guid(Id);
        }

        public override string ToString() => Header;
    }
}
