using ElectricFieldCalculation.Core;
using ElectricFieldCalculation.IO;
using SynteticData;
using System;
using System.IO;
using System.Text;

namespace ElectricFieldCalculation.Console {
    class Program {

        private static string _tsFile = "";
        private static string _zFile = "";
        private static string _outTsFile = "";
        private static string _outPowerSpectraFile = "";

        private static int _windowGenerating = 2048;
        private static int _step = 1;

        private static int _windowPowerSpectra = 2048;

        private static bool _acFilter = false;


        static void Main(string[] args) {

            System.Console.WriteLine();
            System.Console.WriteLine(@"-- Расчёт электрического поля - 1.2 - 01.04.2018 --");
            System.Console.WriteLine(@"--    Епишкин Дмитрий - dmitri_epishkin@mail.ru  --");
            System.Console.WriteLine();

            if (!ReadSettings()) {
                System.Console.WriteLine(@"Программа завершена");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Проверка входных параметров..");

            if (!CheckSettings()) {
                System.Console.WriteLine(@"Файл input.inf содержит недопустимые параметры");
                System.Console.WriteLine(@"Программа завершена");
                System.Console.ReadKey();
                return;
            }
            
            System.Console.WriteLine("Всё в порядке!");
            
            System.Console.WriteLine();

            System.Console.WriteLine("Длина окна для преобразования поля: " + _windowGenerating);
            System.Console.WriteLine("Шаг окна преобразования поля: " + _step);
            System.Console.WriteLine("Длина окна для расчёта амплитуд и спектральных мощностей: " + _windowPowerSpectra);

            System.Console.WriteLine();

            var data = new SynteticDataGenerator(
                new RealDataImporter(),
                new FtfTensorCurveImporter(),
                new ObservatoryDataExporter())

            .Generate(_tsFile, _zFile, _outTsFile, _windowGenerating, _step, _acFilter);
            
            System.Console.WriteLine();

            System.Console.WriteLine("Расчёт средней амплитуды и спектральной мощности Hx");
            var hx = PowerSpectraCalculation.Run(data.Hx, _windowPowerSpectra);

            System.Console.WriteLine("Расчёт средней амплитуды и спектральной мощности Hy");
            var hy = PowerSpectraCalculation.Run(data.Hy, _windowPowerSpectra);

            System.Console.WriteLine("Расчёт средней амплитуды и спектральной мощности Ex");
            var ex = PowerSpectraCalculation.Run(data.Ex, _windowPowerSpectra);

            System.Console.WriteLine("Расчёт средней амплитуды и спектральной мощности Ey");
            var ey = PowerSpectraCalculation.Run(data.Ey, _windowPowerSpectra);

            System.Console.WriteLine("Сохранение амплитуд и спектральных мощностей в файл");
            PowerSpectraExporter.Export(_outPowerSpectraFile, ex, ey, hx, hy);

            System.Console.WriteLine();

            System.Console.WriteLine("-- Готово --");
            System.Console.ReadKey();

        }

        static bool CheckSettings() {

            var state = true;

            if (string.IsNullOrWhiteSpace(_tsFile)) {
                System.Console.WriteLine("Не задан путь к исходным временным рядам");
                state = false;
            }
            if (string.IsNullOrWhiteSpace(_zFile)) {
                System.Console.WriteLine("Не задан путь к файлу с импедансом");
                state = false;
            }
            if (string.IsNullOrWhiteSpace(_outTsFile)) {
                System.Console.WriteLine("Не задан путь для сохранения временных рядов");
                System.Console.WriteLine("Ряды будут сохранены в файл outTs.txt");
                _outTsFile = "outTs.txt";
            }
            if (string.IsNullOrWhiteSpace(_outTsFile)) {
                System.Console.WriteLine("Не задан путь для сохранения средних амплитуд и спектральных мощностей");
                System.Console.WriteLine("Ряды будут сохранены в файл outPower.txt");
                _outTsFile = "outPower.txt";
            }
            if (!File.Exists(_tsFile)) {
                System.Console.WriteLine("Указанный файл с временными рядами не найден");
                state = false;
            }
            if (!File.Exists(_zFile)) {
                System.Console.WriteLine("Указанный файл с импедансом не найден");
                state = false;
            }

            if (!(IsValidSize(_windowGenerating))) {
                System.Console.WriteLine("Неверная длина окна скользящего окна. Будет установлено значение по умолчанию.");
                _windowGenerating = 2048;
            }
            if (!(IsValidSize(_windowPowerSpectra))) {
                System.Console.WriteLine("Неверная длина окна скользящего окна. Будет установлено значение по умолчанию.");
                System.Console.WriteLine("Должно быть >= 16 и быть степенью 2");
                _windowPowerSpectra = 2048;
            }
            if (_step < 1) {
                System.Console.WriteLine("Шаг окна должен быть больше 0. Будет установлено значение по умолчанию.");
                _step = 1;
            }
            if (_step > _windowGenerating / 2) {
                System.Console.WriteLine("Слишком большой шаг. Он будет уменьшен");
                _step = _windowGenerating / 2;
            }

            return state;
        }

        static bool IsValidSize(int val) {
            return val >= 16 && (val & (val - 1)) == 0;
        }

        static bool ReadSettings() {

            if (!File.Exists(@"input.inf")) {
                System.Console.WriteLine(@"Файл input.inf не найден");
                return false;
            }

            using (var sr = new StreamReader(@"input.inf", Encoding.Default)) {

                System.Console.WriteLine(@"Чтение настроек из файла input.inf");

                int count = 0;
                while (!sr.EndOfStream) {
                    var line = sr.ReadLine();
                    ProcessLine(line, ++count);
                }
                
            }

            return true;
        }

        static void ProcessLine(string line, int c) {
            var split = Array.ConvertAll(line.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries), x => x.Trim());
            if (split.Length < 2)
                return;                
            if (split.Length > 2) {
                System.Console.WriteLine("Ошибка! Обнаружено несколько разделителей '=' в строке №" + c +". Строка пропущена");
                return;
            }
            if (split[0] == "TimeSeriesPath") {
                _tsFile = split[1];                
            }
            else if (split[0] == "ImpedancePath") {
                _zFile = split[1];
            }
            else if (split[0] == "OutputTimeSeriesPath") {
                _outTsFile = split[1];
            }
            else if (split[0] == "OutputPowerSpectraPath") {
                _outPowerSpectraFile = split[1];
            }
            else if (split[0] == "WindowForGenerating")
                _windowGenerating = int.Parse(split[1]);
            else if (split[0] == "StepForGenerating")
                _step = int.Parse(split[1]);
            else if (split[0] == "PowerSpectraWindow")
                _windowPowerSpectra = int.Parse(split[1]);
            else if (split[0] == "AcFilter")
                _acFilter = split[1] == "Вкл";
        }

    }
}
