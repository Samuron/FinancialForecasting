using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace BartoszMilewski
{
    public static class Module1
    {
        public static T Indentity<T>(T x)
        {
            return x;
        }

        public static Func<TA, TResult> Compose<TA, TB, TResult>(Func<TA, TB> f1, Func<TB, TResult> f2)
        {
            return x => f2(f1(x));
        }

        public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> f)
        {
            var dictionary = new Dictionary<T, TResult>();
            return x =>
            {
                if (dictionary.ContainsKey(x))
                    return dictionary[x];

                var value = f(x);
                dictionary.Add(x, value);
                return value;
            };
        }
    }
}