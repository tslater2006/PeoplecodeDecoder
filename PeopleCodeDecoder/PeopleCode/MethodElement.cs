﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class MethodElement : Element
    {
        public String MethodName;
        List<Element> Body = new List<Element>();

        public override string ToString()
        {
            //TODO: Implement
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);
            sb.Append("method ").Append(MethodName.Trim()).Append("\r\n");

            foreach (Element e in Body)
            {
                sb.Append(e);
            }

            DoPadding(sb);
            sb.Append("end-method;");

            return sb.ToString();
            throw new NotImplementedException();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the method byte */
            ms.ReadByte();

            /* eat the 65 byte ? */
            ms.ReadByte();

            Element stringElement = new PureStringElement();
            stringElement.Parse(ms, state);
            MethodName = stringElement.Value;

            /* eat new line */
            Element.GetNextElement(ms, state, IndentLevel, false);

            while (Peek(ms) != 100)
            {
                state.AlternateBreak = 100;
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                Body.Add(nextElement);
            }

            /* eat the end method */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
