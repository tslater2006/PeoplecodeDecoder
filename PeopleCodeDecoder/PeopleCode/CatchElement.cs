﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class CatchElement : Element
    {
        String CatchClause;
        List<Element> Body = new List<Element>();

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            sb.Append("catch ").Append(CatchClause);
            sb.Append("\r\n");
            foreach (Element e in Body)
            {
                e.Write(sb);
            }
            
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the catch byte */
            ms.ReadByte();
            Element nextElement = null;
            state.AlternateBreak = 45;

            StringBuilder sb1 = new StringBuilder();

            var exceptionType = Element.GetNextElement(ms, state, -1,false);

            exceptionType.Write(sb1);
            while (Peek(ms) == 87)
            {
                exceptionType = Element.GetNextElement(ms, state, -1, false);
                exceptionType.Write(sb1);
                exceptionType = Element.GetNextElement(ms, state, -1, false);
                exceptionType.Write(sb1);
            }
            var variableName = Element.GetNextElement(ms, state, -1, false);

            StringBuilder sb = new StringBuilder();
            sb.Append(sb1.ToString());
            sb.Append(" ");
            variableName.Write(sb);

            CatchClause = sb.ToString();

            /* eat the newline */
            ms.ReadByte();

            state.AlternateBreak = 103;
            while (Peek(ms) != 103)
            {
                nextElement = Element.GetNextElement(ms, state, IndentLevel, true);
                Body.Add(nextElement);
            }
        }
    }
}
