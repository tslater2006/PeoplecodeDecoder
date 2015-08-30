using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class NewLineElement : Element
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            DoPadding(sb);
            sb.Append("\r\n");
            return sb.ToString();
        }
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the new line byte */
            ms.ReadByte();
        }
    }
}
