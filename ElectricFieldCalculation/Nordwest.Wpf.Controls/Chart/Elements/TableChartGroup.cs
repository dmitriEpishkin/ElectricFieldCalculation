using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls.Chart
{
    public class TableChartGroup : IComparable<TableChartGroup>, INotifyPropertyChanged
    {
        private double _start;
        private double _end;
        private double _position;
        public object Tag { get; set; }

        public double Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Position"));
            }
        }
        public double Start
        {
            get { return _start; }
            set
            {
                //if (value > _end) throw new ArgumentException("Start > End");
                _start = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Start"));
            }
        }
        public double End
        {
            get { return _end; }
            set
            {
                //if (_start > value) throw new ArgumentException("Start > End");//TODO отключено, иначе гемор
                _end = value;
                OnPropertyChanged(new PropertyChangedEventArgs("End"));
            }
        }

        //public readonly SortedCollection<TableChartCell> Cells = new SortedCollection<TableChartCell>();
        public readonly TableChartCellCollection Cells = new TableChartCellCollection();

        public TableChartGroup(double position, double start, double end)
        {
            if (start > position) throw new ArgumentException(Resources.TableChartGroup_StartGreaterThenPosition_Exception);
            if (position > end) throw new ArgumentException(Resources.TableChartGroup_PositionGraterThenEnd_Exception);
            _position = position;
            _start = start;
            _end = end;
        }

        public int GetCellIndex(double value)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].Start < value && Cells[i].End >= value) return i;
            }
            return -1;
        }

        public int CompareTo(TableChartGroup other)
        {
            return _position.CompareTo(other._position);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }

    public class TableChartGroupCollection : INotifyCollectionChanged, IEnumerable<TableChartGroup>
    {
        private readonly List<TableChartGroup> _list = new List<TableChartGroup>();

        public void Add(TableChartGroup group)
        {
            var index = _list.BinarySearch(group);
            if (index < 0)// xor
                index = ~index;
            _list.Insert(index, group);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, group));
        }
        public void Remove(TableChartGroup group)
        {
            _list.Remove(group);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, group));
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

        public IEnumerator<TableChartGroup> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TableChartGroup this[int i]
        {
            get { return _list[i]; }
            set
            {
                var old = _list[i];
                _list[i] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, old));
            }
        }

        public int IndexOf(TableChartGroup group)
        {
            return _list.IndexOf(group);
        }
    }


}