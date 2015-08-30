using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class PropertyElement : Element
    {
        private string VariableType;
        private List<Element> Declaration = new List<Element>();
        private bool HasGet;
        private bool HasSet;

        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the Property byte */
            byte variableType = (byte)ms.ReadByte();

            byte nextByte = Peek(ms);
            Element stringElement = new PureStringElement();

            while (nextByte != 10)
            {
                stringElement.Parse(ms, state);
                VariableType += stringElement.Value + " ";
                nextByte = Peek(ms);
            }
            VariableType = VariableType.Trim();

            while (nextByte != 21) /* semicolon */
            {
                if (nextByte == 95)
                {
                    HasGet = true;
                    ms.ReadByte();
                }

                else if (nextByte == 73)
                {
                    HasSet = true;
                    ms.ReadByte();
                }
                else
                {
                    Declaration.Add(GetNextElement(ms, state, 0));
                }

                nextByte = Peek(ms);
            }
            /* eat the semicolon */
            ms.ReadByte();
        }
    }
}
