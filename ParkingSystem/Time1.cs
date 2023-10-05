using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parking
{
    class Time1
    {
        private string date;
        private string hour;
        private string minute;

        public string Gettime()
        {
            date = DateTime.Now.ToString("yyyyMMdd");
            hour = DateTime.Now.Hour.ToString();//获取小时
            minute = DateTime.Now.Minute.ToString();//获取分钟
            if (Convert.ToInt32(hour) < 10) { hour = '0' + hour; }
            if (Convert.ToInt32(minute) < 10) { minute = '0' + minute; }
            return date + hour + minute;
        }
    }
}
