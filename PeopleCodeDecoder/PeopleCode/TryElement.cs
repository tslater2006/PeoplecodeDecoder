using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class TryElement : Element
    {
        List<Element> Body = new List<Element>();
        List<CatchElement> Catches = new List<CatchElement>();

        public override void Write(StringBuilder sb)
        {
            //TODO: Implement
            //throw new NotImplementedException();
            DoPadding(sb);
            sb.Append("try\r\n");

            foreach(Element e in Body)
            {
                e.Write(sb);
            }
            
            foreach(CatchElement c in Catches)
            {
                c.Write(sb);
            }
            DoPadding(sb);
            sb.Append("end-try;\r\n");
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the try byte */
            ms.ReadByte();
            state.AlternateBreak = 102;
            while (Peek(ms) != 102)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
            }

            while (Peek(ms) != 103)
            {
                CatchElement catchElem = new CatchElement();
                catchElem.IndentLevel = IndentLevel;
                catchElem.Parse(ms, state);
                Catches.Add(catchElem);
            }

            /* eat the end try */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
