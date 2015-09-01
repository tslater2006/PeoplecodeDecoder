using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class WhenElement : Element
    {
        bool WhenOther = false;
        List<Element> Condition;
        List<Element> Body = new List<Element>();

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            if (WhenOther)
            {
                sb.Append("When-Other\r\n");
            }
            else
            {
                sb.Append("When ");
                foreach (Element e in Condition)
                {
                    e.IndentLevel = 0;
                    e.Write(sb);
                }

                while (Char.IsWhiteSpace(sb[sb.Length -1]))
                {
                    sb.Length--;
                }
                if (sb[sb.Length-1] == ';')
                {
                    sb.Length--;
                }
                
                sb.Append("\r\n");
            }

            foreach(Element e in Body)
            {
                e.Write(sb);
            }
            if (WhenOther == false || (WhenOther == true && Body.Count == 0))
            {
                IndentLevel++;
                DoPadding(sb);
                sb.Append("Break;\r\n");
                IndentLevel--;
            }
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            var whenType = ms.ReadByte();

            if (whenType == 61)
            {
                /* will have a criteria */
                Condition = new List<Element>();
                byte nextByte = Peek(ms);
                while (nextByte != 45) /* new line */
                {
                    state.AlternateBreak = 45;
                    Condition.Add(Element.GetNextElement(ms, state, 0,true));
                    nextByte = Peek(ms);
                }

                /* eat the newline */
                ms.ReadByte();

                nextByte = Peek(ms);

                while (nextByte != 46)
                {
                    state.AlternateBreak = 46;
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                    Body.Add(nextElement);
                    nextByte = Peek(ms);
                    if (Peek(ms) == 21)
                    {
                        ms.ReadByte();
                    }
                }

                /* eat the break */
                ms.ReadByte();

                /* eat the semicolon */
                ms.ReadByte();
            } else if (whenType == 62)
            {
                WhenOther = true;

                /* eat the when-other */

                while (Peek(ms) != 46 && Peek(ms) != 63)
                {
                    state.AlternateBreak = 46;
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                    Body.Add(nextElement);
                }
                if (Peek(ms) == 46)
                {
                    /* eat the break */
                    ms.ReadByte();

                    /* eat the semicolon */
                    ms.ReadByte();
                }
                
            }
        }
    }
}
