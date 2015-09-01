using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class SetterElement : Element
    {
        public string PropertyName;
        List<Element> Body = new List<Element>();

        public override void Write(StringBuilder sb)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the Set byte */
            ms.ReadByte();

            /* eat the space */
            ms.ReadByte();

            PropertyName = Element.GetNextElement(ms, state, 0).Value;

            while (Peek(ms) != 107)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
            }

            /* eat the end set */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();

        }
    }
}
