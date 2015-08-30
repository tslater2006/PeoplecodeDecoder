using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class FunctionElement : Element
    {
        public String MethodName;
        List<Parameter> Params = new List<Parameter>();
        public List<Element> Body = new List<Element>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);

            sb.Append("Function ").Append(MethodName);

            sb.Append("(");

            if (Params.Count > 0)
            {
                
                foreach (Parameter p in Params)
                {
                    sb.Append(p.Name).Append(" As ").Append(p.Type).Append(", ");
                }
                sb.Length -= 2;
                
            }

            sb.Append(")");
            sb.Append("\r\n");
            foreach (Element e in Body)
            {
                sb.Append(e.ToString());
            }

            DoPadding(sb);
            sb.Append("End-Function;");
            return sb.ToString();
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the opening Function byte */
            ms.ReadByte();

            /* function name */
            var stringElement = new PureStringElement();
            stringElement.Parse(ms, state);
            MethodName = stringElement.Value;

            byte nextByte = Peek(ms);

            if (nextByte == 11)
            {
                /* function has parameters */
                /* eat open parens */
                ms.ReadByte();
                nextByte = Peek(ms);
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
                    if (nextByte == 3) /* comma */
                    {
                        ms.ReadByte();
                        nextByte = Peek(ms);
                    }
                }
                /* eat close parens */
                ms.ReadByte();
            }

            /* eat the newline */
            ms.ReadByte();

            
            while (nextByte != 55) /* end function */
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel,true);
                Body.Add(nextElement);
                nextByte = Peek(ms);
            }

            /* eat end-function */
            ms.ReadByte();

            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
