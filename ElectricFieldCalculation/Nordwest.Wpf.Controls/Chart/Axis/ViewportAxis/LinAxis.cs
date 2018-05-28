namespace Nordwest.Wpf.Controls.Chart {
    public class LinAxis : ViewportAxis {
        public override double ViewToHomogenous(double value) => value;
        public override double HomogenousToView(double value) => value;
    }
}