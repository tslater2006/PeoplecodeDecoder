using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCodeDecoder.PeopleCode
{
    public class NumberElement : Element
    {
        public override void Parse(MemoryStream ms, ParseState state)
        {
            int numBytes = 18;
            /* skip the marker */
            ms.ReadByte();
            int firstByte = ms.ReadByte();
            int decimalPlace = ms.ReadByte();

            BigInteger value = BigInteger.Zero;
            BigInteger fact = BigInteger.One;
            for (var i = 0; i < (numBytes - 4); i++)
            {
                value = BigInteger.Add(value, BigInteger.Multiply(fact, new BigInteger(ms.ReadByte())));
                fact = BigInteger.Multiply(fact, new BigInteger(256));
            }
            string number = value.ToString();

            if (decimalPlace > 0)
            {
                while (number.Length < decimalPlace)
                {
                    number = "0" + number;
                }
                number = number.Insert(number.Length - (decimalPlace), ".");
            }
            /* skip last 2 bytes */
            ms.ReadByte();
            ms.ReadByte();
            this.Value = number;
        }
    }
}
