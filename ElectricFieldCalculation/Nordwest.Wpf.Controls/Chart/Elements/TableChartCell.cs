using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls.Chart
{
    public class TableChartCell : IComparable<TableChartCell>, INotifyPropertyChanged
    {
        private double _start;
        private double _end;

        public TableChartCell(double start, double end, int backIntColor) : this(start, end, null, backIntColor) { }
        public TableChartCell(double start, double end, object tag, int backIntColor)
        {
            if (start > end) throw new ArgumentException(Resources.TableChartCell_StartGreaterThenEnd_Exception);
            _start = start;
            _end = end;
            _backIntColor = backIntColor;
            Tag = tag;
        }
        public TableChartCell(double start, double end, Color backColor) : this(start, end, null, backColor) { }
        public TableChartCell(double start, double end, object tag, Color backColor)
        {
            if (start > end) throw new ArgumentException(Resources.TableChartCell_StartGreaterThenEnd_Exception);
            _start = start;
            _end = end;
            BackColor = backColor;
            Tag = tag;
        }

        public object Tag { get; set; }

        public double Start
        {
            get { return _start; }
            set
            {
                if (value > _end) throw new ArgumentException(Resources.TableChartCell_StartGreaterThenEnd_Exception);
                _start = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Start"));
            }
        }

        public double End
        {
            get { return _end; }
            set
            {
                if (_start > value) throw new ArgumentException(Resources.TableChartCell_StartGreaterThenEnd_Exception);
                _end = value;
                OnPropertyChanged(new PropertyChangedEventArgs("End"));
            }
        }

        private int _backIntColor; //для увеличения производительности, writaeableBitmapEx все равно в инты конвертит при отрисовки
        public Color BackColor
        {
            get
            {
                var intBytes = BitConverter.GetBytes(_backIntColor);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes);

                return Color.FromArgb(intBytes[0], intBytes[1], intBytes[2], intBytes[3]);
            }
            set { _backIntColor = Helpers.ConvertColor(value); OnPropertyChanged(new PropertyChangedEventArgs("BackColor")); }
        }
        public int BackIntColor { get { return _backIntColor; } set { _backIntColor = value; OnPropertyChanged(new PropertyChangedEventArgs("BackIntColor")); } }

        public int CompareTo(TableChartCell other)
        {
            return _start.CompareTo(other._start);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }

    public class TableChartCellCollection : INotifyCollectionChanged, IEnumerable<TableChartCell>
    {
        private readonly List<TableChartCell> _list = new List<TableChartCell>();

        public void Add(TableChartCell cell)
        {
            var index = _list.BinarySearch(cell);
            if (index < 0)// xor
                index = ~index;
            //если встречаются несколько одинаковых элементов - добавляем вконец(иначе будут баггги;)
            while (index < _list.Count && _list[index].CompareTo(cell) == 0)
                index++;
            _list.Insert(index, cell);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, cell));
        }
        public void Remove(TableChartCell cell)
        {
            _list.Remove(cell);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, cell));
        }
        public void Clear()
        {
            _list.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public int Count { get { return _list.Count; } }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }

        public IEnumerator<TableChartCell> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TableChartCell this[int i]
        {
            get { return _list[i]; }
            set
            {
                var old = _list[i];
                _list[i] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, old));
            }
        }
    }
}