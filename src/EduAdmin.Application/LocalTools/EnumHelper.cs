using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EduAdmin.LocalTools
{
    /// <summary>
    /// 枚举处理类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetEnumName<TEnum>(int num) where TEnum: Enum
        {
            return Enum.GetName(typeof(TEnum), num);
        }
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="num">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<TEnum>(this TEnum num) where TEnum : Enum
        {
            return Enum.GetName(typeof(TEnum), num);
        }
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(int num) where TEnum : Enum
        {
            FieldInfo fieldInfo = typeof(TEnum).GetField(Enum.GetName(typeof(TEnum), num));
            DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return attr.Description;
        }
        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="num">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(this TEnum num) where TEnum : Enum
        {
            FieldInfo fieldInfo = num.GetType().GetField(Enum.GetName(typeof(TEnum), num));
            DescriptionAttribute attr =  Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return attr.Description;
        }

        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<TEnum>() where TEnum : Enum
        {
            return Enum.GetNames(typeof(TEnum));
        }
        /// <summary>
        /// 将枚举转换成字典集合
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnumDic<TEnum>() where TEnum : Enum
        {
            Dictionary<string, int> resultList = new Dictionary<string, int>();
            Type type = typeof(TEnum);
            var strList = GetNamesArr<TEnum>().ToList();
            foreach (string key in strList)
            {
                string val = Enum.Format(type, Enum.Parse(type, key), "d");
                resultList.Add(key, int.Parse(val));
            }
            return resultList;
        }

        /// <summary>
        /// 将枚举转换成K_V_Dto集合
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns></returns>
        public static List<K_V_Dto<int>> GetEnumDescriptionList<TEnum>() where TEnum : Enum
        {
            List<K_V_Dto<int>> dic = new List<K_V_Dto<int>>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(new K_V_Dto<int>(GetEnumDescription((TEnum)item), (int)item));
            }
            return dic;
        }
        /// <summary>
        /// 将枚举转换成字典集合
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnumDescriptionDic<TEnum>() where TEnum : Enum
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(GetEnumDescription((TEnum)item), (int)item);
            }
            return dic;
        }
        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDic<TEnum>() where TEnum : Enum
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }
            return dic;
        }
    }
}
