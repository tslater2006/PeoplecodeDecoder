using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ClassElement : Element
    {
        String ClassName;
        List<Element> Public = new List<Element>();
        List<Element> Private = new List<Element>();
        List<Element> Protected = new List<Element>();
        List<Element> Body = new List<Element>();

        public override void Parse(MemoryStream ms, ParseState state)
        {
            Element stringElement = new PureStringElement();
            /* eat the class byte */
            ms.ReadByte();
            state.InClassDefn = true;
            stringElement.Parse(ms, state);
            ClassName = stringElement.Value;

            byte nextByte = Peek(ms);

            while (nextByte != 91 && nextByte != 97 && nextByte != 115)
            {
                Element nextElement = Element.GetNextElement(ms, state,IndentLevel);
                Public.Add(nextElement);
                nextByte = Peek(ms);
            }
            if (nextByte == 115) /* has protected section */
            {
                /* eat protected byte */
                ms.ReadByte();
                while (nextByte != 91 && nextByte != 97)
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                    Protected.Add(nextElement);
                    nextByte = Peek(ms);
                }
            }
            if (nextByte == 97) /* has private section */
            {
                /* eat private byte */
                ms.ReadByte();
                while (nextByte != 91)
                {
                    Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                    Private.Add(nextElement);
                    nextByte = Peek(ms);
                }
            }
            /* eat the end-class */
            ms.ReadByte();

            state.InClassDefn = false;

            /* eat the semicolon */
            ms.ReadByte();

            /* class defn has been parsed, now for the body */

            while (Peek(ms) != 7)
            {
                Element nextElement = Element.GetNextElement(ms, state, IndentLevel);
                Body.Add(nextElement);
            }
        }
    }
}
