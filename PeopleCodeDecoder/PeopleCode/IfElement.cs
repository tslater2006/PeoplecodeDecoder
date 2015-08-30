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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);

            sb.Append("If ");
            foreach (var e in Condition)
            {
                e.IndentLevel = 0;
                if (e is BooleanLogicElement)
                {
                    // handle things special for "AND"
                    sb.Append(e.ToString());

                    var type = ((BooleanLogicElement)e).Type;

                    if (type == BooleanType.AND || type == BooleanType.OR)
                    {
                        sb.Append("\r\n");
                        IndentLevel += 2;
                        DoPadding(sb);
                        IndentLevel -= 2;
                    }

                }
                else
                {
                    sb.Append(e.ToString());
                }
            }

            sb.Append(" Then\r\n");

            foreach (var e in Body)
            {
                sb.Append(e.ToString());
            }

            if (ElseBody != null && ElseBody.Count > 0)
            {
                DoPadding(sb);
                sb.Append("Else\r\n");

                foreach(var e in ElseBody)
                {
                    sb.Append(e.ToString());
                }
            }
            DoPadding(sb);
            sb.Append("End-If;\r\n");
            return sb.ToString();
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

            nextByte = Peek(ms);

            while (nextByte != 25 && nextByte != 26) /* not an "else" or an "end-if" */
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                Body.Add(nextElement);
                nextByte = Peek(ms);
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
                }
            }

            /* eat the end-if */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
