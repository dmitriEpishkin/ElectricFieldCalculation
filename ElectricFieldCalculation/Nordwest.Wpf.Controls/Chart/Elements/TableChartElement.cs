using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nordwest.Wpf.Controls.MiniMap;

namespace Nordwest.Wpf.Controls.Chart
{
    public class TableChartElement : ChartElement, IDrawToMiniMap
    {
        // private readonly SortedCollection<TableChartGroup> _groups = new SortedCollection<TableChartGroup>();
        private readonly TableChartGroupCollection _groups = new TableChartGroupCollection();

        private bool _drawVerticalSplit = true;
        private bool _drawHorizontalSplits = true;
        private Color _splitsColor = Colors.Navy;
        private TextRenderer _textRenderer = new TextRenderer(true);//TODO проблема с кешированием при переключении цвета
        private int _selectedGroupIndex = -1;

        public bool DrawVerticalSplit
        {
            get { return _drawVerticalSplit; }
            set { _drawVerticalSplit = value; Redraw(); }
        }
        public bool DrawHorizontalSplits
        {
            get { return _drawHorizontalSplits; }
            set { _drawHorizontalSplits = value; Redraw(); }
        }
        public Color SplitsColor
        {
            get { return _splitsColor; }
            set { _splitsColor = value; }
        }

        public TextRenderer TextRenderer
        {
            get { return _textRenderer; }
            set { _textRenderer = value; }
        }

        public int SelectedGroupIndex
        {
            get { return _selectedGroupIndex; }
            set
            {
                if (_selectedGroupIndex == value) return;
                var old = _selectedGroupIndex;
                _selectedGroupIndex = value;
                this.RedrawGroup(old);
                this.RedrawGroup(_selectedGroupIndex);
                OnSelectedGroupChanged();
            }
        }
        public TableChartGroup SelectedGroup
        {
            get
            {
                if (_selectedGroupIndex >= 0 && _selectedGroupIndex < _groups.Count)
                    return _groups[_selectedGroupIndex];
                else return null;
            }
            set
            {
                if (_groups.Contains(value)) SelectedGroupIndex = _groups.IndexOf(value);
                else SelectedGroupIndex = -1;
            }
        }

        #region Overrides of ChartElement

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {

            foreach (var g in _groups)
            {
                if (g.Start > dirtyModelRect.Right || g.End < dirtyModelRect.Left) continue;
                var x1 = writeableBitmap.CheckX(GetClientX(g.Start));
                var x2 = writeableBitmap.CheckX(GetClientX(g.End));
                foreach (var cell in g.Cells)
                {
                    if (cell.Start > dirtyModelRect.Bottom || cell.End < dirtyModelRect.Top) continue;
                    var y1 = writeableBitmap.CheckY(GetClientY(cell.Start));
                    var y2 = writeableBitmap.CheckY(GetClientY(cell.End));
                    if (x1 >= x2) continue;
                    if (y1 >= y2) continue;

                    writeableBitmap.FillRectangle(x1, y1, x2, y2, cell.BackColor);

                    if (_drawHorizontalSplits)
                    {
                        writeableBitmap.DrawLine(x1, y1, x2, y1, _splitsColor);
                        writeableBitmap.DrawLine(x1, y2, x2, y2, _splitsColor);
                    }

                    var textSize = _textRenderer.MeasureString(cell.Tag.ToString());
                    if (textSize.Item1 < x2 - x1 && textSize.Item2 < y2 - y1)
                    {
                        var tx = (x1 + x2) / 2 - textSize.Item1 / 2;
                        var ty = (y1 + y2 - textSize.Item2) / 2;
                        _textRenderer.Color = cell.BackColor.GetContrastColor();
                        writeableBitmap.DrawString(cell.Tag.ToString(), tx, ty, _textRenderer);
                    }
                }

                if (_drawVerticalSplit && g.Cells.Count != 0)
                {
                    var y1 = writeableBitmap.CheckY(GetClientY(g.Cells[0].Start));
                    var y2 = writeableBitmap.PixelHeight;
                    writeableBitmap.DrawLine(x1, y1, x1, y2, SplitsColor);
                    writeableBitmap.DrawLine(x2, y1, x2, y2, SplitsColor);
                }

                if (_selectedGroupIndex >= 0 && _selectedGroupIndex < _groups.Count &&
                    g == _groups[_selectedGroupIndex])
                {
                    var x = writeableBitmap.CheckX(GetClientX(g.Position));
                    var y = 0;
                    var y2 = writeableBitmap.PixelHeight;
                    writeableBitmap.DrawLine(x, y, x, y2, Colors.Red);
                }
            }

            if (_markData.Area != HitTestArea.None) DrawMark(writeableBitmap, _markData.GroupIndex, _markData.CellIndex, _markData.Area);
        }

