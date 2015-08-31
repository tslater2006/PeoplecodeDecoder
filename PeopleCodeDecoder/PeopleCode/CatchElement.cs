using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class CatchElement : Element
    {
        String CatchClause;
        List<Element> Body = new List<Element>();

        public override string ToString()
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the catch byte */
            ms.ReadByte();
            Element nextElement = null;
            state.AlternateBreak = 45;

                nextElement = Element.GetNextElement(ms, state, IndentLevel, true);

            CatchClause = nextElement.Value.Substring(0, nextElement.Value.Length - 3);

            /* eat the newline */
            ms.ReadByte();

            state.AlternateBreak = 103;
            while (Peek(ms) != 103)
            {
                nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
            }
        }
    }
}
