using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class EvaluateElement : Element
    {
        String Variable = "";
        List<WhenElement> Cases = new List<WhenElement>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);
            sb.Append("Evaluate ").Append(Variable).Append("\r\n");
            foreach(WhenElement w in Cases)
            {
                sb.Append(w);
            }
            DoPadding(sb);
            sb.Append("End-Evaluate;\r\n");
            return sb.ToString();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the "Evaluate" byte */
            ms.ReadByte();
            

            while (Peek(ms) != 61)
            {
                
                Variable += Element.GetNextElement(ms, state, -1).ToString();
            }


            byte nextByte = Peek(ms);

            while (nextByte == 61 || nextByte == 62)
            {
                WhenElement nextElement = new WhenElement();
                nextElement.IndentLevel = this.IndentLevel;
                nextElement.Parse(ms, state);
                
                Cases.Add(nextElement);
                nextByte = Peek(ms);
            }

            /* eat the end evaluate */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
