using addOneSecond.Background;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Background;

namespace addOneSecond
{
    static class BackgroundHelper
    {
        public static void RegesterLiveTile(bool reg)
        {
            if (reg)
            {
                BackgroundTaskHelper.Register(typeof(LiveTileTask), new TimeTrigger(15, false), true);
            }
            else
            {
                BackgroundTaskHelper.Unregister(typeof(LiveTileTask));
            }
        }
    }
}
