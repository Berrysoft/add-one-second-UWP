﻿using System;
using System.ComponentModel;
using Windows.System.Display;
using Windows.UI.ViewManagement;

namespace addOneSecond
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Second { get; set; }

        public DateTime RealTime => DateTime.Now.AddSeconds(Second);

        public void RefreshRealTime()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RealTime)));
        }

        public TimeSpan TotalTime => TimeSpan.FromSeconds(Second);

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
            BackgroundHelper.RegisterLiveTile(TileFresh);
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
        public int PlayAudioLimit => 233;
        private void OnPlayAudioChanged()
        {
            if (PlayAudio && Second < PlayAudioLimit)
            {
                PlayAudio = false;
            }
        }
    }
}
