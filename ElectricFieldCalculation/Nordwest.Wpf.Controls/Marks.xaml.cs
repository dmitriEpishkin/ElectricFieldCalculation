using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Nordwest.Wpf.Controls {
    /// <summary>
    /// Логика взаимодействия для Marks.xaml
    /// </summary>
    public partial class Marks : UserControl {
        public Marks() {
            InitializeComponent();
        }

        
        private void SetLines()
        {
            var c = canv.Children;
            c.Clear();

            var g = new GeometryGroup();

            for (int i = 0; i < Lines.Count; i++)
            {
                var f = Lines[i];
                int count = 1;
                while (i + count < Lines.Count && Lines[i + count] - Lines[i + count - 1] < 1 / 1500.0f)
                {
                    count++;
                }

                if (count == 1)
                {
                    g.Children.Add(new LineGeometry(new Point(Lines[i] * canv.Width, 0),
                                                    new Point(Lines[i] * canv.Width, 10)));
                }
                else
                {
                    g.Children.Add(new RectangleGeometry(new Rect(Lines[i] * canv.Width, 0,
                                                                  Lines[i + count - 1] * canv.Width - Lines[i] * canv.Width,
                                                                  5)));
                    i += count - 1;
                }
            }
            var p = new Path();
            p.Data = g;
            p.Stroke = new SolidColorBrush(Colors.DarkRed);
            p.Stretch = Stretch.Fill;
            p.Fill = new SolidColorBrush(Colors.DarkRed);
            p.StrokeThickness = 0.3; //1500/(double)Length;

            c.Add(p);
        }

        // TODO
        public int Length { get; set; }

       public static readonly DependencyProperty LinesProperty =
           DependencyProperty.Register("Lines", typeof(List<float>), typeof(Marks), new PropertyMetadata(default(List<float>), OnLinesChanged));

        public List<float> Lines {
            get { return (List<float>)GetValue(LinesProperty); }
            set {
                if (Lines == null || !value.SequenceEqual(Lines)) {
                    SetValue(LinesProperty, value);
                }
            }
        }
        private static void OnLinesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = (Marks)sender;
            if (e.NewValue is List<float>) {
                control.SetLines();
            }
        }
    }
}
