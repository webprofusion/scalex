using System;
using System.Collections.Generic;
using System.Linq;

namespace Webprofusion.Scalex.Util
{
    public class ColorValue
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public ColorValue()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 255;
        }

        public ColorValue(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        private string intToHex(int intval)
        {
            return intval.ToString("X");
        }

        public string AsRGBAString()
        {
            return "rgba(" + R + "," + G + "," + B + "," + A + ")";
        }
    }
}