using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public abstract class Element
    {
        public String Value;
        public int IndentLevel;
        public abstract void Parse(MemoryStream ms, ParseState state);
        public abstract override string ToString();
        public static byte Peek(MemoryStream ms)
        {
            byte b = (byte)ms.ReadByte();
            ms.Seek(-1, SeekOrigin.Current);
            return b;
        }

        public void DoPadding(StringBuilder sb)
        {
            for (var x = 0; x < IndentLevel; x++)
            {
                sb.Append("   ");
            }
        }

        public static Element GetNextElement(MemoryStream ms, ParseState state, int indentationLevel, bool collapsePrimitives = false)
        {
            bool isPrimitive = false;
            byte nextByte = Peek(ms);

            /* bytes that we should just skip */
            if (nextByte == 65)
            {
                /* this is for "AND" inside an IF */
                ms.ReadByte();
                nextByte = Peek(ms);
            }
            Element nextElement;

            switch (nextByte)
            {
                case 1:
                case 10:
                case 18:
                    isPrimitive = true;
                    nextElement = new PureStringElement();
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 8:
                case 9:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 19:
                case 20:
                case 21:
                case 35:
                case 46:
                case 75:
                case 76:
                case 77:
                case 87:
                case 105:
                    isPrimitive = true;
                    nextElement = new OperatorElement();
                    break;
                case 22:
                    isPrimitive = true;
                    nextElement = new QuotedStringElement();
                    break;
                case 24:
                case 29:
                case 30:
                case 47:
                case 48:
                    isPrimitive = true;
                    nextElement = new BooleanLogicElement();
                    break;
                case 28:
                    nextElement = new IfElement();
                    break;
                case 33:
                case 74:
                    isPrimitive = true;
                    nextElement = new ReferenceElement();
                    break;
                case 36:
                case 78:
                case 85:
                case 109:
                    nextElement = new CommentElement();
                    break;
                case 37:
                    nextElement = new WhileElement();
                    break;
                case 41:
                    nextElement = new ForElement();
                    break;
                case 45:
                case 79:
                    nextElement = new NewLineElement();
                    break;
                case 50:
                    nextElement = new FunctionElement();
                    break;
                case 56:
                    isPrimitive = true;
                    nextElement = new ReturnElement();
                    break;
                case 60:
                    nextElement = new EvaluateElement();
                    break;
                case 66:
                    nextElement = new PureStringElement();
                    ms.ReadByte();
                    nextElement.Value = "";
                    return nextElement;
                case 68:
                case 69:
                case 84:
                case 86:
                case 98:
                    nextElement = new VariableDeclarationElement();
                    break;
                case 80:
                    isPrimitive = true;
                    nextElement = new NumberElement();
                    break;
                case 88:
                    nextElement = new ImportElement();
                    break;
                case 90:
                    nextElement = new ClassElement();
                    break;
                case 94:
                    nextElement = new PropertyElement();
                    break;
                case 95:
                    nextElement = new GetterElement();
                    break;
                case 73:
                    nextElement = new SetterElement();
                    break;
                case 99:
                    if (state.InClassDefn)
                    {
                        nextElement = new MethodDeclarationElement();
                    } else
                    {
                        nextElement = new MethodElement();
                        //nextElement = new MethodElement();
                    }

                    break;
                case 101:
                    nextElement = new TryElement();
                    break;
                default:
                    return null;
            }
            nextElement.IndentLevel = indentationLevel + 1;
            nextElement.Parse(ms, state);

            if (isPrimitive && collapsePrimitives)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(nextElement.ToString());
                /* we found a "primitive" (non-control struct), lets collapse until we see a semicolon */
                var tempElement = GetNextElement(ms, state, indentationLevel, false);
                while (tempElement.Value == null || tempElement.Value.Equals(";") == false || tempElement is NewLineElement)
                {
                    sb.Append(tempElement.ToString());
                    if (Peek(ms) == state.AlternateBreak)
                    {
                        break;
                    }
                    tempElement = GetNextElement(ms, state, -1, false);
                    
                    
                }
                string finalLine = sb.ToString().Trim() + ";\r\n";

                nextElement = new PureStringElement();
                nextElement.IndentLevel = indentationLevel + 1;
                nextElement.Value = finalLine;
            }

            return nextElement;
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return Value;
            }
            return base.ToString();
        }
    }

    public enum ElementSpacing
    {
        NONE, BEFORE, AFTER, BOTH
    }
}
