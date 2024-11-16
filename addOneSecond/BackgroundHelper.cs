using System;
using System.Linq;
using addOneSecond.Background;
using Windows.ApplicationModel.Background;

namespace addOneSecond
{
    static class BackgroundHelper
    {
        static IBackgroundTaskRegistration GetBackgroundTask(Type backgroundTaskType)
        {
            return BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == backgroundTaskType.Name).Value;
        }

        static bool IsBackgroundTaskRegistered(Type backgroundTaskType)
        {
            return BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == backgroundTaskType.Name);
        }

        public static void RegisterLiveTile(bool reg)
        {
            Type type = typeof(LiveTileTask);
            if (reg)
            {
                if (IsBackgroundTaskRegistered(type))
                {
                    BackgroundTaskRegistration previous = GetBackgroundTask(type) as BackgroundTaskRegistration;
                    previous.Unregister(true);
                }

                BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                {
                    Name = type.Name,
                    TaskEntryPoint = type.FullName,
                    CancelOnConditionLoss = true,
                };

                builder.SetTrigger(new TimeTrigger(15, false));

                builder.Register();
            }
            else
            {
                GetBackgroundTask(type)?.Unregister(true);
            }
        }
    }
}
