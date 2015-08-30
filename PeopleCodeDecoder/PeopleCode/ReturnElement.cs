using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ReturnElement : Element
    {
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat the return byte */
            ms.ReadByte();
        }

        public override string ToString()
        {
            return "Return ";
        }
    }
}
