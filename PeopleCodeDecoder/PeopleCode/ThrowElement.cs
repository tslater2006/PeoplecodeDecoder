using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ThrowElement : Element
    {
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the throw byte */
            ms.ReadByte();

            Value = "";
            //while (Peek(ms) != 21 && Peek(ms) != state.AlternateBreak)
            //{
                var elem = Element.GetNextElement(ms, state, -1,true);
                Value += elem.ToString();
            //}

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);
            sb.Append("throw ").Append(Value);
            return sb.ToString();
        }
    }
}
