
using System;
using System.Collections.Specialized;
using System.Linq;
using ElectricFieldCalculation.Core.Data;
using ElectricFieldCalculation.IO;
using SynteticData;
using SynteticData.Data;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Forms;
using ElectricFieldCalculation.Core;
using SynteticData.TimeSeries;
using Nordwest.Wpf.Controls;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Panel = System.Windows.Controls.Panel;

namespace ElectricFieldCalculation {
    public class ViewModel : Model {

        private readonly SynteticDataGenerator _engine;

        private int _step = 1;
        private int _window = 4096;
        private bool _acFilter = true;
        
        private TensorCurve _z;

        private string _gicFileName;
        private TimeSeriesDouble _gic;

        private bool _separateCharts;
       
        public ViewModel() {

            DataRepository = new DataRepository();
            
            _engine = new SynteticDataGenerator(
                new RealDataImporter(),
                new FtfTensorCurveImporter(),
                new ObservatoryDataExporter());

            LoadDataCommand = new DelegateCommand(Load);
            LoadImpedanceCommand = new DelegateCommand(LoadImpedance);
            LoadGicCommand = new DelegateCommand(LoadGic);
            SaveDataCommand = new DelegateCommand(SaveData, CanSaveData);
            ExportCommand = new DelegateCommand(ExportImage);

            Status = new CalculationInfoModel(Validation, Calculate);

            Status.RaiseValidation();
            SaveDataCommand.RaiseCanExecuteChanged();

            DataRepository.All.CollectionChanged += All_CollectionChanged;
        }

        private void All_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Gic == null || e.Action != NotifyCollectionChangedAction.Add)
                return;

