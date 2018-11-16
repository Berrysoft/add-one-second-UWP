using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Background;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.VoiceCommands;

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
            GetSettings();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新时间
            timer.Start();  //开始计时器

            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///VoiceCommandDictionary.xml"));
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);     //加载语音字典

            //覆盖电脑状态栏
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //加载秒数统计
            long total;
            total = await GetTotalSecond();
            SaveTotalSecond(total);
            secondTotalShow.Text = $"你已经贡献了{total}秒";
        }

        private async void Timer_Tick(object sender, object e)    //1s定时执行
        {
            if (isAutoAddOneSecondOpen.IsOn)
            {
                PlayAudio();
                addedOneSecondStoryboard.Begin();  //+1s动画
                try
                {
                    await client.PostAsync("https://angry.im/p/life", new StringContent("+1s", Encoding.UTF8, "application/x-www-form-urlencoded"));//post用来+1s
                    SecondAdd();
                }
                catch (Exception) { }
            }
            try
            {
                string allSecondsString = await client.GetStringAsync("https://angry.im/l/life");  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                TimeSpan span = TimeSpan.FromSeconds(allSeconds);
                secondsShow.Text = span.ToString("d\\:hh\\:mm\\:ss");  //显示结果
            }
            catch (Exception) { }
            await ShowRealTime();
        }

        private async void secondGet_Click(object sender, RoutedEventArgs e)  //+1s按键
        {
            PlayAudio();
            addedOneSecondStoryboard.Begin();  //+1s动画
            try
            {
                await client.PostAsync("https://angry.im/p/life", new StringContent("+1s", Encoding.UTF8, "application/x-www-form-urlencoded"));//post用来+1s
                SecondAdd();
            }
            catch (Exception) { }
        }

        private void PlayAudio() //播放声音
        {
            if (MyMediaElement.CurrentState.ToString() != "Playing" && isPlayAudio.IsOn)
            {
                MyMediaElement.Source = new Uri("ms-appx:///Assets/wav/" + rankey.Next(1, 10) + ".wav");
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)  //设置按钮
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;  //开关SplitView设置页
        }

        private void isfullScreen_Toggled(object sender, RoutedEventArgs e)  //全屏按钮
        {
            if (isfullScreen.IsOn)
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();  //全屏
            }
            else
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
            SaveSettings();
        }

        private void ApplyBackGroungColor_Click(object sender, RoutedEventArgs e) //应用背景颜色
        {
            SaveSettings();
        }

        private void ApplyFontColor_Click(object sender, RoutedEventArgs e)//应用字体颜色
        {
            SaveSettings();
        }

        private async void SecondAdd()
        {
            try
            {
                long total;
                total = await GetTotalSecond();
                total++;
                SaveTotalSecond(total);
                secondTotalShow.Text = $"你已经贡献了{total}秒";
            }
            catch { }
        }   //本地计数+1s

        private async void SaveTotalSecond(long seconds)
        {
            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("seconds", CreationCollisionOption.ReplaceExisting);
                //创建文件

                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        write.Write(string.Format($"{seconds}"));
                    }
                }
            }
            catch (Exception) { }
        }  //保存总秒数

        private async Task<long> GetTotalSecond()
        {
            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("seconds", CreationCollisionOption.OpenIfExists);
            //创建文件

            string s;

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    s = read.ReadToEnd();
                }
            }

            if (long.TryParse(s, out long seconds))
            {
                return seconds;
            }
            else
            {
                return 0;
            }
        }  //获取总秒数

        private async void SaveSettings()    //保存设置
        {
            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹
            try
            {
                var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.ReplaceExisting);
                //创建文件


                using (Stream file = await file_demonstration.OpenStreamForWriteAsync())
                {
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        write.Write(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                                                    isfullScreen.IsOn,
                                                    isAutoAddOneSecondOpen.IsOn,
                                                    BackGroundColorRedSlider.Value,
                                                    BackGroundColorGreenSlider.Value,
                                                    BackGroundColorBlueSlider.Value,
                                                    FontColorRedSlider.Value,
                                                    FontColorGreenSlider.Value,
                                                    FontColorBlueSlider.Value,
                                                    isTileFresh.IsOn,
                                                    isDisplayRequest.IsOn,
                                                    isPlayAudio.IsOn
                                                   ));
                    }
                }
            }
            catch (Exception) { }

        }

        private async void GetSettings()
        {
            BackGroundColorRedSlider.Value = 255;
            BackGroundColorGreenSlider.Value = 255;
            BackGroundColorBlueSlider.Value = 255;


            StorageFolder folder;
            folder = ApplicationData.Current.RoamingFolder; //获取应用目录的文件夹

            var file_demonstration = await folder.CreateFileAsync("settings", CreationCollisionOption.OpenIfExists);
            //创建文件

            string s;

            using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    s = read.ReadToEnd();
                }
            }


            if (s.IndexOf(";") >= 1 && s.IndexOf(";") != s.Length - 1)
            {
                string[] str2;
                int count_temp = 0;
                str2 = s.Split(';');
                foreach (string i in str2)
                {
                    if (count_temp == 0)
                    {
                        if (i.ToString() == "True")
                        {
                            isfullScreen.IsOn = true;
                        }
                        else
                        {
                            isfullScreen.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 1)
                    {
                        if (i.ToString() == "True")
                        {
                            isAutoAddOneSecondOpen.IsOn = true;
                        }
                        else
                        {
                            isAutoAddOneSecondOpen.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 2)
                    {
                        BackGroundColorRedSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 3)
                    {
                        BackGroundColorGreenSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 4)
                    {
                        BackGroundColorBlueSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 5)
                    {
                        FontColorRedSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 6)
                    {
                        FontColorGreenSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 7)
                    {
                        FontColorBlueSlider.Value = double.Parse(i.ToString());
                        count_temp++;
                    }
                    else if (count_temp == 8)
                    {
                        if (i.ToString() == "True")
                        {
                            isTileFresh.IsOn = true;
                        }
                        else
                        {
                            isTileFresh.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 9)
                    {
                        if (i.ToString() == "True")
                        {
                            isDisplayRequest.IsOn = true;
                        }
                        else
                        {
                            isDisplayRequest.IsOn = false;
                        }
                        count_temp++;
                    }
                    else if (count_temp == 10)
                    {
                        if (i.ToString() == "True")
                        {
                            isPlayAudio.IsOn = true;
                        }
                        else
                        {
                            isPlayAudio.IsOn = false;
                        }
                        count_temp++;
                    }
                }
            }
            SetBackGroundColor();
            SetForeColor();
        }   //加载设置

        private void isAutoAddOneSecondOpen_Toggled(object sender, RoutedEventArgs e)  //自动+1s开关按键
        {
            SaveSettings();
        }

        private void BackGroundColorRedSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //背景颜色调节
        {
            SetBackGroundColor();
        }

        private void BackGroundColorGreenSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //背景颜色调节
        {
            SetBackGroundColor();
        }

        private void BackGroundColorBlueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //背景颜色调节
        {
            SetBackGroundColor();
        }

        private void FontColorRedSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //颜色调节
        {
            SetForeColor();
        }

        private void FontColorGreenSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //颜色调节
        {
            SetForeColor();
        }

        private void FontColorBlueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)  //颜色调节
        {
            SetForeColor();
        }


        private async void isTileFresh_Toggled(object sender, RoutedEventArgs e)  //磁贴设置按钮
        {
            if (isTileFresh.IsOn)
            {
                await setLiveTile();
            }
            SaveSettings();
        }


        private async Task setLiveTile()   //开启磁贴
        {
            try
            {
                string allSecondsString = await client.GetStringAsync("https://angry.im/l/life");  //获取秒数
                long allSeconds = long.Parse(allSecondsString);   //转换成long
                long dd, mm, hh, ss;     //用于存储最终数值
                dd = allSeconds / 60 / 60 / 24;
                hh = allSeconds / 60 / 60 % 24;
                mm = allSeconds / 60 % 60;
                ss = allSeconds % 60;

                var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);

                var tileAttributes = tileXml.GetElementsByTagName("text");
                tileAttributes[0].AppendChild(tileXml.CreateTextNode("时间众筹总计"));
                tileAttributes[1].AppendChild(tileXml.CreateTextNode(dd + "天"));
                tileAttributes[2].AppendChild(tileXml.CreateTextNode(hh + "小时"));
                tileAttributes[3].AppendChild(tileXml.CreateTextNode(mm + "分钟"));
                var tileNotification = new TileNotification(tileXml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch (Exception) { }

            var tileContent = new Uri("https://www.chenxublog.com/getsecond.php");  //自建网站
            var requestedInterval = PeriodicUpdateRecurrence.HalfHour;   //半小时一次

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(tileContent, requestedInterval);
        }

        private Windows.System.Display.DisplayRequest _displayRequest;

        private void isDisplayRequest_Toggled(object sender, RoutedEventArgs e) //常亮按钮
        {
            if (isDisplayRequest.IsOn)
            {
                //create the request instance if needed
                if (_displayRequest == null)
                    _displayRequest = new Windows.System.Display.DisplayRequest();
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
            SaveSettings();
        }


        private void SetForeColor()  //设置字体颜色
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value));
            secondsShow.Foreground = color;    //应用字体颜色
            secondGet.Foreground = color;
            addedOneSecondTextBlock.Foreground = color;
            settings.Foreground = color;
            secondTotalShow.Foreground = color;
            realTime.Foreground = color;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                //statusBar.BackgroundColor = Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value);
                statusBar.ForegroundColor = Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value);
                statusBar.BackgroundOpacity = 1;
            }//手机状态栏颜色
        }

        private void SetBackGroundColor()  //设置背景颜色
        {
            try
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase"))
                {
                    AcrylicBrush myBrush = new AcrylicBrush();
                    myBrush.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    myBrush.TintColor = Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value);
                    myBrush.FallbackColor = Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value);
                    myBrush.TintOpacity = BackGroundAcrylicBlueSlider.Value / 100;

                    mainGrid.Background = myBrush;
                }
                else
                {
                    SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value));
                    mainGrid.Background = color;    //应用背景颜色

                    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                    {
                        StatusBar statusBar = StatusBar.GetForCurrentView();
                        statusBar.BackgroundColor = Color.FromArgb(255, (byte)BackGroundColorRedSlider.Value, (byte)BackGroundColorGreenSlider.Value, (byte)BackGroundColorBlueSlider.Value);
                        //statusBar.ForegroundColor = Color.FromArgb(255, (byte)FontColorRedSlider.Value, (byte)FontColorGreenSlider.Value, (byte)FontColorBlueSlider.Value);
                        statusBar.BackgroundOpacity = 1;
                    }//手机状态栏颜色
                }
            }
            catch (Exception) { }
        }

        private async Task ShowRealTime()  //显示被续过的时间
        {
            try
            {
                long total;
                total = await GetTotalSecond();
                DateTime now = DateTime.Now;
                DateTime timeDeleted = now.AddSeconds(total);
                realTime.Text = "你的实际时间：" + timeDeleted.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
            catch (Exception) { }
        }

        public void openAuto()  //语音调用的东西
        {
            isAutoAddOneSecondOpen.IsOn = true;
        }

        private async void isPlayAudio_Toggled(object sender, RoutedEventArgs e)   //音效开关按钮
        {
            try
            {
                if (await GetTotalSecond() < 2333)
                {
                    isPlayAudio.IsOn = false;
                }
            }
            catch (Exception)
            {
                isPlayAudio.IsOn = false;
            }
            SaveSettings();
        }

        private void BackGroundAcrylicBlueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SetBackGroundColor();
        }
    }
}
