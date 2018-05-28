using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public class StepLineChartElement : ChartElement
    {
        private ObservableCollection<Point> _points = new ObservableCollection<Point>();
        private int _intColor = Helpers.ConvertColor(Colors.Red);

        public Color IntColor
        {
            get
            {
                var intBytes = BitConverter.GetBytes(_intColor);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes);

                return Color.FromArgb(intBytes[0], intBytes[1], intBytes[2], intBytes[3]);
            }
            set { _intColor = Helpers.ConvertColor(value); }
        }

        public ObservableCollection<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public StepLineChartElement()
        {
            _points.CollectionChanged += _points_CollectionChanged;
        }

        void _points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnNeedRedraw(GetModelBounds());
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            if (Points.Count == 0 || writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;
            var prevX = _points[0].X;//writeableBitmap.CheckX(GetClientX(_points[0].X));
            var prevY = 0.0;// writeableBitmap.CheckY(GetClientY(_points[0].Y));
            var prevModelY = 0.0;
            for (int i = 0; i < Points.Count; i++)
            {
                var x = Points[i].X; //writeableBitmap.CheckX(GetClientX(Points[i].X));
                var y = Points[i].Y + prevModelY;//writeableBitmap.CheckY(GetClientY(Points[i].Y + prevModelY)) ;
                prevModelY = Points[i].Y + prevModelY;
                //y = writeableBitmap.CheckY(y);
                if (double.IsInfinity(y)) y = dirtyModelRect.Bottom;
                var line1 = dirtyModelRect.CohenSutherlandLineClip(new Point(x, y), new Point(x, prevY));
                var line2 = dirtyModelRect.CohenSutherlandLineClip(new Point(x, prevY), new Point(prevX, prevY));
                if (line1 != null)
                {
                    var x1 = writeableBitmap.CheckX(GetClientX(line1[0].X));
                    var y1 = writeableBitmap.CheckY(GetClientY(line1[0].Y));
                    var x2 = writeableBitmap.CheckX(GetClientX(line1[1].X));
                    var y2 = writeableBitmap.CheckY(GetClientY(line1[1].Y));
                    writeableBitmap.DrawLineAa(x1, y1, x2, y2, IntColor);
                }
                if (line2 != null)
                {
                    var x1 = writeableBitmap.CheckX(GetClientX(line2[0].X));
                    var y1 = writeableBitmap.CheckY(GetClientY(line2[0].Y));
                    var x2 = writeableBitmap.CheckX(GetClientX(line2[1].X));
                    var y2 = writeableBitmap.CheckY(GetClientY(line2[1].Y));
                    writeableBitmap.DrawLineAa(x1, y1, x2, y2, IntColor);
                }
                //writeableBitmap.DrawLineAa(x, prevY, x, y, IntColor);
                //writeableBitmap.DrawLineAa(x, prevY, prevX, prevY, IntColor);
                if (_markedPoint == i && _markedArea!= StepLineArea.None)
                    DrawMark(writeableBitmap,x,y,prevX,prevY , _markedArea);
                prevX = x;
                prevY = y;
            }
            
        }

        private void DrawMark(WriteableBitmap writeableBitmap, double cx, double cy, double px, double py, StepLineArea area)
        {
            int x;
            int y;
            switch (area)
            {
                case StepLineArea.None:
                    return;
                case StepLineArea.Horizontal:
                    x = (writeableBitmap.CheckX(GetClientX(px)) + writeableBitmap.CheckX(GetClientX(cx))) / 2;
                    y = writeableBitmap.CheckY(GetClientY(py));
                    break;
                case StepLineArea.PointPrev:
                    x = writeableBitmap.CheckX(GetClientX(cx));
                    y = writeableBitmap.CheckY(GetClientY(py));
                    break;
                case StepLineArea.Vertical:
                    x = writeableBitmap.CheckX(GetClientX(cx));
                    y = (writeableBitmap.CheckY(GetClientY(py)) + writeableBitmap.CheckY(GetClientY(cy))) / 2;
                    break;
                case StepLineArea.PointCurrent:
                    x = writeableBitmap.CheckX(GetClientX(cx));
                    y = writeableBitmap.CheckY(GetClientY(cy));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("area");
            }
            if (x < 3 || x > writeableBitmap.PixelWidth - 3 || y < 3 || y > writeableBitmap.PixelHeight - 3) return;
            writeableBitmap.FillEllipseCentered(x, y, 3, 3, _intColor);
        }

        public override Rect GetModelBounds()
        {
            var x = MathHelper.RoundLogMin(Points.Min(p => p.X));
            var y = MathHelper.RoundLogMin(Points[0].Y);
            var w = MathHelper.RoundLogMax(Points.Max(p => p.X)) - x;
            var h = MathHelper.RoundLogMax(Points.Select(p => p.Y).Where(d => !double.IsInfinity(d)).Sum()) - y;
            return new Rect(x, y, w, h);
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            var x = modelPoint.X;
            var y = modelPoint.Y;
            var px = _points[0].X;
            var py = 0.0;
            var prevModelY = 0.0;
            for (int i = 0; i < Points.Count; i++)
            {
                var cx = Points[i].X;
                var cy = Points[i].Y + prevModelY;
                prevModelY = Points[i].Y + prevModelY;

                var x2 = (GetClientX(cx));
                var y2 = (GetClientY(cy));
                var x1 = (GetClientX(px));
                var y1 = (GetClientY(py));
                if ((x > x2 - 3 && x < x2 + 3) && (y > y1 - 3 && y < y1 + 3))
                    return new StepLineHitTestResult(true,StepLineArea.PointPrev,i);
                if ((x > x2 - 3 && x < x2 + 3) && (y > y2 - 3 && y < y2 + 3))
                    return new StepLineHitTestResult(true,StepLineArea.PointCurrent,i);
                if ((x > x2 - 3 && x < x2 + 3)&&
                   (y > y1 && y < y2))
                    return new StepLineHitTestResult(true,StepLineArea.Vertical,i);
                if (((x > x1 && x < x2) || (x < x1 && x > x2)) &&
                    (y > y1 - 3 && y < y1 + 3))
                    return new StepLineHitTestResult(true,StepLineArea.Horizontal,i);
                px = cx;
                py = cy;
            }
            return new ChartElementHitTestResult(false);
        }
        private int _markedPoint;
        private StepLineArea _markedArea;
        public void SetMark(int pointIndex, StepLineArea area)
        {
            if (_markedArea!=area || _markedPoint!= pointIndex)
            _markedArea = area;
            _markedPoint = pointIndex;
            Redraw();
        }
    }

    public enum StepLineArea
    {
        None,
        Horizontal,
        PointPrev,
        Vertical,
        PointCurrent
    }

    public class StepLineHitTestResult : ChartElementHitTestResult
    {
        public StepLineArea Area { get; private set; }

        public int PointIndex { get; private set; }

        public StepLineHitTestResult(bool success, StepLineArea area, int pointIndex)
            : base(success)
        {
            Area = area;
            PointIndex = pointIndex;
        }
    }
}