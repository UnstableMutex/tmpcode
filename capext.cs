using System;
using System.Windows;

namespace CAP
{
    static class Ext
    {
        public static bool IsEmpty(this Thickness thickness)
        {
            const double delta = 0.003;
            return Math.Abs(thickness.Bottom - 0) < delta
                   & Math.Abs(thickness.Top - 0) < delta
                   & Math.Abs(thickness.Left - 0) < delta
                   & Math.Abs(thickness.Right - 0) < delta;
        }
       public static bool IsOdd(this int i)
        {
            return i % 2 == 1;
        }
        public static  bool IsEven(this int i)
        {
            return i % 2 == 0;
        }
    }
}
