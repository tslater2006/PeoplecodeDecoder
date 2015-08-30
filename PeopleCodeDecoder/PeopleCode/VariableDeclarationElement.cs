using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class VariableDeclarationElement : Element
    {
        DeclareType declareType;
        string VariableType;
        List<Element> Declaration = new List<Element>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoPadding(sb);
            switch(declareType)
            {
                case DeclareType.LOCAL:
                    sb.Append("Local ");
                    break;
                case DeclareType.GLOBAL:
                    sb.Append("Global ");
                    break;
                case DeclareType.COMPONENT:
                    sb.Append("Component ");
                    break;
                case DeclareType.INSTANCE:
                    sb.Append("instance ");
                    break;

            }

            sb.Append(VariableType).Append(" ");

            foreach(Element e in Declaration)
            {
                e.IndentLevel = 0;
                sb.Append(e.ToString());
            }
            var result = sb.ToString().TrimEnd(' ') + ";\r\n";

            return result;
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the Local byte */
            byte variableType = (byte)ms.ReadByte();

            switch(variableType)
            {
                case 68:
                    declareType = DeclareType.LOCAL;
                    break;
                case 69:
                    declareType = DeclareType.GLOBAL;
                    break;
                case 84:
                    declareType = DeclareType.COMPONENT;
                    break;
                case 98:
                    declareType = DeclareType.INSTANCE;
                    break;
            }

            byte nextByte = Peek(ms);
            Element stringElement = new PureStringElement();
            while(nextByte != 1)
            {
                stringElement.Parse(ms, state);
                VariableType += stringElement.Value + " ";
                nextByte = Peek(ms);
            }
            VariableType = VariableType.Trim();

            while (nextByte != 21) /* semicolon */
            {
                Declaration.Add(GetNextElement(ms, state, 0));
                nextByte = Peek(ms);
            }
            /* eat the semicolon */
            ms.ReadByte();
        }
    }

    enum DeclareType
    {
        LOCAL, COMPONENT, GLOBAL, INSTANCE
    }
}
