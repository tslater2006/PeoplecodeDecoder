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
        string VariableType = "";
        List<Element> Declaration = new List<Element>();

        public override void Write(StringBuilder sb)
        {
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
                case DeclareType.CONSTANT:
                    sb.Append("Constant ");
                    break;
            }

            sb.Append(VariableType).Append(" ");

            foreach(Element e in Declaration)
            {
                e.IndentLevel = 0;
                e.Write(sb);
            }
            sb.Append(";\r\n");
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
                case 86:
                    declareType = DeclareType.CONSTANT;
                    break;
                case 98:
                    declareType = DeclareType.INSTANCE;
                    break;
            }

            byte nextByte = Peek(ms);
            Element stringElement = new PureStringElement();
            StringBuilder sb = new StringBuilder();
            if (declareType != DeclareType.CONSTANT)
            {
                while (nextByte != 1)
                {
                    var e = Element.GetNextElement(ms, state, -1);

                    if (e.Value == ":" && sb[sb.Length - 1] == ' ')
                    {
                        sb.Length--;
                    }

                    e.Write(sb);
                    if (e.Value != ":" && sb[sb.Length-1] != ':')
                    {
                        sb.Append(" ");
                    }

                    nextByte = Peek(ms);
                }

                VariableType = sb.ToString().Trim();
            }
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
        LOCAL, COMPONENT, GLOBAL, INSTANCE, CONSTANT
    }
}
