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
        List<Element> Condition;
        List<Element> Body = new List<Element>();
        
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
                    Condition.Add(Element.GetNextElement(ms, state, IndentLevel,true));
                    nextByte = Peek(ms);
                }

                /* eat the newline */
                ms.ReadByte();

                nextByte = Peek(ms);

                while (nextByte != 46)
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                    Body.Add(nextElement);
                }

                /* eat the break */
                ms.ReadByte();

                /* eat the semicolon */
                ms.ReadByte();
            } else if (whenType == 62)
            {
                /* need to figure out */
                Debugger.Break();
                
            }
        }
    }
}
