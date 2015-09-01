using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ReturnElement : Element
    {
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the return byte */
            ms.ReadByte();

            Value = "";
            StringBuilder sb = new StringBuilder();
            while (Peek(ms) != 21 && Peek(ms) != 26 && Peek(ms) != 25 && Peek(ms) != 100)
            {
                var elem = Element.GetNextElement(ms, state, -1);
                elem.Write(sb);
                Value += sb.ToString();
                sb.Length = 0;
            }

        }

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append("Return ").Append(Value).Append("\r\n");
        }
    }
}
