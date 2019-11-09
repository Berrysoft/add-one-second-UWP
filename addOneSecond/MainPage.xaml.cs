using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.Web.Http;

namespace addOneSecond
{
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
            //if (MyMediaElement.CurrentState != MediaElementState.Playing && Model.PlayAudio)
            //{
            //    MyMediaElement.Source = new Uri("ms-appx:///Assets/wav/" + rankey.Next(1, 10) + ".wav");
            //}
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
                    ["tileRefresh"] = JsonValue.CreateBooleanValue(Model.TileFresh),
                    ["displayRequest"] = JsonValue.CreateBooleanValue(Model.DisplayRequest),
                    ["playAudio"] = JsonValue.CreateBooleanValue(Model.PlayAudio),
                    ["bkR"] = JsonValue.CreateNumberValue(Model.PageBackgroundColor.R),
                    ["bkG"] = JsonValue.CreateNumberValue(Model.PageBackgroundColor.G),
                    ["bkB"] = JsonValue.CreateNumberValue(Model.PageBackgroundColor.B),
                    ["bkA"] = JsonValue.CreateNumberValue(Model.PageBackgroundOpacity),
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
            catch (Exception) { }
        }

        private async Task GetSettings()
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
            //创建文件
            try
            {
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
                            Model.TileFresh = json.GetNamedBoolean("tileRefresh");
                            Model.DisplayRequest = json.GetNamedBoolean("displayRequest");
                            Model.PlayAudio = json.GetNamedBoolean("playAudio");
                            Model.BackgroundPickerColor = Color.FromArgb((byte)(json.GetNamedNumber("bkA") * 255), (byte)json.GetNamedNumber("bkR"), (byte)json.GetNamedNumber("bkG"), (byte)json.GetNamedNumber("bkB"));
                            Model.TextForegroundColor = Color.FromArgb(0xFF, (byte)json.GetNamedNumber("frR"), (byte)json.GetNamedNumber("frG"), (byte)json.GetNamedNumber("frB"));
                        }
                    }
                }
            }
            catch (Exception) { }
            BackgroundHelper.RegesterLiveTile(Model.TileFresh);
        }   //加载设置

        public void OpenAuto()  //语音调用的东西
        {
            Model.AutoAdd = true;
            voiceAutoAdd = true;
        }
    }
}
