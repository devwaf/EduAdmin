using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EduAdmin.LocalTools
{
    /// <summary>
    /// 复制对象通用类（高效率） https://www.cnblogs.com/lsgsanxiao/p/8205096.html
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public static class TransExpV2<TIn, TOut>
    {
        //使用方法 Test test = TransExpV2<Test, Test>.Trans(s);
        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }
        /// <summary>
        /// 单个对象复制
        /// </summary>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Trans(TIn tIn)
        {
            return cache(tIn);
        }
        /// <summary>
        /// 多个对象复制
        /// </summary>
        /// <param name="tIns"></param>
        /// <returns></returns>
        public static List<TOut> TransList(List<TIn> tIns)
        {
            List<TOut> res = new List<TOut>();
            if (tIns == null)
                return res;
            foreach (var tIn in tIns)
                res.Add(cache(tIn));
            return res;
        }

    }
}
