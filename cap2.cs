using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CAP
{
    public class CoupleAlignmentPanel : Panel
    {

        public CoupleAlignmentPanel()
        {
            VerticalAlignment = VerticalAlignment.Top;
        
        }



        public Thickness ControlsMargin
        {
            get
            {

                return (Thickness)GetValue(ControlsMarginProperty);
            }
            set { SetValue(ControlsMarginProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ControlsMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlsMarginProperty =
            DependencyProperty.Register("ControlsMargin", typeof(Thickness), typeof(CoupleAlignmentPanel), new FrameworkPropertyMetadata(new Thickness(0)));

        private void SetControlsMargin()
        {
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
            }
        }


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
                bool ise = i.IsEven();
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
        //private void MeasureChildrenByInfinity()
        //{
        //    var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

        //    foreach (UIElement child in Children)
        //    {
        //        child.Measure(size);

        //    }
        //}
        protected override Size MeasureOverride(Size availableSize)
        {


            if (Children.Count.IsOdd())
            {
                throw new CoupleAlignmentPanelException("Children count must be even");
            }
            var elements = GetElementPairs();
            SetControlsMargin();

            foreach (var label in elements.Select(x => x.Item1))
            {
                label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            var labelmaxwidth = elements.Select(element => element.Item1.DesiredSize.Width).Max();

            foreach (var inputElement in elements.Select(x => x.Item2))
            {
                inputElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            var labelmaxheight = elements.Select(element => element.Item1.DesiredSize.Height).Sum();
            var inputControlMaxHeight = elements.Select(element => element.Item2.DesiredSize.Height).Sum();

            var textmaxwidth = elements.Select(element => element.Item2.DesiredSize.Width).Max();


            var w = labelmaxwidth + textmaxwidth;
            if (w < 0)
                w = 0;
           
            var h = Math.Max(labelmaxheight, inputControlMaxHeight);
            if (h < 0)
                h = 0;
            return new Size(w, h);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
         
            if (Children.Count.IsOdd())
            {
                throw new CoupleAlignmentPanelException("Children count must be even");
            }

            var elements = GetElementPairs();
            var heights = GetHeights(elements, x => x.DesiredSize.Height);
            var labelssizes = elements.Select(element => element.Item1.DesiredSize);
            var i = 0;
            //считаем максимальную ширину лебела по которой будем выравнивать все
            var maxWidth = labelssizes.Max(x => x.Width);
            if (maxWidth < 0)
            {
                maxWidth = 0;
            }

            var txtWidth = elements.Select(x => x.Item2.DesiredSize.Width).Max();
            var txtWidth2 = finalSize.Width - maxWidth;
            if (txtWidth2 < 0)
            {
                txtWidth2 = 0;
            }
      if (txtWidth < 0)
                txtWidth = 0;
            txtWidth = Math.Max(txtWidth, txtWidth2);
      


            double curY = 0;
            foreach (var element in elements)
            {
                //считаем y как сумму высот всех лебелов до текущего

                element.Item1.Arrange(new Rect(0, curY, maxWidth, heights[i]));
                element.Item2.Arrange(new Rect(maxWidth, curY, txtWidth, heights[i]));
                curY += heights.ElementAt(i);
                i++;
            }
            var w = maxWidth + txtWidth;
            if (w < 0)
            {
                w = 0;
            }

            var h = heights.Sum();
            if (h < 0)
                h = 0;
            return new Size(w, h);
        }


    }
}
