﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ReturnElement : Element
    {
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the return byte */
            ms.ReadByte();

            Value = "";

            while (Peek(ms) != 21)
            {
                var elem = Element.GetNextElement(ms, state, -1);
                Value += elem.ToString();
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);
            sb.Append("Return ").Append(Value).Append("\r\n");
            return sb.ToString();
        }
    }
}
