using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class ReferenceElement : Element
    {
        byte referenceType;
        string RecordName;
        string ReferenceName;
        string PackageRoot;
        string QualifyPath;

        private static string[] refKeywords = new string[] {"Component","Panel","RecName", "Scroll", "MenuName", "BarName", "ItemName", "CompIntfc",
                "Image", "Interlink", "StyleSheet", "FileLayout", "Page", "PanelGroup", "Message", "BusProcess", "BusEvent", "BusActivity",
                "Field", "Record"};
        public override void Write(StringBuilder sb)
        {
            if (referenceType == 33)
            {
                sb.Append(RecordName + "." + ReferenceName);
            } else if (referenceType == 74)
            {
                sb.Append(ReferenceName);
            }
        }
        public override void Parse(MemoryStream ms, ParseState state)
        {
            /* eat reference marker */
            referenceType = (byte) ms.ReadByte();
            byte[] bShort = new byte[2];
            bShort[0] = (byte)ms.ReadByte();
            bShort[1] = (byte)ms.ReadByte();

            int refNum = BitConverter.ToInt16(bShort, 0);
            JObject reference = (JObject)state.References[refNum];
            RecordName = reference["RECNAME"].ToString();
            ReferenceName = reference["REFNAME"].ToString();
            PackageRoot = reference["PACKAGEROOT"].ToString();
            QualifyPath = reference["QUALIFYPATH"].ToString();

            foreach (String keyword in refKeywords)
            {
                if (RecordName.Equals(keyword.ToUpper()))
                {
                    RecordName = keyword;
                    break;
                }
            }

        }
    }
}
