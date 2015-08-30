using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ParseState
    {
        public JArray References;
        public int TabIndent;
        public StringBuilder Output = new StringBuilder();
        public bool InClassDefn = false;
        public int AlternateBreak;
    }
}
