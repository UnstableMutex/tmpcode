using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CAP
{
    public class CoupleAlignmentPanel : Panel
    {
        public Thickness ControlsMargin
        {
            get { return (Thickness)GetValue(ControlsMarginProperty); }
            set { SetValue(ControlsMarginProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ControlsMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlsMarginProperty =
            DependencyProperty.Register("ControlsMargin", typeof(Thickness), typeof(CoupleAlignmentPanel), new FrameworkPropertyMetadata(new Thickness(0)));
        protected override Size MeasureOverride(Size availableSize)
        {
            if (IsOdd(Children.Count))
            {
                throw new CoupleAlignmentPanelException("Children count must be even");
            }
            MeasureChildrenByInfinity();
            var elements = GetElementPairs();
            Func<UIElement, double> g =
                x =>
                {
                    double d = x.DesiredSize.Height;
                    return d;
                };
            var heights = GetHeights(elements, g);
            //   var width = elements.Select(x => x.Item1.DesiredSize.Width + x.Item2.DesiredSize.Width).Max();
            foreach (UIElement child in Children)
            {
                var ch = child as FrameworkElement;
                if (ch != null)
                {
                    var be = ch.GetBindingExpression(MarginProperty);
                    if (be == null)
                    {
                        if (ch.Margin.IsEmpty())
                        {
                            ch.Margin = ControlsMargin;
                        }
                    }
                    else
                    {
                        if (be.ParentBinding == null)
                        {
                            if (ch.Margin.IsEmpty())
                            {
                                ch.Margin = ControlsMargin;
                            }
                        }
                    }
                }
                child.Arrange(new Rect(new Point(0, 0), child.DesiredSize));
            }
            //  return new Size(width, heights.Sum());
            return new Size(availableSize.Width, heights.Sum());
        }
        bool IsOdd(int i)
        {
            return i % 2 == 1;
        }
        bool IsEven(int i)
        {
            return i % 2 == 0;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (IsOdd(Children.Count))
            {
                throw new CoupleAlignmentPanelException("Children count must be even");
            }
            MeasureChildrenByInfinity();
            var elements = GetElementPairs();
            var heights = GetHeights(elements, x => x.DesiredSize.Height);
            var labelssizes = elements.Select(element => element.Item1.DesiredSize);
            var i = 0;
            //считаем максимальную ширину лебела по которой будем выравнивать все
            var maxWidth = labelssizes.Max(x => x.Width);
            var txtWidth = finalSize.Width - maxWidth;

            //  var txtWidth = elements.Select(e => e.Item2.DesiredSize).Max(x=>x.Width);

            if (txtWidth < 0)
            {
                txtWidth = 0;
            }
            foreach (var element in elements)
            {
                //считаем y как сумму высот всех лебелов до текущего
                var y = heights.Take(i).Sum();
                element.Item1.Arrange(new Rect(0, y, maxWidth, heights[i]));
                element.Item2.Arrange(new Rect(maxWidth, y, txtWidth, heights[i]));
                i++;
            }
            if (!_measureInvalidated)
            {
                InvalidateMeasure();
                _measureInvalidated = true;
            }
            return new Size(txtWidth + maxWidth, heights.Sum());
        }
        private bool _measureInvalidated;
        private static double[] GetHeights(Tuple<UIElement, UIElement>[] elements, Func<UIElement, double> heightGetter)
        {
            var res = new double[elements.Count()];
            int i = 0;
            foreach (var element in elements)
            {
                res[i++] = Math.Max(heightGetter(element.Item1), heightGetter(element.Item2));
            }
            return res;
        }
        private Tuple<UIElement, UIElement>[] GetElementPairs()
        {
            int i = -1;
            var half = Children.Count / 2;
            var even = new UIElement[half];
            var odd = new UIElement[half];
            foreach (UIElement child in Children)
            {
                i++;
                bool ise = IsEven(i);
                if (ise)
                {
                    even[i / 2] = child;
                }
                else
                {
                    odd[(i - 1) / 2] = child;
                }
            }
            var elements = new Tuple<UIElement, UIElement>[half];
            for (i = 0; i < half; i++)
            {
                elements[i] = new Tuple<UIElement, UIElement>(even[i], odd[i]);
            }
            return elements;
        }
        private void MeasureChildrenByInfinity()
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement child in Children)
            {
                child.Measure(size);
            }
        }
    }
}

