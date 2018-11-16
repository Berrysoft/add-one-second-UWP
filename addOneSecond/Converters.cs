using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace addOneSecond
{
    class SecondToTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (long.TryParse(value.ToString(), out long total))
            {
                return $"你已经贡献了{total}秒";
            }
            return null;
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
            if (long.TryParse(value.ToString(), out long total))
            {
                DateTime realTime = DateTime.Now.AddSeconds(total);
                return "你的实际时间：" + realTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