            foreach (var d in e.NewItems.Cast<SiteData>()) {
                d.Gic = Gic;
            }
        }

        private Tuple<bool,string> Validation() {
            if (Status.IsCalculating)
                return new Tuple<bool, string>(false, "Идут вычисления");
            if (DataRepository.SelectedSites.Any(d => d.Hx == null || d.Hy == null))
                return new Tuple<bool, string>(false, "Отсутствуют Hx и Hy");
            if (Z == null)
                return new Tuple<bool, string>(false, "Отсутствует импеданс");
            return new Tuple<bool, string>(true, "Всё готово для нового расчёта");
        }

        private void Calculate(Action<double> progress) {
            progress(0);

            var counter = 0;
            var sel = DataRepository.SelectedSites;
            foreach (var data in sel) {

                Application.Current.Dispatcher.Invoke(new Action(() =>
                    data.Z = Z));

                var res = _engine.Generate(data, Window, Step, AcFilter, p => Status.Progress = (100 * counter + p) / (double)sel.Count);

                Application.Current.Dispatcher.Invoke(() => {
                    data.Ex = res[0];
                    data.Ey = res[1];
                });

                counter++;
            }

            progress(100);
        }

        private void Load(object obj) {
            if (obj is string && obj != null) {
                try {
                    var ts = new Col2Importer().Import((string) obj);

                    foreach (var d in ts.Where(t => t.Key.Component == FieldComponent.Hx && !ts.Any(b => b.Key.Name == t.Key.Name && b.Key.Component == FieldComponent.Dx)).ToArray())
                        ts.Add(new ChannelInfo(d.Key.Name, FieldComponent.Dx), TsUtil.GetDerivate(d.Value));
                    foreach (var d in ts.Where(t => t.Key.Component == FieldComponent.Hy && !ts.Any(b => b.Key.Name == t.Key.Name && b.Key.Component == FieldComponent.Dy)).ToArray())
                        ts.Add(new ChannelInfo(d.Key.Name, FieldComponent.Dy), TsUtil.GetDerivate(d.Value));

                    DataRepository.SetData((string) obj, ts);
                    Status.RaiseValidation();
                    SaveDataCommand.RaiseCanExecuteChanged();
                }
                catch (Exception e) {
                    MessageBox.Show(@"При чтении временных рядов произошла ошибка. " + e.Message);
                }
            }
            else {
                OpenFileDialog d = new OpenFileDialog();
                var df = d.ShowDialog();
                if (df.HasValue && df.Value) {
                    Load(d.FileName);
                }
            }
        }

        private void LoadImpedance(object obj) {
            if (obj is string && obj != null) {
                try {
                    var ts = new FtfTensorCurveImporter().Import((string)obj);
                    Z = ts;
                }
                catch (Exception e) {
                    MessageBox.Show(@"При чтении файла с импедансом произошла ошибка. " + e.Message);
                }
            }
            else {
                OpenFileDialog d = new OpenFileDialog();
                var df = d.ShowDialog();
                if (df.HasValue && df.Value) {
                    LoadImpedance(d.FileName);
                }
            }
        }

        private void LoadGic(object obj) {
            if (obj is string && obj != null)
            {
                try {
                    GicFileName = (string) obj;
                    var ts = new GicImporter().Import((string)obj);
                    Gic = ts.First().Value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(@"При чтении файла с GIC произошла ошибка. " + e.Message);
                }
            }
            else
            {
                OpenFileDialog d = new OpenFileDialog();
                var df = d.ShowDialog();
                if (df.HasValue && df.Value)
                {
                    LoadGic(d.FileName);
                }
            }
        }

        private bool CanSaveData(object obj) {
            return DataRepository.All.Count > 0;
        }

        private void SaveData(object obj) {
            var s = obj as string;
            if (s != null)
            {
                try
                {
                    new Col2Exporter().Export(s, DataRepository.GetDictionary());
                }
                catch (Exception e)
                {
                    MessageBox.Show(@"При сохранении данных произошла ошибка. " + e.Message);
                }
            }
            else
            {
                SaveFileDialog d = new SaveFileDialog();

                var df = d.ShowDialog();
                if (df == DialogResult.OK)
                {
                    SaveData(d.FileName);
                }
            }
        }

        private void ExportImage(object obj) {
            var s = obj as string;
            if (s != null) {
                try {
                    RenderVisualObjectHelper.ConvertToJpeg(ChartsPanel, s, 96);
                }
                catch (Exception e) {
                    MessageBox.Show(@"При экспорте данных произошла ошибка. " + e.Message);
                }
            }
            else {
                SaveFileDialog d = new SaveFileDialog();
                d.AddExtension = true;
                d.DefaultExt = "jpg";

                var df = d.ShowDialog();
                if (df == DialogResult.OK) {
                    ExportImage(d.FileName);
                }
            }
           
        }
        
        public Panel ChartsPanel { get; set; }

        public DataRepository DataRepository { get; }

        public TensorCurve Z {
            get { return _z; }
            private set {
                if (_z != value) {
                    _z = value;
                    RaisePropertyChanged(nameof(Z));
                    Status.RaiseValidation();
                }
            }
        }

        public string GicFileName {
            get { return _gicFileName; }
            set {
                if (_gicFileName != value) {
                    _gicFileName = value;
                    RaisePropertyChanged(nameof(GicFileName));
                }
            }
        }

        public TimeSeriesDouble Gic {
            get { return _gic; }
            set {
                if (_gic != value) {
                    _gic = value;
                    RaisePropertyChanged(nameof(Gic));
                    foreach (var siteData in DataRepository.All) {
                        siteData.Gic = _gic;
                    }
                }
            }
        }

        public int Step {
            get { return _step; }
            set {
                if (_step != value) {
                    _step = value;
                    RaisePropertyChanged(nameof(Step));
                }
            }
        }

        public int Window {
            get { return _window; }
            set {
                if (_window != value) {
                    _window = value;
                    RaisePropertyChanged(nameof(Window));
                }
            }
        }

        public bool AcFilter {
            get { return _acFilter; }
            set {
                if (_acFilter != value) {
                    _acFilter = value;
                    RaisePropertyChanged(nameof(AcFilter));
                }
            }
        }
        
        public int[] AvailableWindows { get; } = { 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144};

        public DelegateCommand LoadDataCommand { get; set; }
        public DelegateCommand LoadImpedanceCommand { get; set; }
        public DelegateCommand LoadGicCommand { get; set; }
        public DelegateCommand SaveDataCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }
        
        public CalculationInfoModel Status { get; }

        public bool SeparateCharts {
            get { return _separateCharts; }
            set {
                if (_separateCharts != value) {
                    _separateCharts = value;
                    RaisePropertyChanged(nameof(SeparateCharts));
                }
            }
        }


    }
}
