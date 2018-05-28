using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace Nordwest.Wpf.Controls.Chart
{
    public abstract class Tool
    {
        private ChartCanvas _chartCanvas;
        public ChartCanvas ChartCanvas
        {
            get { return _chartCanvas; }
            set
            {
                if (_chartCanvas != null && _isEnabled) RemoveHandlers(_chartCanvas);
                if (_chartCanvas != value) _chartCanvas = value;
                if (_chartCanvas != null && _isEnabled) AddHandlers(_chartCanvas);
            }
        }

        public Cursor Cursor { get; protected set; }

        private bool _isEnabled = true;
        private IEnumerable<KeyValuePair<Type, Tool>> _monopoleDisabledTools;

        protected Tool()
        {
            Cursor = null;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                if (_chartCanvas == null) return;
                if (_isEnabled)
                    AddHandlers(_chartCanvas);
                else
                    RemoveHandlers(_chartCanvas);
            }
        }

        //public ChartElement ChartElement { get; set; }

        protected abstract void AddHandlers(ChartCanvas chartCanvas);

        protected abstract void RemoveHandlers(ChartCanvas chartCanvas);

        protected void SetMonopole()
        {
            _monopoleDisabledTools = _chartCanvas.Tools.Where(t => t.Value.IsEnabled).ToList();
            foreach (var tool in _monopoleDisabledTools.Where(t => t.Value != this))
                tool.Value.IsEnabled = false;
        }

        protected void ResetMonopole()
        {
            if (_monopoleDisabledTools == null) return;
            foreach (var tool in _monopoleDisabledTools)
                tool.Value.IsEnabled = true;
            _monopoleDisabledTools = null;
        }
    }
}