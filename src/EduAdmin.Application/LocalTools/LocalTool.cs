using Microsoft.International.Converters.PinYinConverter;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EduAdmin.LocalTools
{
    public class LocalTool
    {
        #region 将汉字转化成首拼 
        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFristOne(string name)
        {
            string strTemp = "";
            int iLen = name.Length;

            for (int i = 0; i <= iLen - 1; i++)
            {
                if (!Regex.IsMatch(name.Substring(i, 1), @"[\u4E00-\u9FA5]+$"))
                {
                    continue;
                }
                strTemp += GetCharSpellCode(name.Substring(i, 1));
            }
            return strTemp;
        }
        /// <summary>
        /// 单个字符转化
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetCharSpellCode(string c)
        {
            char Initial = Convert.ToChar(c.Substring(0, 1));//得出首字母
            ChineseChar chn = new ChineseChar(Initial);
            string Initial1 = chn.Pinyins[0].Substring(0, 1);
            return Initial1;
        }
        #endregion
        #region 创建图片
        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="path"></param>
        /// <param name="timestamp"></param>
        public static string CreateImage(string imageName, string path, string timestamp)
        {
            string name = imageName + "_" + timestamp + ".jpg";
            SKBitmap bmp = new SKBitmap(128, 128);
            string str = imageName.Substring(imageName.Length - 2, 2);
            using (SKCanvas canvas = new SKCanvas(bmp))
            {
                SKColor[] color = new SKColor[] { new SKColor(161, 136, 127), new SKColor(255, 138, 31) };
                Random r = new Random();
                int num = r.Next(color.Length);
                canvas.DrawColor(color[num]); // colors是图片背景颜色集合，这里代码就不贴出来了，随机找一个
                using (SKPaint sKPaint = new SKPaint())
                {
                    sKPaint.Color = SKColors.White;//字体颜色
                    sKPaint.TextSize = 50;//字体大小
                    sKPaint.IsAntialias = true;//开启抗锯齿                
                    sKPaint.Typeface = SKTypeface.FromFamilyName("宋体", SKTypefaceStyle.Normal);//字体
                    SKRect size = new SKRect();
                    sKPaint.MeasureText(str, ref size);//计算文字宽度以及高度
                    float temp = (128 - size.Size.Width) / 2;
                    float temp1 = (128 - size.Size.Height) / 2;
                    canvas.DrawText(str, temp, temp1 - size.Top, sKPaint);//画文字
                }
                //保存成图片文件
                using (SKImage img = SKImage.FromBitmap(bmp))
                {
                    using (SKData p = img.Encode(SKEncodedImageFormat.Jpeg, 100))
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var stream = File.Create(Path.Combine(path, name)))
                        {
                            stream.Write(p.ToArray(), 0, p.ToArray().Length);
                        }
                    }
                }
            }
            return Path.Combine(path, name);
        }
        #endregion
        #region 字符数组 <=> 字符串
        /// <summary>
        /// 数组转换成单个字符串
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string ListToString1(List<string> strs)
        {
            string res = "";
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(res))
                    res = str;
                else
                    res = res + ";" + str;
            }
            return res;
        }
        /// <summary>
        /// 单个字符串转换成数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> StringToList1(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] arr = str.Split(';');//分割字符串
                return new List<string>(arr);
            }
            else
                return new List<string>();
        }
        #endregion
        #region 字符串转数字
        /// <summary>
        /// 字符串转数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double StringToNum(string str)
        {
            if (string.IsNullOrEmpty(str))//字符为空
            {
                str = "0";
                return Convert.ToDouble(str);
            }
            else if (!str.Contains("."))//字符不为空且不包含小数点
            {
                string strnum = Regex.Replace(str, @"[^\d]", "");
                //string strnum = Regex.Replace(str, @"[^0-9]+", "");
                if (string.IsNullOrEmpty(strnum))
                {
                    strnum = "0";
                }
                return Convert.ToDouble(strnum);
            }
            else
            {//字符不为空且包含小数点
                string strnum = Regex.Replace(str, @"[^\d.\d]", "");
                return Convert.ToDouble(strnum);
            }
        }
        #endregion
        #region 时间戳
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 0, 0, 0);
            double timeStamp = Convert.ToDouble((dt - dateStart).TotalSeconds) * 1000;
            return (long)timeStamp;
        }
        #endregion
        /// <summary>
        /// 字符串转float
        /// </summary>
        /// <param name="doubleStr">要进行转换的字符串</param>
        /// <returns></returns>
        public static dynamic ParseDouble(string doubleStr)
        {
            double parseDouble;
            if (double.TryParse(doubleStr, out parseDouble))
                return parseDouble;
            return doubleStr;
        }
        #region 获得AppSettings的值
        /// <summary>
        /// 获得AppSettings的值
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetAppSettings(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
        #endregion
        #region 文件大小
        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetFileSize(long bytes)
        {
            if (bytes < 1000)
                return bytes + "bytes";
            if (bytes / 1024.0 < 1000)
                return Math.Round(bytes / 1024.0, 2) + "KB";
            if (bytes / 1024.0 / 1024.0 < 1000)
                return Math.Round(bytes / 1024.0 / 1024.0, 2) + "MB";
            if (bytes / 1024.0 / 1024.0 / 1024.0 < 1000)
                return Math.Round(bytes / 1024.0 / 1024.0 / 1024.0, 2) + "GB";
            return Math.Round(bytes / 1024.0 / 1024.0 / 1024.0 / 1024.0, 2) + "TB";
        }
        #endregion
        #region 文件类型统一转换
        /// <summary>
        /// 文件类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FileTypeTFM(string type)
        {
            string res = type.ToLower();
            if (res == ".pdf")
            {
                res = "PDF文档";
            }
            else if (res == ".ppt" || res == ".pptx")
            {
                res = "PPT文档";
            }
            else if (res == ".xls" || res == ".xlsx")
            {
                res = "Excel文档";
            }
            else if (res == ".doc" || res == ".docx")
            {
                res = "word文档";
            }
            else if (res == ".jpg" || res == ".jpeg" || res == ".png" || res == ".gif" || res == ".tiff" || res == ".svg")
            {
                res = "图片";
            }
            else if (res == ".mp4")
            {
                res = "视频";
            }
            else if (res == "文件夹")
            {
                res = "文件夹";
            }
            else
            {
                res = "其他";
            }
            return res;
        }
        #endregion
        #region 时间字符串输出(前端显示部分)
        /// <summary>
        /// 时间字符串输出统一(type默认0)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TimeFormatStr(DateTime time, int type)
        {
            switch (type)
            {
                case 1:
                    return time.ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return time.ToString("yyyy.MM.dd");
                case 3:
                    return time.ToString("yyyy-MM-dd");
                case 4:
                    return time.ToString("yyyy年MM月dd日");
                case 5:
                    return time.ToString("yyyy-MM-dd HH:mm");
                case 6:
                    return time.ToString("yyyy.MM.dd HH.mm");
                default:
                    return time.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 拼接两个时间字符串(type默认0)
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TwoTimeFormatStr(DateTime time1, DateTime time2, int type)
        {
            return TimeFormatStr(time1, 2) + "-" + TimeFormatStr(time2, 2);
        }
        #endregion
        #region 根据日期判断当天是当月的第几周
        /// <summary>
        /// 根据日期判断当天是当月的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sundayStart">一周的第一天是否从周日开始</param>
        /// <returns></returns>
        public static int WeekOfMonth(DateTime dateTime, bool sundayStart = true)
        {
            var result = 1;
            if (dateTime.Day == 1)
            {
                return result;
            }

            //得到本月第一天是周几
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            if (!sundayStart && dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }
            result = (int)Math.Ceiling((double)(dateTime.Day + dayOfWeek - (sundayStart ? 0 : 1)) / 7);
            return result;
        }
        #endregion
        #region 时间进度、差异
        /// <summary>
        /// 计算当前时间进度（以周为单位）
        /// </summary>
        /// <returns></returns>
        public static decimal GetPercentInTime()
        {
            var year = DateTime.Now.Year;
            //计算当前的天数（星期天是第一天）
            var daysoff = (int)(7 - DateTime.Now.DayOfWeek - 1);
            var days = DateTime.Now.DayOfYear + daysoff;
            var nowWeek = days / 7 + 1;

            //计算今年总天数
            var lastDay = new DateTime(year, 12, 31);
            var allDayOff = (int)(7 - lastDay.DayOfWeek - 1);
            var allDays = lastDay.DayOfYear + allDayOff;
            var allWeeks = allDays / 7 + 1;
            return (decimal)nowWeek / allWeeks;
        }
        /// <summary>
        /// 差异百分比
        /// </summary>
        /// <param name="AddedUpAmount"></param>
        /// <param name="PlanAmount"></param>
        /// <returns></returns>
        public static decimal GetAmountPercent(decimal AddedUpAmount, decimal PlanAmount)
        {
            decimal result = 0;
            if (PlanAmount == 0)
            {
                return result;
            }
            //投资金额百分比
            result = AddedUpAmount / PlanAmount;
            return result;
        }

        #endregion 时间进度、差异
        #region 判断两个时间是否为同一天
        /// <summary>
        /// 判断两个时间是否为同一天
        /// </summary>
        /// <returns></returns>
        public static bool IsOneDay(DateTime one, DateTime two)
        {
            return one.ToString("yyyy-MM-dd") == two.ToString("yyyy-MM-dd");
        }
        #endregion
        #region 显示模糊时间
        /// <summary>
        /// 显示模糊时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string TimeToString(DateTime? dt)
        {
            if (dt == null)
            {
                return null;
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - dt);
            if (span.TotalDays > 3)
            {
                return $"{dt:yyyy-MM-dd HH:mm:ss}";
            }

            if (span.TotalDays >= 2)
            {
                return "前天";
            }

            if (span.TotalDays >= 1)
            {
                return "昨天";
            }

            if (span.TotalHours > 1)
            {
                return $"{(int)Math.Floor(span.TotalHours)}小时前";
            }

            if (span.TotalMinutes > 1)
            {
                return $"{(int)Math.Floor(span.TotalMinutes)}分钟前";
            }

            if (span.TotalSeconds >= 10)
            {
                return $"{(int)Math.Floor(span.TotalSeconds)}秒前";
            }

            return "刚刚";
        }
        #endregion
    }
}
