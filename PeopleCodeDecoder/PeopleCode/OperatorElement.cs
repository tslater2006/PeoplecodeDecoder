using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    
    public class OperatorElement : Element
    {
        ElementSpacing Spacing;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Spacing == ElementSpacing.BEFORE || Spacing == ElementSpacing.BOTH)
            {
                sb.Append(" ");
            }
            sb.Append(Value);

            if (Spacing == ElementSpacing.AFTER || Spacing == ElementSpacing.BOTH)
            {
                sb.Append(" ");
            }

            return sb.ToString();
        }
        public override void Parse(MemoryStream ms, ParseState state)
        {
            byte b = (byte)ms.ReadByte();

            switch (b)
            {
                case 3:
                    Value = ",";
                    Spacing = ElementSpacing.AFTER;
                    break;
                case 4:
                    Value = "/";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 5:
                    Value = ".";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 6:
                    Value = "=";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 8:
                    Value = ">=";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 9:
                    Value = ">";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 11:
                    Value = "(";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 12:
                    Value = "<=";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 13:
                    Value = "<";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 14:
                    Value = "-";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 15:
                    Value = "*";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 16:
                    Value = "<>";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 19:
                    Value = "+";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 20:
                    Value = ")";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 21:
                    Value = ";";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 35:
                    Value = "|";
                    Spacing = ElementSpacing.BOTH;
                    break;
                case 46:
                    Value = "Break";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 75:
                    Value = "Null";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 76:
                    Value = "[";
                    Spacing = ElementSpacing.BEFORE;
                    break;
                case 77:
                    Value = "]";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 87:
                    Value = ":";
                    Spacing = ElementSpacing.NONE;
                    break;
                case 105:
                    Value = "create";
                    Spacing = ElementSpacing.AFTER;
                    break;
                        
            }
            
        }
    }
}
