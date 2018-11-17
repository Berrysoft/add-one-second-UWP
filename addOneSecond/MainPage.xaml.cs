using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.Web.Http;

namespace addOneSecond
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();//定义定时器
        Random rankey = new Random();
        static HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)  //页面加载完毕
        {
            await GetSettings();
            //加载秒数统计
            Model.Second = await GetTotalSecond();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新时间
            timer.Start();  //开始计时器

            //覆盖电脑状态栏
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }

        private async void Timer_Tick(object sender, object e)    //1s定时执行
        {
            if (Model.AutoAdd)
            {
                await SecondAdd();
            }
            try
            {
                string allSecondsString = await client.GetStringAsync(new Uri("https://angry.im/l/life"));  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                TimeSpan span = TimeSpan.FromSeconds(allSeconds);
                secondsShow.Text = span.ToString("d\\:hh\\:mm\\:ss");  //显示结果
            }
            catch (Exception) { }
        }

        private async void SecondGet_Click(object sender, RoutedEventArgs e)  //+1s按键
        {
            await SecondAdd();
        }

        private void PlayAudio() //播放声音
        {
            if (MyMediaElement.CurrentState != MediaElementState.Playing && Model.PlayAudio)
            {
                MyMediaElement.Source = new Uri("ms-appx:///Assets/wav/" + rankey.Next(1, 10) + ".wav");
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)  //设置按钮
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;  //开关SplitView设置页
        }

        //本地计数+1s
        private async Task SecondAdd()
        {
            PlayAudio();
            addedOneSecondStoryboard.Begin();  //+1s动画
            try
            {
                await client.PostAsync(new Uri("https://angry.im/p/life"), new HttpStringContent("+1s"));
                Model.Second++;
            }
            catch (Exception) { }
        }

        internal async Task SaveTotalSecond()
        {
            long seconds = Model.Second;
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("seconds", CreationCollisionOption.ReplaceExisting);
                //创建文件

                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        write.Write(seconds);
                    }
                }
            }
            catch (Exception) { }
        }  //保存总秒数

        private async Task<long> GetTotalSecond()
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("seconds", CreationCollisionOption.OpenIfExists);
            //创建文件

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    string s = await read.ReadToEndAsync();
                    if (long.TryParse(s, out long seconds))
                    {
                        return seconds;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }  //获取总秒数

        internal async Task SaveSettings()    //保存设置
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.ReplaceExisting);
                //创建文件

                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        await write.WriteAsync(
                            string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                                Model.FullScreen,
                                Model.AutoAdd,
                                BackGroundColorRedSlider.Value,
                                BackGroundColorGreenSlider.Value,
                                BackGroundColorBlueSlider.Value,
                                BackGroundAcrylicOpacitySlider.Value,
                                FontColorRedSlider.Value,
                                FontColorGreenSlider.Value,
                                FontColorBlueSlider.Value,
                                Model.DisplayRequest,
                                Model.PlayAudio));
                    }
                }
            }
            catch (Exception) { }
        }

        private async Task GetSettings()
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.OpenIfExists);
            //创建文件

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    string s = await read.ReadToEndAsync();
                    if (s.IndexOf(";") >= 1 && s.IndexOf(";") != s.Length - 1)
                    {
                        string[] str2;
                        int count_temp = 0;
                        str2 = s.Split(';');
                        foreach (string i in str2)
                        {
                            switch (count_temp)
                            {
                                case 0:
                                    Model.FullScreen = bool.Parse(i);
                                    break;
                                case 1:
                                    Model.AutoAdd = bool.Parse(i);
                                    break;
                                case 2:
                                    BackGroundColorRedSlider.Value = double.Parse(i);
                                    break;
                                case 3:
                                    BackGroundColorGreenSlider.Value = double.Parse(i);
                                    break;
                                case 4:
                                    BackGroundColorBlueSlider.Value = double.Parse(i);
                                    break;
                                case 5:
                                    BackGroundAcrylicOpacitySlider.Value = double.Parse(i);
                                    break;
                                case 6:
                                    FontColorRedSlider.Value = double.Parse(i);
                                    break;
                                case 7:
                                    FontColorGreenSlider.Value = double.Parse(i);
                                    break;
                                case 8:
                                    FontColorBlueSlider.Value = double.Parse(i);
                                    break;
                                case 9:
                                    Model.DisplayRequest = bool.Parse(i);
                                    break;
                                case 10:
                                    Model.PlayAudio = bool.Parse(i);
                                    break;
                            }
                            count_temp++;
                        }
                    }
                }
            }
            SetBackgroundColor();
            SetForegroundColor();
        }   //加载设置

        private void BackGroundColorSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //背景颜色调节
        {
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        {
            Model.PageBackgroundColor = Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value);
            Model.PageBackgroundOpacity = BackGroundAcrylicOpacitySlider.Value / 100;
        }

        private void FontColorSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //颜色调节
        {
            SetForegroundColor();
        }

        private void SetForegroundColor()
        {
            Color c = Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value);
            Model.TextForeground = new SolidColorBrush(c);
            //手机状态栏颜色
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = c;
                statusBar.BackgroundOpacity = 1;
            }
        }

        public void OpenAuto()  //语音调用的东西
        {
            Model.AutoAdd = true;
        }
    }
}
