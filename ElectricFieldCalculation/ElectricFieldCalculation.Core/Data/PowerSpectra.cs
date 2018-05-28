
namespace ElectricFieldCalculation.Core.Data {
    public class PowerSpectra {
        private float[] _f;
        private double[] _mag;
        private double[] _mag2;

        public PowerSpectra(float[] f, double[] mag, double[] mag2) {
            _f = f;
            _mag = mag;
            _mag2 = mag2;
        }

        public float[] F { get { return _f; } }
        public double[] Mag {  get { return _mag; } }
        public double[] Mag2 { get { return _mag2; } }
    }
}
