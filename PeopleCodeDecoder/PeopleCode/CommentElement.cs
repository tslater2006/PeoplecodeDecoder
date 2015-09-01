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

        public override void Write(StringBuilder sb)
        {
            if (Type == CommentType.SLASH_STAR_SAME_LINE)
            {
                /* trim the end of sb */
                while (Char.IsWhiteSpace(sb[sb.Length -1]))
                {
                    sb.Length--;
                }
                sb.Append(" ");
            } else
            {
                DoPadding(sb);
            }

            
            sb.Append(Value).Append("\r\n");
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat comment marker */
            byte commentType = (byte) ms.ReadByte();

            if (commentType == 36)
            {
                Type = CommentType.SLASH_STAR;
            }
            else if (commentType == 78)
            {
                //TODO: make a global string writer to use instead of each element making its own string builder
                Type = CommentType.SLASH_STAR_SAME_LINE;
            }
            else if (commentType == 85)
            {
                Type = CommentType.ANGLE_STAR;
            }
            else if (commentType == 109)
            {
                Type = CommentType.SLASH_PLUS;
            }
            if (Type == CommentType.SLASH_PLUS)
            {
                ms.Seek(-1, SeekOrigin.Current);
                PureStringElement commentText = new PureStringElement();
                commentText.Parse(ms, state);
                this.Value = "/+ " + commentText.Value + " +/";
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
        SLASH_STAR, SLASH_STAR_SAME_LINE, REM, ANGLE_STAR, SLASH_PLUS
    }
}
