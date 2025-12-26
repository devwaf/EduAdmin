using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EduAdmin.LocalTools.Dto
{
    /// <summary>
    /// 键值对对象
    /// </summary>
    public class K_V_Dto
    {
        /// <summary>
        /// 键
        /// </summary>
        [Description("键")]
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Description("值")]
        public string Value { get; set; }

        public K_V_Dto()
        {
        }

        public K_V_Dto(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    /// <summary>
    /// 键值对对象
    /// </summary>
    public class K_V_Dto<T>
    {
        /// <summary>
        /// 键
        /// </summary>
        [Description("键")]
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Description("值")]
        public T Value { get; set; }

        public K_V_Dto()
        {
        }

        public K_V_Dto(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}
