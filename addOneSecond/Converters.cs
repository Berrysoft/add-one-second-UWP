using System;
using Windows.UI.Xaml.Data;

namespace addOneSecond
{
    class SecondToTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"你已经贡献了{value}秒";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class SecondToRealTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime realTime = DateTime.Now.AddSeconds((long)value);
            return "你的实际时间：" + realTime.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
