
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
                        e.Gic.Ts = d.Value;
                        break;
                    case FieldComponent.Ex:
                        e.Ex.Ts = d.Value;
                        break;
                    case FieldComponent.Ey:
                        e.Ey.Ts = d.Value;
                        break;
                    case FieldComponent.Hx:
                        e.Hx.Ts = d.Value;
                        break;
                    case FieldComponent.Hy:
                        e.Hy.Ts = d.Value;
                        break;
                    case FieldComponent.Hz:
                        e.Hz.Ts = d.Value;
                        break;
                    case FieldComponent.Dx:
                        e.Dhx.Ts = d.Value;
                        break;
                    case FieldComponent.Dy:
                        e.Dhy.Ts = d.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public Dictionary<ChannelInfo, TimeSeriesDouble> GetDictionary() {
            var res = new Dictionary<ChannelInfo, TimeSeriesDouble>();

            foreach (var d in All) {
                
                if (d.Ex.Ts != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ex), d.Ex.Ts);
                if (d.Ey.Ts != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ey), d.Ey.Ts);
                if (d.Hx.Ts != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hx), d.Hx.Ts);
                if (d.Hy.Ts != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hy), d.Hy.Ts);
                if (d.Hz.Ts != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hz), d.Hz.Ts);
            }

            return res;
        }

        public Dictionary<ChannelInfo, PowerSpectra> GetSpectraDictionary() {
            var res = new Dictionary<ChannelInfo, PowerSpectra>();

            foreach (var d in All) {

                if (d.Ex.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ex), d.Ex.Spectra);
                if (d.Ey.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Ey), d.Ey.Spectra);
                if (d.Hx.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hx), d.Hx.Spectra);
                if (d.Hy.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hy), d.Hy.Spectra);
                if (d.Hz.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Hz), d.Hz.Spectra);
                if (d.Gic.Spectra != null)
                    res.Add(new ChannelInfo(d.Name, FieldComponent.Gic), d.Gic.Spectra);
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
        
        public ObservableCollection<SiteData> SelectedSites { get; } = new ObservableCollection<SiteData>();

        private void _selectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SelectedData." + e.PropertyName);
        }
    }

}
