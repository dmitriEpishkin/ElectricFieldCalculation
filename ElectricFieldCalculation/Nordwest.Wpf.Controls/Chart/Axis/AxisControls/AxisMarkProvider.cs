using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public abstract class AxisMarkProvider
    {
        protected double MinStep { get; set; } = 16;
        
        public abstract IEnumerable<Tuple<double, string>> GetMarks(ViewportAxis axis);

        public abstract void IterateMinorGrid(ViewportAxis axis, Action<double> gridAction);
        public abstract void IterateMajorGrid(ViewportAxis axis, Action<double> gridAction);

        protected static double ToClient(ViewportAxis axis, double value) {
            return axis.ViewToClient(value);
        }
        protected static double ToValue(ViewportAxis axis, double value) {
            return axis.ClientToView(value);
        }

        protected static Interval ToViewInterval(ViewportAxis axis) {
            var v = axis.View;
            var s = v.Start;
            var e = v.End;

            return new Interval(Math.Min(s, e), Math.Max(s, e));
        }
    }

    public class LinLogMarkProvider : AxisMarkProvider
    {
        public double LongLogEps = 0.001;

        public override IEnumerable<Tuple<double, string>> GetMarks(ViewportAxis axis)
        {
            var interval = ToViewInterval(axis);

            var next = -MathHelper.RoundLogMax(LongLogEps);// first;
            var prev = 0.0;
            while (next >= interval.Start) //обходим отрицательные значения в логарифмической части
            {
                if (ToClient(axis, prev) - MinStep >= ToClient(axis, next))
                {
                    yield return new Tuple<double, string>(ToClient(axis, next), next.ToString());
                    prev = next;
                }
                next *= 10;
            }

            if (interval.Start < 0)
                yield return new Tuple<double, string>(ToClient(axis, 0), 0.ToString()); //Добавляем ноль(линейная часть)

            if (LongLogEps > interval.Start)
                next = MathHelper.RoundLogMax(LongLogEps);
            else
                next = MathHelper.RoundLogMax(interval.Start);
            prev = 0.0;
            while (next <= interval.End) //Обходим положительные значения в логарифмической части
            {
                if (ToClient(axis, prev) + MinStep <= ToClient(axis, next))
                {
                    yield return new Tuple<double, string>(ToClient(axis, next), next.ToString());
                    prev = next;
                }
                next *= 10;
            }
        }

        public override void IterateMinorGrid(ViewportAxis axis, Action<double> minorGridAction)
        {
            var interval = ToViewInterval(axis);

            var next = -MathHelper.RoundLogMax(LongLogEps);// first;
            var prev = 0.0;
            while (next >= interval.Start) //обходим отрицательные значения в логарифмической части
            {
                if (ToClient(axis, prev) - MinStep >= ToClient(axis, next))
                {
                    
                    prev = next;
                }
                for (int i = 2; i < 10; i++)
                {
                    minorGridAction.Invoke(ToClient(axis, next * i));
                }
                next *= 10;
            }
            
            if (LongLogEps > interval.Start)
                next = MathHelper.RoundLogMax(LongLogEps);
            else
                next = MathHelper.RoundLogMax(interval.Start);
            prev = 0.0;
            while (next <= interval.End) //Обходим положительные значения в логарифмической части
            {
                for (int i = 2; i < 10; i++)
                {
                    minorGridAction.Invoke(ToClient(axis, next * i));
                }
                if (ToClient(axis, prev) + MinStep <= ToClient(axis, next))
                {
                    prev = next;
                }
                next *= 10;
            }
        }

        public override void IterateMajorGrid(ViewportAxis axis, Action<double> action)
        {
            var interval = ToViewInterval(axis);

            var next = -MathHelper.RoundLogMax(LongLogEps);// first;
            var prev = 0.0;
            while (next >= interval.Start) //обходим отрицательные значения в логарифмической части
            {
                if (ToClient(axis, prev) - MinStep >= ToClient(axis, next))
                {
                    action.Invoke(ToClient(axis, next));
                    prev = next;
                }
                next *= 10;
            }

            if (interval.Start < 0) action.Invoke(ToClient(axis, 0)); //Добавляем ноль(линейная часть)

            if (LongLogEps > interval.Start)
                next = MathHelper.RoundLogMax(LongLogEps);
            else
                next = MathHelper.RoundLogMax(interval.Start);
            prev = 0.0;
            while (next <= interval.End) //Обходим положительные значения в логарифмической части
            {
                if (ToClient(axis, prev) + MinStep <= ToClient(axis, next))
                {
                    action.Invoke(ToClient(axis, next));
                    prev = next;
                }
                next *= 10;
            }
        }
    }

    public class Log10MarkProvider : AxisMarkProvider
    {
        public override IEnumerable<Tuple<double, string>> GetMarks(ViewportAxis axis)
        {
            var interval = ToViewInterval(axis);

            var first = MathHelper.RoundLogMax(interval.Start);
            var next = first;

            if (next < double.Epsilon)
                next = double.Epsilon;

            do
            {
                var nextClient = ToClient(axis, next);

                yield return new Tuple<double, string>(nextClient, next.ToString(System.Globalization.CultureInfo.CurrentUICulture));

                next *= 10;
            } while (next <= interval.End);
        }

        public override void IterateMinorGrid(ViewportAxis axis, Action<double> minorGridAction)
        {
            var interval = ToViewInterval(axis);

            var first = MathHelper.RoundLogMax(interval.Start);
            var next = first;
            
            if (next < double.Epsilon)
                next = double.Epsilon;

            // мелкие линии до первой крупной(ибо она не учитывается)
            var minorStart = MathHelper.RoundLogMin(interval.Start);
            for (int i = 2; i < 10; i++)
                if (minorStart * i > interval.Start)
                    minorGridAction.Invoke(ToClient(axis, minorStart * i));

            do
            {
                for (int i = 2; i < 10; i++) {
                    var view = next * i;
                    if (interval.Start < view && view < interval.End)
                        minorGridAction.Invoke(ToClient(axis, view));
                }

                next *= 10;
            } while (next <= interval.End);
        }

        public override void IterateMajorGrid(ViewportAxis axis, Action<double> action)
        {
            var interval = ToViewInterval(axis);
            
            var next = MathHelper.RoundLogMax(interval.Start);

            if (next < double.Epsilon)
                next = double.Epsilon;
            
            do
            {
                var nextClient = ToClient(axis, next);

                action.Invoke(nextClient);
                
                next *= 10;
            } while (next <= interval.End);
        }
    }

    public class LinearMarkProvider : AxisMarkProvider
    {
        public LinearMarkProvider(double minMinorStep) {
            MinStep = 2;//minMinorStep;
        }
        
        public double MinMinorGridStep { get; set; } = 8;
        public double MinMajorGridStep { get; set; } = 80;

        public override IEnumerable<Tuple<double, string>> GetMarks(ViewportAxis axis)
        {
            if (axis == null)
                yield break;

            var interval = ToViewInterval(axis);
            
            var majorStep = Math.Abs(ToValue(axis, MinMajorGridStep) - ToValue(axis, 0));
            var pow = Math.Floor(Math.Log10(majorStep));
            majorStep = Round(majorStep, pow);

            var start = interval.Start;
            start = Math.Ceiling(start / majorStep) * majorStep;
            if (pow < -15)
                pow = -15;
            if (pow < 0)
                start = Math.Round(start, -(int)pow);

            var minorStep = Math.Abs(ToValue(axis, MinStep) - ToValue(axis, 0));
            if (minorStep <= double.Epsilon)
                yield break;
            
            foreach (var m in IterateMarks(axis, interval, minorStep, start))
                yield return m;

        }

        protected virtual IEnumerable<Tuple<double, string>> IterateMarks(ViewportAxis axis, Interval interval, double mstep, double start) {
            
            var pow = Math.Floor(Math.Log10(mstep)); //степень шага
            mstep = Round(mstep, pow);
            
            if (pow < -15)
                pow = -15;
            
            for (var i = start; i < interval.End; i += mstep)
            {
                var iRound = pow < 0 ? Math.Round(i, -(int)pow) : i;
                yield return new Tuple<double, string>(ToClient(axis, iRound), iRound.ToString(@"G5"));
            }

            for (var i = start - mstep; i > interval.Start; i -= mstep)
            {
                var iRound = pow < 0 ? Math.Round(i, -(int)pow) : i;
                yield return new Tuple<double, string>(ToClient(axis, iRound), iRound.ToString(@"G5"));
            }

        }

        public override void IterateMinorGrid(ViewportAxis axis, Action<double> action)
        {
            if (axis == null)
                return;

            var interval = ToViewInterval(axis);
            
            var minorStep = Math.Abs(ToValue(axis, MinMinorGridStep) - ToValue(axis, 0));
            IterateGrid(axis, interval, minorStep, action);
            
        }

        public override void IterateMajorGrid(ViewportAxis axis, Action<double> action)
        {
            if (axis == null)
                return;

            var interval = ToViewInterval(axis);
            
            var majorStep = Math.Abs(ToValue(axis, MinMajorGridStep) - ToValue(axis, 0));
            IterateGrid(axis, interval, majorStep, action);

        }

        private void IterateGrid(ViewportAxis axis, Interval interval, double mstep, Action<double> action) {
            var mstart = interval.Start;

            var pow = Math.Floor(Math.Log10(mstep)); //степень шага
            mstep = Round(mstep, pow);
            
            mstart = Math.Ceiling(mstart / mstep) * mstep;
            for (var i = mstart; i <= interval.End; i += mstep)
                action.Invoke(ToClient(axis, i));
        }

        protected virtual double Round(double mstep, double pow) {
            if (mstep <= 2 * Math.Pow(10, pow))
                //если модельный шаг в диапазоне (1; 2] * 10^"степень шага" - округляем до шага 2
                mstep = 2 * Math.Pow(10, pow);
            else if (mstep <= 5 * Math.Pow(10, pow))
                //если в диапазоне от (2, 5] * 10^"степень шага" - округляем с шагом 5
                mstep = 5 * Math.Pow(10, pow);
            else //если (5, 10] * 10^"степень шага" - с шагом 10
                mstep = 10 * Math.Pow(10, pow);
            return mstep;
        }

    }

    public class TimeMarkProvider : LinearMarkProvider
    {
        public TimeMarkProvider() : base(2) {
            
        }

        protected override IEnumerable<Tuple<double, string>> IterateMarks(ViewportAxis axis, Interval interval, double mstep, double start)
        {
            var pow = Math.Floor(Math.Log10(mstep)); //степень шага
            mstep = Round(mstep, pow);

            if (pow < -15)
                pow = -15;

            for (var i = start; i < interval.End; i += mstep)
            {
                var iRound = pow < 0 ? Math.Round(i, -(int)pow) : i;
                yield return new Tuple<double, string>(ToClient(axis, iRound), DateTime.FromOADate(iRound).ToString("yyyy/MM/dd HH:mm:ss"));
            }

            for (var i = start - mstep; i > interval.Start; i -= mstep)
            {
                var iRound = pow < 0 ? Math.Round(i, -(int)pow) : i;
                yield return new Tuple<double, string>(ToClient(axis, iRound), DateTime.FromOADate(iRound).ToString("yyyy/MM/dd HH:mm:ss"));
            }

        }

        protected override double Round(double mstep, double pow) {
            if (mstep <= 2 * Math.Pow(10, pow))
                //если модельный шаг в диапазоне (1; 2] * 10^"степень шага" - округляем до шага 2
                mstep = 2 * Math.Pow(10, pow);
            else if (mstep <= 5 * Math.Pow(10, pow))
                //если в диапазоне от (2, 5] * 10^"степень шага" - округляем с шагом 5
                mstep = 5 * Math.Pow(10, pow);
            else //если (5, 10] * 10^"степень шага" - с шагом 10
                mstep = 10 * Math.Pow(10, pow);
            return mstep;
        }

    }

    public class IndexedMarkProvider : AxisMarkProvider
    {
        private readonly SortedCollection<Tuple<double, string>> _indexes =
            new SortedCollection<Tuple<double, string>>(new IndexComparer());

        public SortedCollection<Tuple<double, string>> Indexes
        {
            get { return _indexes; }
        }

        public override IEnumerable<Tuple<double, string>> GetMarks(ViewportAxis axis)
        {
            var interval = ToViewInterval(axis);

            foreach (var index in _indexes)
            {
                if (interval.Start <= index.Item1 && interval.End >= index.Item1)
                    yield return new Tuple<double, string>(ToClient(axis, index.Item1), index.Item2);
            }
        }

        public override void IterateMinorGrid(ViewportAxis axis, Action<double> action) { }

        public override void IterateMajorGrid(ViewportAxis axis, Action<double> action)
        {
            var interval = ToViewInterval(axis);

            foreach (var index in _indexes)
            {
                if (interval.Start <= index.Item1 && interval.End >= index.Item1)
                    action.Invoke(ToClient(axis, index.Item1));
            }
        }

        private class IndexComparer : IComparer<Tuple<double, string>>
        {
            public int Compare(Tuple<double, string> x, Tuple<double, string> y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}