        public override Rect GetModelBounds()
        {
            if (_groups.Count == 0 || _groups.All(g => g.Cells.Count == 0)) return new Rect();
            var x1 = _groups[0].Start;
            var x2 = _groups[_groups.Count - 1].End;
            var y1 = _groups.Min(g => g.Cells[0].Start);
            var y2 = _groups.Max(g => g.Cells[g.Cells.Count - 2].End) * 2;
            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                var g = _groups[i];
                if (g.Start > modelPoint.X || g.End < modelPoint.X) continue;

                for (int j = 0; j < g.Cells.Count; j++)
                {
                    var c = g.Cells[j];
                    if (c.Start > modelPoint.Y || c.End < modelPoint.Y) continue;

                    var area = GetHitArea(new Point(GetClientX(modelPoint.X), GetClientY(modelPoint.Y)), g, c, 5);
                    return new TableChartHitTestResult(true, area, i, j);
                }
                return new TableChartHitTestResult(true, GetHitArea(new Point(GetClientX(modelPoint.X), GetClientY(modelPoint.Y)), g, null, 5), i, -1);
            }
            return new ChartElementHitTestResult(false);
        }

        #endregion

        private void DrawMark(WriteableBitmap wb, int gi, int ci, HitTestArea area)
        {
            var g = _groups[gi];
            var x1 = wb.CheckX(GetClientX(g.Start));
            var x2 = wb.CheckX(GetClientX(g.End));
            var y1 = wb.CheckY(GetClientY(g.Cells[ci].Start));
            var y2 = wb.CheckY(GetClientY(g.Cells[ci].End));
            if (y1-y2 == 0 || x1-x2 == 0) return;
            if (area.HasFlag(HitTestArea.GroupStart))
                wb.FillEllipse(wb.CheckX(x1 - 2), wb.CheckY((y2 + y1) / 2 - 2), wb.CheckX(x1 + 2), wb.CheckY((y2 + y1) / 2 + 2), g.Cells[ci].BackColor.GetContrastColor());
            else if (area.HasFlag(HitTestArea.GroupEnd))
                wb.FillEllipse(wb.CheckX(x2 - 2), wb.CheckY((y2 + y1) / 2 - 2), wb.CheckX(x2 + 2), wb.CheckY((y2 + y1) / 2 + 2), g.Cells[ci].BackColor.GetContrastColor());
            else if (area.HasFlag(HitTestArea.CellStart))
                wb.FillEllipse(wb.CheckX((x1 + x2) / 2 - 2), wb.CheckY(y1 - 2),wb.CheckX( (x1 + x2) / 2 + 2),wb.CheckY( y1 + 2), g.Cells[ci].BackColor.GetContrastColor());
            else if (area.HasFlag(HitTestArea.CellEnd))
                wb.FillEllipse(wb.CheckX((x1 + x2) / 2 - 2),wb.CheckY( y2 - 2), wb.CheckX((x1 + x2) / 2 + 2),wb.CheckY( y2 + 2), g.Cells[ci].BackColor.GetContrastColor());
            else if (area.HasFlag(HitTestArea.CellBody))
                wb.DrawRectangle(wb.CheckX(x1 + 1),wb.CheckY( y1 + 1), wb.CheckX(x2), wb.CheckY(y2), g.Cells[ci].BackColor.GetContrastColor());


        }
        private struct MarkData
        {
            public MarkData(HitTestArea area, int groupIndex, int cellIndex, bool locked)
                : this()
            {
                this.Area = area;
                this.GroupIndex = groupIndex;
                this.CellIndex = cellIndex;
                IsLocked = locked;
            }

            internal bool IsLocked { get; private set; }
            internal int CellIndex { get; private set; }
            internal int GroupIndex { get; private set; }
            internal HitTestArea Area { get; private set; }
        }

        private MarkData _markData;
        public void SetMark(int gi, int ci, HitTestArea area, bool locked)
        {
            if (_markData.IsLocked) return;
            var oldMark = _markData;
            _markData = new MarkData( area, gi, ci, locked);
            RedrawGroup(oldMark.Area != HitTestArea.None ? oldMark.GroupIndex : gi);
        }
        public void UnsetMark(HitTestArea area)
        {
            if (_markData.IsLocked) return;
            var a = _markData.Area & (~area);
            _markData = new MarkData(a, _markData.GroupIndex, _markData.CellIndex, false);
            RedrawGroup(_markData.GroupIndex);
        
        }
        public void UnlockMark()
        {
            if (_markData.IsLocked)
            {
                _markData = new MarkData(_markData.Area, _markData.GroupIndex, _markData.CellIndex, false);
            }
        }

