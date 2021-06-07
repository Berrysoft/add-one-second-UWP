using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace addOneSecond.Background
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        private IList<string> poems;
        private string tileT;
        public LiveTileTask()
        {
            LoadPoems().Wait();
        }

        private async Task LoadPoems()
        {
            poems = await FileIO.ReadLinesAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///addOneSecond.Background/poems.txt")));
            tileT = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///addOneSecond.Background/tile.xml")));
        }

        private async Task<long> GetTotalSeconds()
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
                        return (long)json.GetNamedNumber("totalSeconds", 0);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        private readonly Random rand = new Random();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                TimeSpan span = TimeSpan.FromSeconds(await GetTotalSeconds());
                DateTime date = DateTime.Now;
                bool isBirthday = date.Month == 8 && date.Day == 17;
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(string.Format(tileT,
                    isBirthday ? "<image src=\"ms-appx:///addOneSecond.Background/Birthday.png\" placement=\"peek\"/>" : null,
                    span.Days, span.Hours, span.Minutes,
                    poems[rand.Next(poems.Count)]));
                var notification = new TileNotification(dom);
                notification.ExpirationTime = date.AddMinutes(15);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
