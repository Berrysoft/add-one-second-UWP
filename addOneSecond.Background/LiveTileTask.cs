using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Http;

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
            poems = await FileIO.ReadLinesAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///poems.txt")));
            tileT = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///tile.xml")));
        }

        private static HttpClient client = new HttpClient();
        private Random rand = new Random();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                string result = await client.GetStringAsync(new Uri("https://angry.im/l/life"));
                TimeSpan span = TimeSpan.FromSeconds(long.Parse(result));
                DateTime date = DateTime.Now;
                bool isBirthday = date.Month == 8 && date.Day == 17;
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(string.Format(tileT,
                    isBirthday ? "<image src=\"ms-appx:///Birthday.png\" placement=\"background\"/>" : null,
                    span.Days, span.Hours, span.Minutes,
                    poems[rand.Next(poems.Count - 1)]));
                var notification = new TileNotification(dom);
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.Update(notification);
            }
            catch (Exception) { }
            deferral.Complete();
        }
    }
}
