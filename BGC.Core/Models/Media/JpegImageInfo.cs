using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class JpegImageInfo : VisualMediaInfo
    {
        private struct _JFIFHeader
        {
            private static readonly byte[] ExpectedIdentifier = Encoding.ASCII.GetBytes("JFIF\0");

            public static _JFIFHeader FromByteArray(byte[] data)
            {
                _JFIFHeader result = new _JFIFHeader();

                result.SOI        = data.Skip(0x00).Take(0x02).ToArray();
                result.app0       = data.Skip(0x02).Take(0x02).ToArray();
                result.length     = data.Skip(0x04).Take(0x02).ToArray();
                result.identifier = data.Skip(0x06).Take(0x05).ToArray();
                result.version    = data.Skip(0x0B).Take(0x02).ToArray();
                result.units      = data.Skip(0x0E).Take(0x01).First();
                result.xdensity   = data.Skip(0x0F).Take(0x02).ToArray();
                result.ydensity   = data.Skip(0x11).Take(0x02).ToArray();
                result.xThumbnail = data.Skip(0x13).Take(0x01).First();
                result.yThumbnail = data.Skip(0x14).Take(0x01).First();

                return result;
            }

            byte[] SOI;          /* 00h  Start of Image Marker     */
            byte[] app0;         /* 02h  Application Use Marker    */
            byte[] length;       /* 04h  Length of APP0 Field      */
            byte[] identifier;   /* 06h  "JFIF" (zero terminated) Id String */
            byte[] version;      /* 0Bh  JFIF Format Revision      */
            byte units;          /* 0Eh  Units used for Resolution */
            byte[] xdensity;     /* 0Fh  Horizontal Resolution     */
            byte[] ydensity;     /* 11h  Vertical Resolution       */
            byte xThumbnail;     /* 13h  Horizontal Pixel Count    */
            byte yThumbnail;     /* 14h  Vertical Pixel Count      */

            public int XResolution => (((int)xdensity[0]) << sizeof(byte) | xdensity[1]);

            public int YResolution => (ydensity[0] << sizeof(byte) | ydensity[1]);

            public int App0Length => (length[0] << sizeof(byte) | length[1]);

            public double Version => version[0] + version[1] / (10 * Math.Log10(version[1]));

            public override string ToString()
            {
                return
                    string.Format("{0:X2}{1:X2} ", SOI[0], SOI[1]) +
                    string.Format("{0:X2}{1:X2} ", app0[0], app0[1]) +
                    string.Format("{0:X2} ", App0Length) +
                    string.Format("{0} ", Encoding.ASCII.GetString(identifier)) +
                    string.Format("{0}", XResolution) +
                    string.Format("{0}", YResolution);
            }
        };

        private static readonly ContentType JpegContentType = new ContentType(MediaTypeNames.Image.Jpeg);

        public override ContentType MimeType => JpegContentType;

        public override bool ValidateHeader()
        {
            return true;
        }

        public JpegImageInfo(Stream content)
        {
            byte[] headerData = new byte[21];
            content.Read(headerData, 0, headerData.Length);
            _JFIFHeader header = _JFIFHeader.FromByteArray(headerData);
            Width = header.XResolution;
            Height = header.YResolution;
        }
    }
}
