using addOneSecond.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace addOneSecond
{
    static class BackgroundHelper
    {
        public static async Task<bool> RequestAccessAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser || status == BackgroundAccessStatus.DeniedBySystemPolicy)
            {
                return false;
            }
            return true;
        }

        private const string LIVETILETASK = "LIVETILETASK";

        public static void RegesterLiveTile(bool reg)
        {
            foreach (var t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == LIVETILETASK)
                {
                    t.Value.Unregister(true);
                }
            }
            if (reg)
            {
                var taskBuilder = new BackgroundTaskBuilder
                {
                    Name = LIVETILETASK,
                    TaskEntryPoint = typeof(LiveTileTask).FullName
                };
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                taskBuilder.Register();
            }
        }
    }
}
