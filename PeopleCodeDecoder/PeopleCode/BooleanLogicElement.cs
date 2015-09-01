using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class BooleanLogicElement : Element
    {
        public BooleanType Type;

        public override void Write(StringBuilder sb)
        {
            DoPadding(sb);
            switch(Type)
            {
                case BooleanType.AND:
                    sb.Append(" And");
                    break;
                case BooleanType.NOT:
                    if (sb[sb.Length -1] == ' ')
                    {
                        sb.Append("Not ");
                    } else
                    {
                        sb.Append(" Not ");
                    }
                    break;
                case BooleanType.OR:
                    sb.Append(" Or");
                    break;
                case BooleanType.TRUE:
                    sb.Append("True");
                    break;
                case BooleanType.FALSE:
                    sb.Append("False");
                    break;
            }
        }

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* read the byte */
            byte type = (byte) ms.ReadByte();
            switch(type)
            {
                case 24:
                    Type = BooleanType.AND;
                    break;
                case 29:
                    Type = BooleanType.NOT;
                    break;
                case 30:
                    Type = BooleanType.OR;
                    break;
                case 47:
                    Type = BooleanType.TRUE;
                    break;
                case 48:
                    Type = BooleanType.FALSE;
                    break;

            }
        }
    }

    public enum BooleanType
    {
        AND, OR, NOT, TRUE, FALSE
    }
}
