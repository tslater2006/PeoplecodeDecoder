using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ProgramElement : Element
    {
        public List<Element> Statements = new List<Element>();

        public override void Write(StringBuilder sb)
        {

            foreach (Element e in Statements)
            {
                e.Write(sb);
            }
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            IndentLevel = -1;
            /* skip the header */
            byte[] header = new byte[37];
            ms.Read(header, 0, 37);

            byte nextByte = Peek(ms);
            while (nextByte != 7)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                Statements.Add(nextElement);
                nextByte = Peek(ms);
            }
        }
    }
}