        public void DrawToMiniMap(WriteableBitmap writeableBitmap, Rect modelRect)
        {
            var cw = writeableBitmap.PixelWidth;
            var ch = writeableBitmap.PixelHeight;

            lock (_groups)
            {
                for (int i = 0; i < _groups.Count; i++)
                {
                    var g = _groups[i];

                    //foreach (var g in Groups)
                    //{
                    var x1 = writeableBitmap.CheckX(((g.Start - modelRect.X) / modelRect.Width * cw));
                    var x2 = writeableBitmap.CheckX(((g.End - modelRect.X) / modelRect.Width * cw));
                    if (x1 >= x2) continue;

                    for (int j = 0; j < g.Cells.Count; j++)
                    {
                        var cell = g.Cells[j];
                        //foreach (var cell in g.Cells)
                        //{
                        var y1 = writeableBitmap.CheckY((Viewport.VerticalAxis.ViewToHomogenous(cell.Start) -
                                                         Viewport.VerticalAxis.ViewToHomogenous(modelRect.Y)) /
                                                        (Viewport.VerticalAxis.ViewToHomogenous(modelRect.Bottom) -
                                                         Viewport.VerticalAxis.ViewToHomogenous(modelRect.Top)) * ch);
                        var y2 = writeableBitmap.CheckY((Viewport.VerticalAxis.ViewToHomogenous(cell.End) -
                                                         Viewport.VerticalAxis.ViewToHomogenous(modelRect.Y)) /
                                                        (Viewport.VerticalAxis.ViewToHomogenous(modelRect.Bottom) -
                                                         Viewport.VerticalAxis.ViewToHomogenous(modelRect.Top)) * ch);
                        if (y1 >= y2) continue;

                        writeableBitmap.FillRectangle(x1, y1, x2, y2, cell.BackColor);
                    }
                }
            }
        }

        public HitTestArea GetHitArea(Point clientPoint, TableChartGroup g, TableChartCell c, double threshold)
        {
            var area = HitTestArea.None;
            var gs = GetClientX(g.Start);
            var ge = GetClientX(g.End);

            if (gs + threshold >= clientPoint.X && gs - threshold <= clientPoint.X)
                area |= HitTestArea.GroupStart;
            else if (ge + threshold >= clientPoint.X && ge - threshold <= clientPoint.X)
                area |= HitTestArea.GroupEnd;
            else if (gs + threshold < clientPoint.X && ge - threshold > clientPoint.X)
                area |= HitTestArea.GroupBody;

            if (c == null) return area;
            var cs = GetClientY(c.Start);
            var ce = GetClientY(c.End);
            if (cs + threshold >= clientPoint.Y && cs - threshold <= clientPoint.Y)
                area |= HitTestArea.CellStart;
            else if (ce + threshold >= clientPoint.Y && ce - threshold <= clientPoint.Y)
                area |= HitTestArea.CellEnd;
            else if (cs + threshold < clientPoint.Y && ce - threshold > clientPoint.Y)
                area |= HitTestArea.CellBody;

            return area;
        }

        public Rect GetCellRectangle(int groupIndex, int cellIndex)
        {
            var cells = _groups[groupIndex].Cells;
            var y = cells[cellIndex].Start;
            var x = _groups[groupIndex].Start;
            var w = _groups[groupIndex].End - x;
            var h = cells[cellIndex].End - y;
            return new Rect(x, y, w, h);
        }

        public void RedrawGroup(int index)
        {
            if (index < 0 || index >= _groups.Count) return;
            var group = _groups[index];
            var x = group.Start;
            var w = group.End - group.Start;
            var y = Viewport.VerticalAxis.ModelStart;
            var h = Viewport.VerticalAxis.ModelEnd - y;
            Redraw(new Rect(x, y, w, h));
        }

        public void RequestModelUpdate(List<int> changedCells, TableChartGroup group)
        {
            OnDataChanged(new TableChartEventArgs(changedCells, group));
        }

        public event EventHandler<TableChartEventArgs> DataChanged;

        public void OnDataChanged(TableChartEventArgs e)
        {
            EventHandler<TableChartEventArgs> handler = DataChanged;
            if (handler != null) handler(this, e);
        }

        public event EventHandler SelectedGroupChanged;

        public void OnSelectedGroupChanged()
        {
            EventHandler handler = SelectedGroupChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public class TableChartHitTestResult : ChartElementHitTestResult
    {
        public HitTestArea Area { get; private set; }

        public int GroupIndex { get; private set; }

        public int CellIndex { get; private set; }

        public TableChartHitTestResult(bool success, HitTestArea area, int groupIndex, int cellIndex)
            : base(success)
        {
            CellIndex = cellIndex;
            GroupIndex = groupIndex;
            Area = area;
        }
    }

    [Flags]
    public enum HitTestArea
    {
        None = 0,
        GroupStart = 1,
        GroupEnd = 2,
        CellStart = 4,
        CellEnd = 8,
        GroupBody = 16,
        CellBody = 32
    }

}