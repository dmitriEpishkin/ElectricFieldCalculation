using System;
using System.Collections.Generic;
using System.Windows;

namespace Nordwest.Wpf.Controls.Chart
{
    public class ChartElementNeedRedrawEventArgs : EventArgs
    {
        private readonly Rect _dirtyModelRect;

        public ChartElementNeedRedrawEventArgs(Rect dirtyModelRect)
        {
            _dirtyModelRect = dirtyModelRect;
        }

        public Rect DirtyModelRect
        {
            get { return _dirtyModelRect; }
        }
    }

    public class TableChartEventArgs : EventArgs
    {
        private readonly List<int> _changedCells;
        private readonly TableChartGroup _group;
        public TableChartEventArgs(List<int> changedCells, TableChartGroup group)
        {
            _changedCells = changedCells;
            _group = group;
        }

        public TableChartGroup Group
        {
            get { return _group; }
        }

        public List<int> ChangedCells
        {
            get { return _changedCells; }
        }
    }
}