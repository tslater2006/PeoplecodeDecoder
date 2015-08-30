using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class WhileElement : Element
    {
        List<Element> Condition = new List<Element>();
        List<Element> Body = new List<Element>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            DoPadding(sb);

            sb.Append("While ");

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
            sb.Append("\r\n");

            foreach(var e in Body)
            {
                sb.Append(e.ToString());
            }

            DoPadding(sb);
            sb.Append("End-While;\r\n");

            return sb.ToString();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the While byte */
            ms.ReadByte();

            byte nextByte = Peek(ms);
            while (nextByte != 45) /* new line */
            {
                Condition.Add(Element.GetNextElement(ms, state, IndentLevel));
                nextByte = Peek(ms);
            }

            /* eat the new line */
            ms.ReadByte();

            nextByte = Peek(ms);
            state.AlternateBreak = 38;
            while (nextByte != 38) /* end-while */
            {
                Body.Add(Element.GetNextElement(ms, state, IndentLevel,true));
                nextByte = Peek(ms);
            }

            /* eat the end while */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
