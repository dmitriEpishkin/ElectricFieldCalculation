using System;

namespace Nordwest.Wpf.Controls {
    public struct Interval : IEquatable<Interval> {
        private readonly double _start;
        private readonly double _end;
        private readonly double _length;

        public Interval(double start, double end) {
            System.Diagnostics.Debug.Assert(start <= end);

            _start = start;
            _end = start > end ? _start : end;
            _length = _end - _start;
        }

        public double Length {
            get { return _length; }
        }
        public double End {
            get { return _end; }
        }
        public double Start {
            get { return _start; }
        }

        public bool Equals(Interval other) {
            return _start == other.Start && _end == other.End;
        }

        public override string ToString() {
            return string.Format(@"{{{0}, {1}}}", _start, _end);
        }

        public Interval Inflate(double toStart, double toEnd) {
            return new Interval(_start - toStart, _end + toEnd);
        }

        public static bool operator ==(Interval a, Interval b) {
            return a.Equals(b);
        }

        public static bool operator !=(Interval a, Interval b) {
            return !(a == b);
        }

        // задолбали тво WARNINGи /M
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
    }
}