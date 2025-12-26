using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.LocalTools
{
    /// <summary>
    /// 对象去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        //对象去重使用
        //.Distinct(new LambdaComparer<MuchSelect>((a, b) => a.Value == b.Value, obj => obj.ToString().GetHashCode())).ToList();
        private readonly Func<T, T, bool> _lambdaComparer;
        private readonly Func<T, int> _lambdaHash;
        public LambdaComparer(Func<T, T, bool> lambdaComparer)
        : this(lambdaComparer, EqualityComparer<T>.Default.GetHashCode)
        {
        }
        public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            if (lambdaComparer == null)
                throw new ArgumentNullException("lambdaComparer");
            if (lambdaHash == null)
                throw new ArgumentNullException("lambdaHash");
            _lambdaComparer = lambdaComparer;
            _lambdaHash = lambdaHash;
        }

        public bool Equals(T x, T y)
        {
            return _lambdaComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _lambdaHash(obj);
        }
    }
}
