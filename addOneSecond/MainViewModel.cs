using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace addOneSecond
{
    class MainViewModel : DependencyObject
    {
        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register(nameof(Second), typeof(long), typeof(MainViewModel), new PropertyMetadata(0L));
        public long Second
        {
            get => (long)GetValue(SecondProperty);
            set => SetValue(SecondProperty, value);
        }

        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register(nameof(TextForeground), typeof(Brush), typeof(MainViewModel), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public Brush TextForeground
        {
            get => (Brush)GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }
    }
}
