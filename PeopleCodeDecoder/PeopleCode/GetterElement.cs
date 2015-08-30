using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class GetterElement : Element
    {
        public string PropertyName;
        List<Element> Body = new List<Element>();
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the Get byte */
            ms.ReadByte();

            /* eat the space */
            ms.ReadByte();

            PropertyName = Element.GetNextElement(ms, state, 0).Value;

            while (Peek(ms) != 106)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
            }

            /* eat the end get */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();

        }
    }
}
