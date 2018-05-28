
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Waf.Foundation;
using ElectricFieldCalculation.Core.Data;
using SynteticData.Data;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.Core
{
    public class DataRepository : Model {

        private string _name;

        private SiteData _selectedData;

        public DataRepository() {
            SelectedSites.CollectionChanged += SelectedSites_CollectionChanged;
        }

        private void SelectedSites_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var i in e.NewItems.Cast<SiteData>())
                        i.PropertyChanged += _selectedData_PropertyChanged;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var i in e.OldItems.Cast<SiteData>())
                        i.PropertyChanged -= _selectedData_PropertyChanged;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (var i in All) {
                        i.PropertyChanged -= _selectedData_PropertyChanged;
                    }
                    if (e.NewItems != null)
                        foreach (var i in e.NewItems.Cast<SiteData>())
                            i.PropertyChanged += _selectedData_PropertyChanged;
                    break;
                default:
                    break;
            }
        }

        public void SetData(string name, Dictionary<ChannelInfo, TimeSeriesDouble> data) {

            Name = name;

            All.Clear();
            SelectedSites.Clear();

            foreach (var d in data) {
                var e = All.FirstOrDefault(a => a.Name == d.Key.Name);
                if (e == null) {
                    e = new SiteData {Name = d.Key.Name};
                    All.Add(e);
                }
                switch (d.Key.Component) {
                    case FieldComponent.Unckown:
                        throw new Exception("неизвестная компонента");
                    case FieldComponent.Gic:
                        e.Gic = d.Value;
                        break;
                    case FieldComponent.Ex:
                        e.Ex = d.Value;
                        break;
                    case FieldComponent.Ey:
                        e.Ey = d.Value;
                        break;
                    case FieldComponent.Hx:
                        e.Hx = d.Value;
                        break;
                    case FieldComponent.Hy:
                        e.Hy = d.Value;
                        break;
                    case FieldComponent.Hz:
                        e.Hz = d.Value;
                        break;
                    case FieldComponent.Dx:
                        e.Dhx = d.Value;
                        break;
                    case FieldComponent.Dy:
                        e.Dhy = d.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public Dictionary<ChannelInfo, TimeSeriesDouble> GetDictionary() {
            var res = new Dictionary<ChannelInfo, TimeSeriesDouble>();

            foreach (var d in All) {
                
                if (d.Ex != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ex), d.Ex);
                if (d.Ey != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ey), d.Ey);
                if (d.Hx != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hx), d.Hx);
                if (d.Hy != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hy), d.Hy);
                if (d.Hz != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hz), d.Hz);
            }

            return res;
        }

        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public ObservableCollection<SiteData> All { get; } = new ObservableCollection<SiteData>();

        //public SiteData SelectedData {
        //    get { return _selectedData; }
        //    set {
        //        if (_selectedData != value) {

        //            if (_selectedData != null)
        //                _selectedData.PropertyChanged -= _selectedData_PropertyChanged;

        //            _selectedData = value;

        //            if (_selectedData != null)
        //                _selectedData.PropertyChanged += _selectedData_PropertyChanged;

        //            RaisePropertyChanged(nameof(SelectedData));
        //        }
        //    }
        //}

        public ObservableCollection<SiteData> SelectedSites { get; } = new ObservableCollection<SiteData>();

        private void _selectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SelectedData." + e.PropertyName);
        }
    }

}
