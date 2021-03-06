using Nordwest.Wpf.Controls.Chart.Axis.AxisControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public partial class ChartCanvas : UserControl {
        private WriteableBitmap _backBuffer;
        private readonly Viewport _viewport;
        private readonly ObservableCollection<ChartElement> _chartElements = new ObservableCollection<ChartElement>();
        private readonly ToolCollection _tools = new ToolCollection();

        private Rect? _dirtyRect;

        // tooltips
        private readonly ObservableCollection<ToolTipData> _toolTipDatas = new ObservableCollection<ToolTipData>();
        private readonly ToolTip _toolTip;

        private readonly Color _majorGridColor = Colors.DimGray;
        private readonly Color _minorGridColor = Colors.LightGray;

        
        public ChartCanvas() {
            InitializeComponent();

            _viewport = new Viewport(
                new LinAxis { Client = new ViewRange(0, 1), Model = new ViewRange(0, 100) },
                new LinAxis { Client = new ViewRange(0, 1), Model = new ViewRange(0, 100) }
            );

            Reset();

            _chartElements.CollectionChanged += _chartElements_CollectionChanged;
            Tools.CollectionChanged += _tools_CollectionChanged;
            _viewport.PropertyChanged += _viewport_PropertyChanged;
            MouseMove += ChartCanvas_MouseMove;

            // tooltips
            _toolTip = new ToolTip {
                PlacementTarget = this,
                HorizontalOffset = 5,
                VerticalOffset = 5
            };
            _toolTipDatas.CollectionChanged += _toolTipDatas_CollectionChanged;
            ToolTipService.SetPlacement(_toolTip, PlacementMode.Relative);
            ToolTipService.SetShowDuration(_toolTip, 0);
        }

        private bool _isTtFollow = true;

        public bool IsToolTipFollowsMouse {
            get { return _isTtFollow; }
            set {
                if (_isTtFollow == value)
                    return;

                _isTtFollow = value;

                if (_isTtFollow)
                    MouseMove += ChartCanvas_MouseMove;
                else
                    MouseMove -= ChartCanvas_MouseMove;
            }
        }

        private void ChartCanvas_MouseMove(object sender, MouseEventArgs e) {
            var mousePosition = e.MouseDevice.GetPosition(this);

            _toolTip.PlacementTarget = this;
            _toolTip.HorizontalOffset = mousePosition.X;
            _toolTip.VerticalOffset = mousePosition.Y + 15;
        }

        #region Event handlers

        private void _viewport_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            Refresh();
        }

        
        private void _tools_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (Tool item in e.NewItems)
                        item.ChartCanvas = this;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Tool item in e.OldItems)
                        item.ChartCanvas = null;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (Tool item in e.NewItems)
                        item.ChartCanvas = this;
                    foreach (Tool item in e.OldItems)
                        item.ChartCanvas = null;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (Tool item in e.NewItems)
                        item.ChartCanvas = this;
                    foreach (Tool item in e.OldItems)
                        item.ChartCanvas = null;
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
            Reset();
        }

        //это еще одно временное решение т_т чтобы прямо здесь и сейчас убедится что происходит корректная отписка от событий в случае ресета
        // upd: Заметим, что оно абсолютно верное и не временное
        private readonly List<ChartElement> _buckupChartElementList = new List<ChartElement>();
        // Навешиваем/снимаем эвенты на элементы.
        
        private void _chartElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (ChartElement element in e.NewItems) {
                        SetElement(element);
                        _buckupChartElementList.Add(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ChartElement element in e.OldItems) {
                        UnsetElement(element);
                        _buckupChartElementList.Remove(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (var element in _buckupChartElementList)
                        UnsetElement(element);
                    _buckupChartElementList.Clear();
                    foreach (var element in _chartElements) {
                        SetElement(element);
                        _buckupChartElementList.Add(element);
                    }
                    break;
                case NotifyCollectionChangedAction.Move: //ничего делать не надо)
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Срабатывает, если один из элементов хочет перерисовать свою область.
        private void element_NeedRedraw(object sender, ChartElementNeedRedrawEventArgs e) {
            InvalidateRect(e.DirtyModelRect);
        }

        #endregion

        protected override void OnRender(DrawingContext drawingContext) {
            var dirtyRect = _dirtyRect.GetValueOrDefault();
            if (!dirtyRect.IsEmpty) {

                using (_backBuffer.GetBitmapContext()) {
                    var h = _viewport.HorizontalAxis;
                    var v = _viewport.VerticalAxis;

                    var x1 = _backBuffer.CheckX(h.ModelToClient(dirtyRect.Left));
                    var y1 = _backBuffer.CheckY(v.ModelToClient(dirtyRect.Top));
                    var x2 = _backBuffer.CheckX(h.ModelToClient(dirtyRect.Right));
                    var y2 = _backBuffer.CheckY(v.ModelToClient(dirtyRect.Bottom));

                    if (y1 > y2)
                        Swap(ref y1, ref y2);
                    if (x1 > x2)
                        Swap(ref x1, ref x2);

                    _backBuffer.FillRectangle(x1, y1, x2, y2, Colors.Transparent);

                    if (PolarAxis)
                        DrawPolarGrid(_backBuffer);
                    else
                        DrawGridDefault(_backBuffer);

                    foreach (var element in _chartElements)
                        element.Draw(_backBuffer, dirtyRect);
                    
                    _backBuffer.DrawRectangle(x1, y1, x2, y2, Colors.Black);
                    
                }

                

                _dirtyRect = null;
            }

            base.OnRender(drawingContext);
        }

        private void DrawPolarGrid(WriteableBitmap bitmap) {

            var markProvider = new PolarAxisMarkProvider { MinStep = 20, MinMajorStep = 100 };//{ MinStep = 5, MinMajorStep = 25 };
            
            markProvider.IterateRadiusGrid(Viewport.HorizontalAxis, Viewport.VerticalAxis,
                (x0, y0, x, y) => bitmap.DrawLine((int)x0, (int)y0, (int)x, (int)y, _minorGridColor), false);            
            markProvider.IterateEllipseGrid(Viewport.HorizontalAxis, Viewport.VerticalAxis,
                (x0, y0, x, y) => bitmap.DrawEllipseCentered((int)x0, (int)y0, (int)x, (int)y, _minorGridColor), false);

            markProvider.IterateRadiusGrid(Viewport.HorizontalAxis, Viewport.VerticalAxis,
                (x0, y0, x, y) => bitmap.DrawLine((int)x0, (int)y0, (int)x, (int)y, _majorGridColor), true);
            markProvider.IterateEllipseGrid(Viewport.HorizontalAxis, Viewport.VerticalAxis,
                (x0, y0, x, y) => bitmap.DrawEllipseCentered((int)x0, (int)y0, (int)x, (int)y, _majorGridColor), true);

        }

        private void DrawGridDefault(WriteableBitmap bitmap) {
            
            LeftMarkProvider?.IterateMinorGrid(Viewport.VerticalAxis,
                v => bitmap.DrawLine(0, bitmap.CheckY(v), bitmap.PixelWidth, bitmap.CheckY(v), _minorGridColor));

            BottomMarkProvider?.IterateMinorGrid(Viewport.HorizontalAxis,
                v => bitmap.DrawLine(bitmap.CheckX(v), 0, bitmap.CheckX(v), bitmap.PixelHeight, _minorGridColor));

            if (RightMarkProvider != LeftMarkProvider)
                RightMarkProvider?.IterateMinorGrid(Viewport.VerticalAxis,
                    v => bitmap.DrawLine(0, bitmap.CheckY(v), bitmap.PixelWidth, bitmap.CheckY(v), _minorGridColor));

            if (TopMarkProvider != BottomMarkProvider)
                TopMarkProvider?.IterateMinorGrid(Viewport.HorizontalAxis,
                    v => bitmap.DrawLine(bitmap.CheckX(v), 0, bitmap.CheckX(v), bitmap.PixelHeight, _minorGridColor));



            LeftMarkProvider?.IterateMajorGrid(Viewport.VerticalAxis,
               v => bitmap.DrawLine(0, bitmap.CheckY(v), bitmap.PixelWidth, bitmap.CheckY(v), _majorGridColor));

            BottomMarkProvider?.IterateMajorGrid(Viewport.HorizontalAxis,
                v => bitmap.DrawLine(bitmap.CheckX(v), 0, bitmap.CheckX(v), bitmap.PixelHeight, _majorGridColor));

            if (RightMarkProvider != LeftMarkProvider)
                RightMarkProvider?.IterateMajorGrid(Viewport.VerticalAxis,
                    v => bitmap.DrawLine(0, bitmap.CheckY(v), bitmap.PixelWidth, bitmap.CheckY(v), _majorGridColor));

            if (TopMarkProvider != BottomMarkProvider)
                TopMarkProvider?.IterateMajorGrid(Viewport.HorizontalAxis,
                    v => bitmap.DrawLine(bitmap.CheckX(v), 0, bitmap.CheckX(v), bitmap.PixelHeight, _majorGridColor));

        }

        /// <summary>
        /// Пересоздаем битмап. Нужно при изменении размеров, например.
        /// </summary>
        public void Reset() {
            var width = Math.Max(1, (int)ActualWidth);
            var height = Math.Max(1, (int)ActualHeight);

            _backBuffer = BitmapFactory.New(width, height);

            Background = new ImageBrush(_backBuffer);

            Viewport.HorizontalAxis.Client = new ViewRange(0, width);
            Viewport.VerticalAxis.Client = new ViewRange(0, height);

            Refresh();
        }

        /// <summary>
        /// Принудительно перерисовываем все элементы
        /// </summary>
        public void Refresh() {
            var x = Viewport.HorizontalAxis.ModelStart;
            var y = Viewport.VerticalAxis.ModelStart;
            var w = Viewport.HorizontalAxis.ModelEnd - x;
            var h = Viewport.VerticalAxis.ModelEnd - y;

            InvalidateRect(new Rect(x, y, w, h));
        }

        //навешивает обработчики и устанавливаем поля
        private void SetElement(ChartElement element) {
            element.Viewport = _viewport;
            element.NeedRedraw += element_NeedRedraw;
        }

        //снимаем обработчики и сбрасываем поля
        private void UnsetElement(ChartElement element) {
            element.NeedRedraw -= element_NeedRedraw;
            element.Viewport = null; //пусть лучше эксепшн падает, ежели чего
        }

        public Rect GetModelBounds() {
            return ChartElements.Select(ce => ce.GetModelBounds()).Aggregate(Rect.Empty, Rect.Union);
        }

        /// <summary>
        /// Коллекция элементов для отрисовки
        /// </summary>
        public ObservableCollection<ChartElement> ChartElements {
            get { return _chartElements; }
        }

        /// <summary>
        /// Определяет масштабирование и систему координат свзяанных с ним элементов
        /// </summary>
        public Viewport Viewport {
            get { return _viewport; }
        }

        public AxisMarkProvider LeftMarkProvider { get; set; }
        public AxisMarkProvider RightMarkProvider { get; set; }
        public AxisMarkProvider TopMarkProvider { get; set; }
        public AxisMarkProvider BottomMarkProvider { get; set; }

        public bool PolarAxis { get; set; }

        /// <summary>
        /// Коллекция инструментов взаимодействия с пользователем
        /// </summary>
        public ToolCollection Tools {
            get { return _tools; }
        }

        
        protected override void OnQueryCursor(QueryCursorEventArgs e) {
            var tool = Tools.Values.LastOrDefault(v => v.IsEnabled && v.Cursor != null);
            if (tool != null) {
                e.Cursor = tool.Cursor;
                e.Handled = true;
            }

            base.OnQueryCursor(e);
        }

        public void AddToolTip(ToolTipData toolTipData) {
            _toolTipDatas.Add(toolTipData);
        }
        public void RemoveToolTip(ToolTipData toolTipData) {
            _toolTipDatas.Remove(toolTipData);
        }

        public void UpdateToolTip() {
            var text = string.Join(Environment.NewLine, _toolTipDatas.Where(data => data.IsEnabled).Select(data => data.Text));

            if (string.IsNullOrEmpty(text))
                HideToolTip();

            else {
                _toolTip.Content = text;

                ShowToolTip();
            }
        }

        public void ShowToolTip() {
            if (_toolTip.IsOpen)
                return;

            ToolTip = _toolTip;
            _toolTip.IsOpen = true;
        }
        public void HideToolTip() {
            if (!_toolTip.IsOpen)
                return;

            _toolTip.IsOpen = false;
            ToolTip = null;
        }

        
        private void _toolTipDatas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            PropertyChangedEventHandler updateToolTip = delegate { UpdateToolTip(); };

            // а тут рано или поздно потечёт память

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (ToolTipData item in e.NewItems)
                        item.PropertyChanged += updateToolTip;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ToolTipData item in e.OldItems)
                        item.PropertyChanged -= updateToolTip;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (ToolTipData item in e.NewItems)
                        item.PropertyChanged += updateToolTip;
                    foreach (ToolTipData item in e.OldItems)
                        item.PropertyChanged -= updateToolTip;
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (ToolTipData item in e.NewItems)
                        item.PropertyChanged += updateToolTip;
                    foreach (ToolTipData item in e.OldItems)
                        item.PropertyChanged -= updateToolTip;
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private void InvalidateRect(Rect rect) {
            if (!_dirtyRect.HasValue)
                _dirtyRect = rect;
            else
                _dirtyRect = Rect.Union(_dirtyRect.Value, rect);

            InvalidateVisual();
        }

        private static void Swap<T>(ref T l, ref T r) {
            var m = l;
            l = r;
            r = m;
        }
    }
}
