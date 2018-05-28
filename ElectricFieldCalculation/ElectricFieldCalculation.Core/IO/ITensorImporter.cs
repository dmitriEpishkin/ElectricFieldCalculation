using SynteticData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldCalculation.Core.IO {
    public interface ITensorImporter {
        TensorCurve Import(string fileName);
    }
}
