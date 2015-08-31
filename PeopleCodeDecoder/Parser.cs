using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleCodeDecoder.PeopleCode;
namespace PeopleCodeDecoder
{
    public class Parser
    {
        public static PeopleCode.ProgramElement ParsePPC(string filename, string refsFile, ParseOptions opts = null)
        {
            byte[] ppcBytes = File.ReadAllBytes(filename);
            string refsText = File.ReadAllText(refsFile);

            JArray refs = JArray.Parse(refsText);
            ParseState state = new ParseState();
            if (opts != null)
            {
                state.Options = opts;
            } else
            {
                state.Options = new ParseOptions();
            }
            state.References = refs;

            MemoryStream ms = new MemoryStream(ppcBytes);

            ProgramElement p = new ProgramElement();
            p.Parse(ms, state);
            

            return p;
            
        }
    }
}
