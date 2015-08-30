using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class MethodDeclarationElement : Element
    {
        public String MethodName;
        List<Parameter> Params = new List<Parameter>();
        String ReturnType;

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the method byte */
            ms.ReadByte();

            Element stringElement = new PureStringElement();
            stringElement.Parse(ms, state);
            MethodName = stringElement.Value;

            /* eat the open paren */
            ms.ReadByte();
            byte nextByte = Peek(ms);

            while (nextByte != 20) /* close paren */
            {
                stringElement.Parse(ms, state);
                var paramName = stringElement.Value;
                /* eat the "as" */
                ms.ReadByte();
                stringElement.Parse(ms, state);
                var paramType = stringElement.Value;
                Params.Add(new Parameter() { Name = paramName, Type = paramType });

                nextByte = Peek(ms);
                if (nextByte == 3)
                {
                    /* eat the comma */
                    ms.ReadByte();
                    nextByte = Peek(ms);
                }
            }

            /* eat the close paren */
            ms.ReadByte();

            if (Peek(ms) == 57)
            {
                /* has a return value */
                /* eat the "Returns" */
                ms.ReadByte();

                nextByte = Peek(ms);

                while (nextByte != 21) /* semicolon */
                {
                    stringElement.Parse(ms, state);
                    ReturnType += stringElement.Value + " ";
                    nextByte = Peek(ms);
                }
                ReturnType = ReturnType.Trim();
                
            }
            /* eat the semicolon */
            ms.ReadByte();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("method ").Append(MethodName).Append("(");
            foreach( var m in Params)
            {
                sb.Append(m.Name).Append(" as ").Append(m.Type).Append(", ");
            }
            if (Params.Count > 0)
            {
                sb.Length = sb.Length - 2;
            }
            sb.Append(")");

            if (ReturnType != null && ReturnType.Length > 0)
            {
                sb.Append(" Returns ").Append(ReturnType);
            }

            sb.Append(";");

            return sb.ToString();
        }
    }

    class Parameter
    {
        public String Name;
        public String Type;
    }
}
