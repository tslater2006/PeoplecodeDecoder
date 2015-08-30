using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class QuotedStringElement : Element
    {

        public override void Parse(MemoryStream ms, ParseState state)
        {
            Element stringElement = new PureStringElement();
            stringElement.Parse(ms, state);
            Value = "\"" + stringElement.Value + "\"";
        }
    }
}
