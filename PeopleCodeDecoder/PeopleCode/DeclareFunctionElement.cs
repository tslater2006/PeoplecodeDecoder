using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class DeclareFunctionElement : Element
    {

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the declare byte */
            ms.ReadByte();

            StringBuilder sb = new StringBuilder();
            sb.Append("Declare Function ");
            ms.Position += 1;

            Element.GetNextElement(ms, state, -1).Write(sb);

            sb.Append(" ");

            /* eat the "peoplecode" byte */
            ms.ReadByte();

            sb.Append("PeopleCode ");

            Element.GetNextElement(ms, state, -1).Write(sb);

            sb.Append(" ");

            Element.GetNextElement(ms, state, -1).Write(sb);

            sb.Append(";\r\n");

            Value = sb.ToString();

            /* eat 2 bytes */
            ms.Position += 2;
        }

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append(Value);
        }
    }
}
