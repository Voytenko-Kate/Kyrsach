using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tovar
{
    public static class Randomizer
    {
        private static Random rnd = new Random();
        private static ulong RandomUlong()
        {
            byte[] buf = new byte[sizeof(ulong)];
            rnd.NextBytes(buf);

            return BitConverter.ToUInt64(buf, 0);
        }

        public static string RandomBarcode()
        {
            ulong code = RandomUlong();
            string codeString = code.ToString();
            string barcode = "";
            for(int i = 0; i < 13; i++)
            {
                barcode += codeString[i];
            }
            return barcode;
        }

        public static float RandomMorzha()
        {
            return 100 + (float)rnd.NextDouble() * 50;
        }

        public static string RandomVendorCode()
        {
            ulong code = RandomUlong();
            string codeString = code.ToString();
            string vendorCode = "";
            for (int i = 0; i < 6; i++)
            {
                vendorCode += codeString[i];
            }
            return vendorCode;
        }
    }
}
