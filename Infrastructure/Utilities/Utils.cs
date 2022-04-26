using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Utilities
{
    public class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        public static long GetUsTimeDateSpan()
        {           
            return (long)(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById(Environment.OSVersion.Platform == PlatformID.Unix ?
                "America/New_York" :
                "Eastern Standard Time"))
                .Date - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)))
                .TotalMilliseconds;
        }
        /// <summary>
        /// DateTime转换为Unix时间戳-秒
        /// </summary>
        public static long ConvertToUnixOfTime(DateTime date)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(date - startTime).TotalSeconds; // 相差秒数
            return timeStamp;
        }
        /// <summary>
        /// DateTime转换为Unix时间戳-毫秒
        /// </summary>
        public static long ConvertToTimestamp(DateTime date)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(date - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// Unix时间戳转换为DateTime 相差秒数
        /// </summary>
        public static DateTime ConvertToDateTime(long unixTimeStamp)
        {         
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
           return startTime.AddSeconds(unixTimeStamp);
        }
        private static DateTime GetTime(long unixTimeStamp)
        {

            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            return startTime.AddMilliseconds(unixTimeStamp);
        }
        public static int GetB2UShort(byte[] pacInput)
        {
            return BitConverter.ToInt16(pacInput, 20);//消息数包
        }
        public static long GetB2ULong(byte[] pacInput)
        {
            return BitConverter.ToInt64(pacInput, 20);//消息数包
        }
        /// <summary>
        /// 美股时间转换
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetUsDateTimeToStr(byte[] buffer)
        {
            var a = HexStringUtil.ByteArrayTo16String(buffer);
            var b = Convert.ToInt64(a, 16);
            var date = Utils.GetUsTimeDateSpan();
            var c = date + (b / 1000 / 1000);
            var time = Utils.GetTime(c);
            return time.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
        /// <summary>
        /// 美股时间转换
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static DateTime GetUsDateTime(byte[] buffer)
        {
           return Utils.GetTime(GetUsDataTimestamp(buffer));
        }
        /// <summary>
        ///  美股时间转换
        ///  todo:Timestamps reflects the Nasdaq system time at which the outbound message was generated. Nasdaq states time as the number of nanoseconds past midnight. 
        ///  The time zone is U.S. Eastern Time.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static long GetUsDataTimestamp(byte[] buffer)
        {
            return GetUsTimeDateSpan() + (Convert.ToInt64(HexStringUtil.ByteArrayTo16String(buffer), 16) / 1000 / 1000);
            //var a = HexStringUtil.ByteArrayTo16String(buffer);
            //var b = Convert.ToInt64(a, 16);
            //var date = GetUsTimeDateSpan();
            //var c = date + (b / 1000 / 1000);
            //return c;
        }
        ///// <summary>
        /////  美股时间转换
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <returns></returns>
        public static long GetUsTimestamp(byte[] buffer)
        {
            return (Convert.ToInt64(HexStringUtil.ByteArrayTo16String(buffer), 16) / 1000 / 1000);
            //var a = HexStringUtil.ByteArrayTo16String(buffer);
            //var b = Convert.ToInt64(a, 16);
            //return (b / 1000 / 1000);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="utcTick"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTime(long utcTick)
        {
            var time = Utils.GetTime(utcTick);
            return time;
        }
        /// <summary>
        ///北京时间转换美国东部时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime BeijingTimeToUsDateTime(DateTime date)
        {
            return AMESTime.BeijingTimeToUsDateTime(date);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetPathBySysPlat()
        {
            return Path.Combine().Replace(@"\", "/");
            //TimeZoneInfo easternZone;
            //switch (Environment.OSVersion.Platform)
            //{
            //    case PlatformID.Unix:
                   
            //        break;
            //    case PlatformID.Win32NT:
                   
            //        break;
            //    default:

            //        break;
            //}
            //return Path.Combine().Replace(@"\", "/");
        }

        /// <summary>
        /// 判断时间是否在某个时间段内
        /// </summary>
        /// <param name="nowTime"></param>
        /// <param name="beginHm"></param>
        /// <param name="endHm"></param>
        /// <returns></returns>
        public static bool IsInTimeRange(DateTime nowTime, string beginHm, string endHm)
        {

            DateTime start = DateTime.Parse($"{nowTime.ToDateString()} {beginHm}");
            DateTime end = DateTime.Parse($"{nowTime.ToDateString()} {endHm}");
            if (nowTime >= start && nowTime < end)
            {
                return true;
            }
            return false;
        }
    }

    public class AMESTime
    {
        private static DateTime _thisYearDaylightSavingTimeStart,
            _thisYearDaylightSavingTimeEnd;

        private const int TIMEZONE_OFFSET_DAY_SAVING_LIGHT = -12;
        private const int TIMEZONE_OFFSET = -13;

        public static DateTime BeijingTimeToUsDateTime(DateTime beijingTime)
        {
            int offsetHours = (IsNowAMESDayLightSavingTime ? TIMEZONE_OFFSET_DAY_SAVING_LIGHT : TIMEZONE_OFFSET);

            return beijingTime.AddHours(offsetHours);
        }

        public static DateTime AMESNow
        {
            get
            {
                return BeijingTimeToUsDateTime(DateTime.Now);
            }
        }
        public static bool IsNowAMESDayLightSavingTime
        {
            get
            {
                return DateTime.UtcNow > DayLightSavingStartTimeUtc
                    && DateTime.UtcNow < DayLightSavingEndTimeUtc;
            }
        }
        /// <summary>
        /// 夏令时开始时间
        /// </summary>
        static DateTime DayLightSavingStartTimeUtc
        {
            get
            {
                if (_thisYearDaylightSavingTimeStart.Year != DateTime.Now.Year)
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, 3, 8, 0, 0, 0);
                    while (temp.DayOfWeek != DayOfWeek.Sunday)
                    {
                        temp = temp.AddDays(1);
                    }
                    _thisYearDaylightSavingTimeStart = temp.AddHours(TIMEZONE_OFFSET);
                }

                return _thisYearDaylightSavingTimeStart;
            }
        }
        /// <summary>
        /// 夏令时结束时间
        /// </summary>
        static DateTime DayLightSavingEndTimeUtc
        {
            get
            {
                if (_thisYearDaylightSavingTimeEnd.Year != DateTime.Now.Year)
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, 11, 1, 0, 0, 0);
                    while (temp.DayOfWeek != DayOfWeek.Sunday)
                    {
                        temp = temp.AddDays(1);
                    }
                    _thisYearDaylightSavingTimeEnd = temp.AddHours(TIMEZONE_OFFSET_DAY_SAVING_LIGHT);
                }
                return _thisYearDaylightSavingTimeEnd;
            }
        }
    }
    
}
