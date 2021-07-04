using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace addOneSecond
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly DispatcherTimer timer = new DispatcherTimer();//定义定时器
        readonly Random rankey = new Random();
        bool voiceAutoAdd = false;

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

        private void Timer_Tick(object sender, object e)    //1s定时执行
        {
            if (Model.AutoAdd)
            {
                SecondAdd();
            }
        }

        private void SecondGet_Click(object sender, RoutedEventArgs e)  //+1s按键
        {
            SecondAdd();
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
        private void SecondAdd()
        {
            Model.Second++;
            addedOneSecondStoryboard.Begin();
            PlayAudio();
        }

        internal async Task SaveSettings()    //保存设置
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            var file_demonstration = await folder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting);
            //创建文件

            JsonObject json = new JsonObject()
            {
                ["totalSeconds"] = JsonValue.CreateNumberValue(Model.Second),
                ["fullScreen"] = JsonValue.CreateBooleanValue(Model.FullScreen),
                ["autoAdd"] = JsonValue.CreateBooleanValue(Model.AutoAdd),
                ["tileRefresh"] = JsonValue.CreateBooleanValue(Model.TileFresh),
                ["displayRequest"] = JsonValue.CreateBooleanValue(Model.DisplayRequest),
                ["playAudio"] = JsonValue.CreateBooleanValue(Model.PlayAudio),
                ["frR"] = JsonValue.CreateNumberValue(Model.TextForegroundColor.R),
                ["frG"] = JsonValue.CreateNumberValue(Model.TextForegroundColor.G),
                ["frB"] = JsonValue.CreateNumberValue(Model.TextForegroundColor.B)
            };
            using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
            {
                using (StreamWriter write = new StreamWriter(file))
                {
                    await write.WriteAsync(json.ToString());
                }
            }
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
                        Model.Second = (long)json.GetNamedNumber("totalSeconds", 0);
                        Model.FullScreen = json.GetNamedBoolean("fullScreen", false);
                        Model.AutoAdd = json.GetNamedBoolean("autoAdd", false) || voiceAutoAdd;
                        Model.TileFresh = json.GetNamedBoolean("tileRefresh", true);
                        Model.DisplayRequest = json.GetNamedBoolean("displayRequest", false);
                        Model.PlayAudio = json.GetNamedBoolean("playAudio", false);
                        Model.TextForegroundColor = Color.FromArgb(0xFF, (byte)json.GetNamedNumber("frR", 0), (byte)json.GetNamedNumber("frG", 0), (byte)json.GetNamedNumber("frB", 0));
                    }
                }
            }
            BackgroundHelper.RegesterLiveTile(Model.TileFresh);
        }   //加载设置

        public void OpenAuto()  //语音调用的东西
        {
            Model.AutoAdd = true;
            voiceAutoAdd = true;
        }
    }
}
