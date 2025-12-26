using EduAdmin.LocalTools.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EduAdmin.LocalTools
{
    /// <summary>
    /// 动态类转换辅助方法 （只支持同属性名、同类型之间的转换）
    /// </summary>
    public class DynamicClassTransformation
    {
        /// <summary>
        /// 把字符串首字母转为小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChangeFirstToLower(string str)
        {
            return str[0].ToString().ToLower() + str.Substring(1);
        }
        /// <summary>
        /// 把字符串首字母转为大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChangeFirstToUpper(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
    }
    /// <summary>
    /// object、List、Dictionary之间的转换拓展
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 普通对象转换成动态字典对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> ToDictionary(this object obj)
        {
            Dictionary<string, dynamic> Keys = new Dictionary<string, dynamic>();
            var properties = obj.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = info.GetValue(obj, null);
                Keys.Add(DynamicClassTransformation.ChangeFirstToLower(info.Name), value);
            }
            return Keys;
        }
        /// <summary>
        /// 普通对象转换成动态字典对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> ToDictionaryIgnoreList(this object obj)
        {
            Dictionary<string, dynamic> Keys = new Dictionary<string, dynamic>();
            var properties = obj.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                // List 属性不处理
                if (info.PropertyType.Name != "List`1")
                {
                    var value = info.GetValue(obj, null);
                    Keys.Add(DynamicClassTransformation.ChangeFirstToLower(info.Name), value);
                }
            }
            return Keys;
        }
        /// <summary>
        /// 普通对象转换成string字典对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToStringDictionary(this object obj)
        {
            var res = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = info.GetValue(obj, null);
                if (value == null)
                    continue;
                res.Add(info.Name, value.ToString());
            }
            return res;
        }

        /// <summary>
        /// 集合转换成动态字典对象
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<Dictionary<string, dynamic>> ToListDictionary<T>(this List<T> objs)
        {
            List<Dictionary<string, dynamic>> list = new List<Dictionary<string, dynamic>>();
            foreach (var obj in objs)
                list.Add(obj.ToDictionary());
            return list;
        }
        /// <summary>
        /// 集合转换成string字典对象
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ToListStringDictionary<T>(this List<T> objs)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (var obj in objs)
                list.Add(ToStringDictionary(obj));
            return list;
        }

        /// <summary>
        /// 将对象及注释转换成键值对（属性及其注释） 使用见 ExcelHelper.CreateExcel
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<K_V_Dto> ToKVDto<T>(this T obj)
        {
            var properties = obj.GetType().GetProperties();
            List<K_V_Dto> res = new List<K_V_Dto>();
            foreach (PropertyInfo property in properties)
            {
                object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objs.Length > 0)
                    res.Add(new K_V_Dto(property.Name, ((DescriptionAttribute)objs[0]).Description));
            }
            return res;
        }
        /// <summary>
        /// 将对象及注释转换成键值对(复杂的)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<K_V_Dto> ToDescriptionsForExcel<T>(this T obj) where T : K_V_ListDto, new()
        {
            if (obj == null)
                obj = new T();
            var properties = obj.GetType().GetProperties()
                // 属性根据 order 排序
                .OrderBy(c => c.GetCustomAttributes(typeof(DisplayAttribute), true)
                                .Cast<DisplayAttribute>()
                                .Select(a => a.Order)
                                .FirstOrDefault()
                        );
            List<K_V_Dto> res = new List<K_V_Dto>();
            foreach (PropertyInfo property in properties)
            {
                //Console.WriteLine($"属性名称：{property.Name}，类型：{property.PropertyType.Name}，值：{property.GetValue(obj)}");
                if (property.PropertyType.Name == "List`1")
                {
                    res.AddRange(obj.GetDescriptionsForExcel(property.Name));
                }
                else
                {
                    object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (objs.Length > 0)
                        res.Add(new K_V_Dto(property.Name, ((DescriptionAttribute)objs[0]).Description));
                }
            }
            return res;
        }
        /// <summary>
        /// 将对象及注释转换成键值对(复杂的)(指定列)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="qualifiedColumn">限定列</param>
        /// <returns></returns>
        public static List<K_V_Dto> ToDescriptionsForExcel<T>(this T obj,List<string> qualifiedColumn) where T : K_V_ListDto
        {
            var properties = obj.GetType().GetProperties()
                // 属性根据 order 排序
                .OrderBy(c => c.GetCustomAttributes(typeof(DisplayAttribute), true)
                                .Cast<DisplayAttribute>()
                                .Select(a => a.Order)
                                .FirstOrDefault()
                        );
            List<K_V_Dto> res = new List<K_V_Dto>();
            if (qualifiedColumn == null || qualifiedColumn.Count == 0)
                return res;
            foreach (PropertyInfo property in properties)
            {
                //Console.WriteLine($"属性名称：{property.Name}，类型：{property.PropertyType.Name}，值：{property.GetValue(obj)}");
                if (property.PropertyType.Name == "List`1")
                {
                    res.AddRange(obj.GetDescriptionsForExcel(property.Name));
                }
                else if (qualifiedColumn.Contains(property.Name))
                {
                    object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (objs.Length > 0)
                        res.Add(new K_V_Dto(property.Name, ((DescriptionAttribute)objs[0]).Description));
                }
            }
            return res;
        }
        /// <summary>
        /// 集合转换成string字典对象
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<List<K_V_Dto<List<string>>>> GetListValuesForExcel<T>(this List<T> objs) where T : K_V_ListDto,new ()
        {
            List<List<K_V_Dto<List<string>>>> list = new List<List<K_V_Dto<List<string>>>>();
            var properties = new T().GetType().GetProperties();
            foreach (var obj in objs)
            {
                List<K_V_Dto<List<string>>> res = new List<K_V_Dto<List<string>>>();
                foreach (PropertyInfo property in properties)
                {
                    //Console.WriteLine($"属性名称：{property.Name}，类型：{property.PropertyType.Name}，值：{property.GetValue(obj)}");
                    if (property.PropertyType.Name == "List`1")
                    {
                        res.AddRange(obj.GetValuesForExcel(property.Name));
                    }
                    else
                    {
                        object[] item_objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (item_objs.Length > 0)
                        {
                            var value = property.GetValue(obj);
                            if (value == null)
                                value = "";
                            res.Add(new K_V_Dto<List<string>>(property.Name, new List<string>() { value.ToString() }));
                        }
                    }
                }
                list.Add(res);
            }
            return list;
        }
        /// <summary>
        /// 获得指定的横向的最多(单项有效)
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static T GetHorizontalMostItem<T>(this List<T> objs) where T : K_V_ListDto
        {
            if (objs == null || objs.Count == 0)
                return default;
            if (objs.Count == 1)
                return objs[0];
            return objs.OrderByDescending(c => c.GetOrderNum()).First();
        }


        /// <summary>
        /// 将字典属性写入对象(注意：字典key首字母需要小写)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static T DictionarySetInObject<T>(this Dictionary<string, dynamic> dictionary) where T : class, new()
        {
            T t = new T();
            var properties = t.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = dictionary.GetValueOrDefault(DynamicClassTransformation.ChangeFirstToLower(info.Name));
                info.SetValue(t, value);
            }
            return t;
        }

        /// <summary>
        /// 合并字典
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static void AddDictionary(this Dictionary<string, dynamic> dictionary, Dictionary<string, dynamic> info)
        {
            foreach (var item in info) 
            {
                dictionary.Add(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 对象属性写入另一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T SetInObject<T>(this object obj) where T : class, new()
        {
            var dictionary = obj.ToDictionary();
            return dictionary.DictionarySetInObject<T>();
        }
        /// <summary>
        /// 集合对象的转换（只支持同属性名、同类型之间的转换）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<K> SetInList<T, K>(this List<T> list) where K : class, new()
        {
            List<K> ks = new List<K>();
            foreach (var item in list)
            {
                K k = item.SetInObject<K>();
                ks.Add(k);
            }
            return ks;
        }
        /// <summary>
        /// 将字典属性写入对象(注意：字典key首字母需要小写)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> DictionarySetInList<T>(this List<Dictionary<string, dynamic>> list) where T : class, new()
        {
            List<T> ts = new List<T>();
            foreach (var item in list)
            {
                T t = item.DictionarySetInObject<T>();
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 获得所有的Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static List<string> GetKeys<T>(this Dictionary<string, T> dictionary)
        {
            List<string> keyList = new List<string>();
            //List<string> valueList = new ();
            foreach (KeyValuePair<string, T> kvp in dictionary)
            {
                keyList.Add(kvp.Key);
                //valueList.Add(dictionary[kvp.Key]);
            }
            return keyList;
        }
    }
    public interface K_V_ListDto
    {
        /// <summary>
        /// 获得对象的属性名 与对应的描述
        /// </summary>
        /// <returns></returns>
        List<K_V_Dto> GetDescriptionsForExcel(string propertyName);
        /// <summary>
        /// 获得对象的属性名 与对应的属性值
        /// </summary>
        /// <returns></returns>
        List<K_V_Dto<List<string>>> GetValuesForExcel(string propertyName);
        /// <summary>
        /// 得到用于横向排序的列
        /// </summary>
        /// <returns></returns>
        int GetOrderNum();
    }
}
