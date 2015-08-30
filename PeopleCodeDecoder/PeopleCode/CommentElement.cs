using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class CommentElement : Element
    {
        CommentType Type;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var x = 0; x < IndentLevel; x++)
            {
                sb.Append("   ");
            }
            sb.Append(Value).Append("\r\n");

            return sb.ToString();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat comment marker */
            byte commentType = (byte) ms.ReadByte();

            if (commentType == 36)
            {
                Type = CommentType.SLASH_STAR;
            } else if (commentType == 85)
            {
                Type = CommentType.ANGLE_STAR;
            } else if (commentType == 109)
            {
                Type = CommentType.SLASH_PLUS;
            }
            if (Type == CommentType.SLASH_PLUS)
            {
                ms.Seek(-1, SeekOrigin.Current);
                PureStringElement commentText = new PureStringElement();
                commentText.Parse(ms, state);
                this.Value = commentText.Value;
            } else
            {
                byte[] bShort = new byte[2];
                bShort[0] = (byte)ms.ReadByte();
                bShort[1] = (byte)ms.ReadByte();

                int commentLength = BitConverter.ToInt16(bShort, 0);

                byte[] commentData = new byte[commentLength];
                ms.Read(commentData, 0, commentLength);

                string s = Encoding.Unicode.GetString(commentData);
                s = s.Replace("\r\n", "\n"); // CRLF to LF
                s = s.Replace("\n", "\r\n"); // LF to CRLF
                this.Value = s;
            }
            
        }
    }

    enum CommentType
    {
        SLASH_STAR, REM, ANGLE_STAR, SLASH_PLUS
    }
}
