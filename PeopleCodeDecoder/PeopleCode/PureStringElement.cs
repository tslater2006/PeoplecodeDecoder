using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class PureStringElement : Element
    {
        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append(Value);
        }
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the marker */
            var marker = ms.ReadByte();

            MemoryStream bytesRead = new MemoryStream();
            byte[] currentChar = new byte[2];

            ms.Read(currentChar, 0, 2);
            while (currentChar[0] + currentChar[1] > 0)
            {
                bytesRead.Write(currentChar, 0, 2);
                ms.Read(currentChar, 0, 2);
            }

            Value = System.Text.Encoding.Unicode.GetString(bytesRead.ToArray());
        }
    }
}
