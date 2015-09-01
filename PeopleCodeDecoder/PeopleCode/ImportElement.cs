using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ImportElement : Element
    {
        public string Import;

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append("import " + Import + ";\r\n");

        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat import byte */
            ms.ReadByte();

            while (Peek(ms) != 21) /* semicolon */
            {
                Import += Element.GetNextElement(ms, state, 0).Value;
            }
            /* eat semicolon */
            ms.ReadByte();

        }
    }
}
