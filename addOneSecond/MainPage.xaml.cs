using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI;
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
        bool voiceAutoAdd = false;
        static HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)  //页面加载完毕
        {
            await GetSettings();

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
            Task addt = null;
            if (Model.AutoAdd)
                addt = SecondAdd();
            try
            {
                string allSecondsString = await client.GetStringAsync(new Uri("https://angry.im/l/life"));  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                TimeSpan span = TimeSpan.FromSeconds(allSeconds);
                secondsShow.Text = span.ToString("d\\:hh\\:mm\\:ss");  //显示结果
            }
            catch (Exception) { }
            if (addt != null)
                await addt;
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
            try
            {
                await client.PostAsync(new Uri("https://angry.im/p/life"), new HttpStringContent("+1s"));
                Model.Second++;
            }
            catch (Exception) { }
            addedOneSecondStoryboard.Begin();  //+1s动画
            PlayAudio();
        }

        internal async Task SaveSettings()    //保存设置
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting);
                //创建文件

                JsonObject json = new JsonObject()
                {
                    ["totalSeconds"] = JsonValue.CreateNumberValue(Model.Second),
                    ["fullScreen"] = JsonValue.CreateBooleanValue(Model.FullScreen),
                    ["autoAdd"] = JsonValue.CreateBooleanValue(Model.AutoAdd),
                    ["displayRequest"] = JsonValue.CreateBooleanValue(Model.DisplayRequest),
                    ["playAudio"] = JsonValue.CreateBooleanValue(Model.PlayAudio),
                    ["bkR"] = JsonValue.CreateNumberValue(BackGroundColorRedSlider.Value),
                    ["bkG"] = JsonValue.CreateNumberValue(BackGroundColorGreenSlider.Value),
                    ["bkB"] = JsonValue.CreateNumberValue(BackGroundColorBlueSlider.Value),
                    ["bkA"] = JsonValue.CreateNumberValue(BackGroundAcrylicOpacitySlider.Value),
                    ["frR"] = JsonValue.CreateNumberValue(FontColorRedSlider.Value),
                    ["frG"] = JsonValue.CreateNumberValue(FontColorGreenSlider.Value),
                    ["frB"] = JsonValue.CreateNumberValue(FontColorBlueSlider.Value)
                };
                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        await write.WriteAsync(json.ToString());
                    }
                }
            }
            catch (Exception) { }
        }

        private async Task GetSettings()
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
            //创建文件

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    string s = await read.ReadToEndAsync();
                    if (JsonObject.TryParse(s, out JsonObject json))
                    {
                        Model.Second = (long)json.GetNamedNumber("totalSeconds");
                        Model.FullScreen = json.GetNamedBoolean("fullScreen");
                        Model.AutoAdd = json.GetNamedBoolean("autoAdd") || voiceAutoAdd;
                        Model.DisplayRequest = json.GetNamedBoolean("displayRequest");
                        Model.PlayAudio = json.GetNamedBoolean("playAudio");
                        BackGroundColorRedSlider.Value = json.GetNamedNumber("bkR");
                        BackGroundColorGreenSlider.Value = json.GetNamedNumber("bkG");
                        BackGroundColorBlueSlider.Value = json.GetNamedNumber("bkB");
                        BackGroundAcrylicOpacitySlider.Value = json.GetNamedNumber("bkA");
                        FontColorRedSlider.Value = json.GetNamedNumber("frR");
                        FontColorGreenSlider.Value = json.GetNamedNumber("frG");
                        FontColorBlueSlider.Value = json.GetNamedNumber("frB");
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
        }

        public void OpenAuto()  //语音调用的东西
        {
            Model.AutoAdd = true;
            voiceAutoAdd = true;
        }
    }
}
