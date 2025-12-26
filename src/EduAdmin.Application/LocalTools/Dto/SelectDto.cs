using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.LocalTools.Dto
{
    public class SelectDto<T>
    {
        public T Value;
        public string Label;

        public SelectDto()
        {
        }
        public SelectDto(T label)
        {
            Value = label;
            Label = label.ToString();
        }
        public SelectDto(T value, string label)
        {
            Value = value;
            Label = label;
        }
    }

    public class MuchSelectDto<T>
    {
        public T Value;
        public string Label;
        public List<SelectDto<T>> Children;

        public MuchSelectDto(T label)
        {
            Value = label;
            Label = label.ToString();
            Children = new List<SelectDto<T>>();
        }
        public MuchSelectDto(T value, string label)
        {
            Value = value;
            Label = label;
            Children = new List<SelectDto<T>>();
        }
    }
    //public class SelectMuchDto<T>
    //{
    //    public T Value;
    //    public string Label;
    //    public List<SelectMuchDto<T>> Children;

    //    public SelectMuchDto()
    //    {
    //    }
    //    public SelectMuchDto(T label)
    //    {
    //        Value = label;
    //        Label = label.ToString();
    //    }
    //    public SelectMuchDto(T value, string label)
    //    {
    //        Value = value;
    //        Label = label;
    //    }
    //    public SelectMuchDto(T value, string label, List<SelectMuchDto<T>> children) : this(value, label)
    //    {
    //        Children = children;
    //    }
    //}
}
