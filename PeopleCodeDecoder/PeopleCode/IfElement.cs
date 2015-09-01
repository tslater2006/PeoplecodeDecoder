using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class IfElement : Element
    {
        List<Element> Condition = new List<Element>();
        List<Element> Body = new List<Element>();
        List<Element> ElseBody;

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            var openParen = 0;
            sb.Append("If ");
            foreach (var e in Condition)
            {
                if (e.ToString() == "(")
                {
                    openParen++;
                }

                if (e.ToString() == ")")
                {
                    openParen--;
                }
                e.IndentLevel = 0;
                if (e is BooleanLogicElement)
                {
                    // handle things special for "AND"
                    e.Write(sb);

                    var type = ((BooleanLogicElement)e).Type;

                    if (type == BooleanType.AND || type == BooleanType.OR)
                    {
                        sb.Append("\r\n");
                        IndentLevel += (2 + openParen);
                        DoPadding(sb);
                        IndentLevel -= (2 + openParen);
                    }

                }
                else
                {
                    e.Write(sb);
                }
            }

            sb.Append(" Then\r\n");

            foreach (var e in Body)
            {
                e.Write(sb);
            }

            if (ElseBody != null && ElseBody.Count > 0)
            {
                DoPadding(sb);
                sb.Append("Else\r\n");

                foreach(var e in ElseBody)
                {
                    e.Write(sb);
                }
            }
            DoPadding(sb);
            sb.Append("End-If;\r\n");
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the If byte */
            ms.ReadByte();

            byte nextByte = Peek(ms);
            while (nextByte != 31) /* then */
            {
                Condition.Add(Element.GetNextElement(ms, state, IndentLevel));
                nextByte = Peek(ms);
            }

            if (nextByte == 66)
            {
                /* skip this byte */
                ms.ReadByte();
            }

            /* eat the "then" */
            ms.ReadByte();

            if (Peek(ms) == 21) /* semicolon */
            {
                ms.ReadByte();
            }

            nextByte = Peek(ms);

            while (nextByte != 25 && nextByte != 26) /* not an "else" or an "end-if" */
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                Body.Add(nextElement);
                nextByte = Peek(ms);
                if (Peek(ms) == 21)
                {
                    ms.ReadByte();
                }
            }

            if (nextByte == 25)
            {
                ElseBody = new List<Element>();
                /* eat the else */
                var elseMarker = ms.ReadByte();
                while (nextByte != 26 ) /* not an "else" or an "end-if" */
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                    ElseBody.Add(nextElement);
                    nextByte = Peek(ms);
                    if (Peek(ms) == 21)
                    {
                        ms.ReadByte();
                    }
                }

            }

            /* eat the end-if */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
