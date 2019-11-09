using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Color = Windows.UI.Color;

namespace addOneSecond
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(ref T target, T value, Action onChanged = null, [CallerMemberName] string name = null, params string[] names)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                foreach (var n in names)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
                }
                onChanged?.Invoke();
            }
        }

        private long second;
        public long Second
        {
            get => second;
            set => SetValue(ref second, value, null, nameof(Second), nameof(RealTime));
        }

        public DateTime RealTime => DateTime.Now.AddSeconds(Second);

        private Color textForegroundColor = Colors.Black;
        public Color TextForegroundColor
        {
            get => textForegroundColor;
            set => SetValue(ref textForegroundColor, value);
        }

        private Color backgroundPickerColor = Colors.White;
        public Color BackgroundPickerColor
        {
            get => backgroundPickerColor;
            set => SetValue(ref backgroundPickerColor, value, null, nameof(BackgroundPickerColor), nameof(PageBackgroundColor), nameof(PageBackgroundOpacity));
        }

        public Color PageBackgroundColor => Color.FromArgb(0xFF, BackgroundPickerColor.R, BackgroundPickerColor.G, BackgroundPickerColor.B);

        public double PageBackgroundOpacity => BackgroundPickerColor.A / 255.0;

        private bool fullScreen;
        public bool FullScreen
        {
            get => fullScreen;
            set => SetValue(ref fullScreen, value, OnFullScreenChanged);
        }
        private void OnFullScreenChanged()
        {
            if (FullScreen)
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            }
            else
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }

        private bool autoAdd;
        public bool AutoAdd
        {
            get => autoAdd;
            set => SetValue(ref autoAdd, value);
        }

        private bool tileFresh;
        public bool TileFresh
        {
            get => tileFresh;
            set => SetValue(ref tileFresh, value, OnTileFreshChanged);
        }
        private void OnTileFreshChanged()
        {
            BackgroundHelper.RegesterLiveTile(TileFresh);
        }

        private bool displayRequest;
        public bool DisplayRequest
        {
            get => displayRequest;
            set => SetValue(ref displayRequest, value, OnDisplayRequestChanged);
        }
        private void OnDisplayRequestChanged()
        {
            SetDisplay(DisplayRequest);
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

        private bool playAudio;
        public bool PlayAudio
        {
            get => playAudio;
            set => SetValue(ref playAudio, value, OnPlayAudioChanged);
        }
        private void OnPlayAudioChanged()
        {
            if (PlayAudio && Second < 2333)
            {
                PlayAudio = false;
            }
        }
    }
}
