using System;
using System.Collections.Generic;

namespace ElectricFieldCalculation.Core
{
    public static class Statistic
    {

        public static double Average(double[] d, int i1, int iN)
        {
            var count = 0;
            var sum = 0.0;
            for (int i = i1; i < iN; i++)
            {
                if (i >= 0 && i < d.Length)
                {
                    sum += d[i];
                    count++;
                }
            }
            return sum / count;
        }

        public static double Average(IList<double> array)
        {
            return Average(array, 0, array.Count);
        }

        public static double Average(IList<double> array, int start, int count)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (array.Count == 0)
                throw new ArgumentException();

            double sum = 0;
            for (int i = start; i < start + count; i++)
                sum += array[i];

            return sum / count;
        }

        public static double RmsDeviation(double[] data)
        {
            double M = Average(data);
            double disp = 0;
            for (int i = 0; i < data.Length; i++)
            {
                var d = data[i] - M;
                disp += d * d;
            }
            return Math.Sqrt(disp / data.Length);
        }

    }
}
