namespace Nordwest.Wpf.Controls.Chart {
    public struct ViewRange {
        public ViewRange(double from, double to) {
            Start = from;
            End = to;
        }

        public double Start { get; }
        public double End { get; }

        public double Delta() => End - Start;
    }
}