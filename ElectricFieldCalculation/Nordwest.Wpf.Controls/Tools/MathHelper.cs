using System;

namespace Nordwest.Wpf.Controls
{
    public static class MathHelper
    {
        public static double RoundLogMin(double value)
        {
            return Math.Pow(10, Math.Floor(Math.Log10(value)));
        }

        public static double RoundLogMax(double value)
        {
            return Math.Pow(10, Math.Ceiling(Math.Log10(value)));
        }
    }
}