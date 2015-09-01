using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ForElement : Element
    {

        List<Element> Predicate = new List<Element>();
        List<Element> To = new List<Element>();
        List<Element> Step = new List<Element>();
        List<Element> Body = new List<Element>();

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);

            sb.Append("For ");

            foreach(var e in Predicate)
            {
                e.IndentLevel = 0;
                e.Write(sb);
            }

            sb.Append(" To ");

            foreach (var e in To)
            {
                e.IndentLevel = 0;
                e.Write(sb);
            }

            if (Step.Count > 0 )
            {
                sb.Append(" Step ");

                foreach (var e in Step)
                {
                    e.IndentLevel = 0;
                    e.Write(sb);
                }
            }
            sb.Append("\r\n");

            foreach(var e in Body)
            {
                e.Write(sb);
            }
            DoPadding(sb);
            sb.Append("End-For;\r\n");

        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the "For" byte */
            ms.ReadByte();

            byte nextByte = Peek(ms);

            while (nextByte != 42) /* "To" marker */
            {
                Predicate.Add(Element.GetNextElement(ms, state, IndentLevel));
                nextByte = Peek(ms);
            }

            /* eat the "To" */
            ms.ReadByte();

            while (nextByte != 43 && nextByte != 45) /* "Step" or newline marker */
            {
                To.Add(Element.GetNextElement(ms, state, IndentLevel));
                nextByte = Peek(ms);
            }

            if (nextByte == 43)
            {
                // there's a "Step";

                while (nextByte != 45)
                {
                    Step.Add(Element.GetNextElement(ms, state, IndentLevel));
                    nextByte = Peek(ms);
                }
            }

            /* eat the newline */
            nextByte = (byte)ms.ReadByte();
            nextByte = Peek(ms);
            state.AlternateBreak = 44;
            while (nextByte != 44) /* end-for */
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
                nextByte = Peek(ms);
            }

            /* eat the end-for */
            ms.ReadByte();
            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
