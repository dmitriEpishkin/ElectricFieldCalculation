using System;
using System.Numerics;
using SynteticData.FFT;
using SynteticData.Interpolation;

namespace SynteticData.Data {
    public class TensorCurve {
        private readonly int _x;
        private readonly int _y;

        private readonly CubicSpline[,] _amps;
        private readonly CubicSpline[,] _phs;

        private Complex[][,] _tensors;

        public TensorCurve(string name, double[] f, Complex[][,] vals) {

            Name = name;

            Array.Sort(f, vals);

            SourceFrequencies = f;
            SourceTensors = vals;

            _x = vals[0].GetLength(0);
            _y = vals[0].GetLength(1);

            _amps = new CubicSpline[_x, _y];
            _phs = new CubicSpline[_x, _y];

            for (int i = 0; i < _x; i++) {
                for (int j = 0; j < _y; j++) {
                    _amps[i,j] = new CubicSpline();
                    _amps[i,j].PrepareInterpolationFunction(f, Array.ConvertAll(vals, v => Math.Log10(v[i,j].Magnitude)), CubicSpline.BounadaryConditionType.SecondDerivative, 0, 0);
                    _phs[i,j] = new CubicSpline();
                    _phs[i, j].PrepareInterpolationFunction(f, Array.ConvertAll(vals, v => v[i, j].Phase), CubicSpline.BounadaryConditionType.SecondDerivative, 0, 0);
                }
            }

        }

        public Complex[][,] GetTensors(float[] f) {
            var res = new Complex[f.Length][,];
            
            for (int i = 0; i < f.Length; i++)
                res[i] = GetTensor(f[i]);
            
            return res;
        }

        public Complex[,] GetTensor(double f) {
            
            var res = new Complex[_x, _y];
            for (int i = 0; i < _x; i++) {
                for (int j = 0; j < _y; j++) {
                    res[i, j] = Complex.FromPolarCoordinates(Math.Pow(10, _amps[i, j].Interpolate(f)), _phs[i, j].Interpolate(f));
                }
            }
            return res;
        }

        public void Initalize(int window, float sampleRate) {
            _tensors = GetTensors(Fft.GetFrequencies(window, sampleRate));
        }

        public Complex[][] Apply(Complex[][] sp, int window, float sampleRate) {

            var cSp = new Complex[OutputsNumber][];

            for (int i = 0; i < cSp.Length; i++) {
                var ss = new Complex[sp[0].Length];
                for (int k = 0; k < ss.Length - 1; k++) {
                    for (int e = 0; e < sp.Length; e++)
                        ss[k + 1] += sp[e][k + 1]*_tensors[k][i, e];
                }
                for (int e = 0; e < sp.Length; e++)
                    ss[0] += new Complex(0, (sp[e][0].Imaginary*_tensors[ss.Length - 1][i, e]).Magnitude);

                ss[0] += sp[0][0].Real;

                cSp[i] = ss;
            }

            return cSp;  
        } 
        
        public string Name { get; }

        public int InputsNumber => _x;
        public int OutputsNumber => _y;

        public double[] SourceFrequencies { get; }
        public Complex[][,] SourceTensors { get; }
    }
}
