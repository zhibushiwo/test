using System;
using System.Collections.Generic;
using System.Text;

namespace Express.Common.SiteOperate
{
    public class TimeParser
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        #region 返回某年某月最后一天
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region 返回时间差
        /// <summary>
        /// 返回时间差，若大于1天则返回date1的月日
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >=1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion


        /// <summary>
        /// 检测两个时间段是否有重合
        /// </summary>
        /// <param name="A1"></param>
        /// <param name="A2"></param>
        /// <param name="B1"></param>
        /// <param name="B2"></param>
        /// <returns></returns>
        public static bool bool_ExistDuring(DateTime A1, DateTime A2, DateTime B1, DateTime B2)
        {
            bool flag = false;

            if (A1 < B2 && B1 < A2)
            {
                flag = true;
            }

            return flag;

        }


        /// <summary>
        /// 返回 与另一个时间段重合的分钟
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double GetDuringSpan(DateTime startTime1, DateTime endTime1, DateTime startTime2, DateTime endTime2)
        {
            double overlapMinute = 0;
            if (!bool_ExistDuring(startTime1,endTime1,startTime2,endTime2))
                return 0;


            //情况一
            //    |-------startTime1----------------------endTime1-----------|
            //    |----------------startTime2-----endTime2-------------------|
            if (startTime1 <= startTime2 && endTime1 >= endTime2)
            {
                System.TimeSpan diff1 = endTime2 - startTime1;
                System.TimeSpan diff2 = endTime1 - startTime2;
                System.TimeSpan diff3 = endTime1 - startTime1;
                overlapMinute = diff1.TotalMinutes + diff2.TotalMinutes - diff3.TotalMinutes;
            }

            //情况二
            //    |---------------startTime1-----endTime1-------------------|
            //    |--------startTime2--------------------endTime2-----------|
            else if (startTime1 >= startTime2 && endTime1 <= endTime2)
            {

                System.TimeSpan diff1 = endTime1 - startTime2;
                System.TimeSpan diff2 = endTime2 - startTime1;
                System.TimeSpan diff3 = endTime2 - startTime2;
                overlapMinute = diff1.TotalMinutes + diff2.TotalMinutes - diff3.TotalMinutes;

            }

            //情况三
            //    |-------------------------startTime1----------endTime1--------|
            //    |--------------startTime2-----------endTime2------------------|
            else if (startTime1 >= startTime2 && endTime1 >= endTime2)
            {
                System.TimeSpan diff1 = endTime2 - startTime1;
                overlapMinute = diff1.TotalMinutes;

            }

            //情况四
            //    |--------------startTime1----------endTime1-------------------|
            //    |-----------------------startTime2----------endTime2----------|
            else if (startTime1 <= startTime2 && endTime1 <= endTime2)
            {
                System.TimeSpan diff1 = endTime1 - startTime2;
                overlapMinute = diff1.TotalMinutes;


            }
            return overlapMinute;
        }
    }
}
