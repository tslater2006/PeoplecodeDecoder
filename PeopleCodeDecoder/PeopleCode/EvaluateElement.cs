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

        public override void Write(StringBuilder sb)
        {
            
            DoPadding(sb);
            sb.Append("Evaluate ").Append(Variable).Append("\r\n");
            foreach(WhenElement w in Cases)
            {
                w.Write(sb);
            }
            DoPadding(sb);
            sb.Append("End-Evaluate;\r\n");
            
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the "Evaluate" byte */
            ms.ReadByte();

            StringBuilder sb = new StringBuilder();
            while (Peek(ms) != 61)
            {
                Element.GetNextElement(ms, state, -1).Write(sb);
                Variable += sb.ToString();
                sb.Length = 0;
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
