using Windows.System.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

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

        public static readonly DependencyProperty TextForegroundColorProperty = DependencyProperty.Register(nameof(TextForegroundColor), typeof(Color), typeof(MainViewModel), new PropertyMetadata(Colors.Black));
        public Color TextForegroundColor
        {
            get => (Color)GetValue(TextForegroundColorProperty);
            set => SetValue(TextForegroundColorProperty, value);
        }

        public static readonly DependencyProperty BackgroundPickerColorProperty = DependencyProperty.Register(nameof(BackgroundPickerColor), typeof(Color), typeof(MainViewModel), new PropertyMetadata(Colors.White, BackgroundPickerColorPropertyChangedCallback));
        public Color BackgroundPickerColor
        {
            get => (Color)GetValue(BackgroundPickerColorProperty);
            set => SetValue(BackgroundPickerColorProperty, value);
        }
        private static void BackgroundPickerColorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel model = (MainViewModel)d;
            Color value = (Color)e.NewValue;
            model.PageBackgroundColor = Color.FromArgb(0xFF, value.R, value.G, value.B);
            model.PageBackgroundOpacity = value.A / 255.0;
        }

        public static readonly DependencyProperty PageBackgroundColorProperty = DependencyProperty.Register(nameof(PageBackgroundColor), typeof(Color), typeof(MainViewModel), new PropertyMetadata(Colors.WhiteSmoke));
        public Color PageBackgroundColor
        {
            get => (Color)GetValue(PageBackgroundColorProperty);
            set => SetValue(PageBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty PageBackgroundOpacityProperty = DependencyProperty.Register(nameof(PageBackgroundOpacity), typeof(double), typeof(MainViewModel), new PropertyMetadata(1.0));
        public double PageBackgroundOpacity
        {
            get => (double)GetValue(PageBackgroundOpacityProperty);
            set => SetValue(PageBackgroundOpacityProperty, value);
        }

        public static readonly DependencyProperty FullScreenProperty = DependencyProperty.Register(nameof(FullScreen), typeof(bool), typeof(MainViewModel), new PropertyMetadata(false, FullScreenPropertyChangedCallback));
        public bool FullScreen
        {
            get => (bool)GetValue(FullScreenProperty);
            set => SetValue(FullScreenProperty, value);
        }
        private static void FullScreenPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            }
            else
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }

        public static readonly DependencyProperty AutoAddProperty = DependencyProperty.Register(nameof(AutoAdd), typeof(bool), typeof(MainViewModel), new PropertyMetadata(false));
        public bool AutoAdd
        {
            get => (bool)GetValue(AutoAddProperty);
            set => SetValue(AutoAddProperty, value);
        }

        public static readonly DependencyProperty TileFreshProperty = DependencyProperty.Register(nameof(TileFresh), typeof(bool), typeof(MainViewModel), new PropertyMetadata(false, TileFreshProperyChangedCallback));
        public bool TileFresh
        {
            get => (bool)GetValue(TileFreshProperty);
            set => SetValue(TileFreshProperty, value);
        }
        private static void TileFreshProperyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BackgroundHelper.RegesterLiveTile((bool)e.NewValue);
        }

        public static readonly DependencyProperty DisplayRequestProperty = DependencyProperty.Register(nameof(DisplayRequest), typeof(bool), typeof(MainViewModel), new PropertyMetadata(false, DisplayRequestPropertyChangedCallback));
        public bool DisplayRequest
        {
            get => (bool)GetValue(DisplayRequestProperty);
            set => SetValue(DisplayRequestProperty, value);
        }
        private static void DisplayRequestPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel model = (MainViewModel)d;
            model.SetDisplay((bool)e.NewValue);
        }
        private DisplayRequest _displayRequest;
        private void SetDisplay(bool request)
        {
            if (request)
            {
                //create the request instance if needed
                if (_displayRequest == null)
                    _displayRequest = new DisplayRequest();
                //make request to put in active state
                _displayRequest.RequestActive();
            }
            else
            {
                //must be same instance, so quit if it doesn't exist
                if (_displayRequest == null)
                    return;
                //undo the request
                _displayRequest.RequestRelease();
            }
        }


        public static readonly DependencyProperty PlayAudioProperty = DependencyProperty.Register(nameof(PlayAudio), typeof(bool), typeof(MainViewModel), new PropertyMetadata(false, PlayAudioPropertyChangedCallback));
        public bool PlayAudio
        {
            get => (bool)GetValue(PlayAudioProperty);
            set => SetValue(PlayAudioProperty, value);
        }
        private static void PlayAudioPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel model = (MainViewModel)d;
            if ((bool)e.NewValue && model.Second < 2333)
            {
                model.PlayAudio = false;
            }
        }
    }
}
