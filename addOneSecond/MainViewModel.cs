using System;
using System.ComponentModel;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace addOneSecond
{
    class MainViewModel : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public long Second { get; set; }

        public DateTime RealTime => DateTime.Now.AddSeconds(Second);

        public TimeSpan TotalTime => TimeSpan.FromSeconds(Second);

        public Color TextForegroundColor { get; set; } = Colors.Black;

        public bool FullScreen { get; set; }
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

        public bool AutoAdd { get; set; }

        public bool TileFresh { get; set; }
        private void OnTileFreshChanged()
        {
            BackgroundHelper.RegesterLiveTile(TileFresh);
        }

        public bool DisplayRequest { get; set; }
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


        public bool PlayAudio { get; set; }
        private void OnPlayAudioChanged()
        {
            if (PlayAudio && Second < 2333)
            {
                PlayAudio = false;
            }
        }
    }
}
