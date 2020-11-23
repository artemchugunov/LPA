using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LPA.Client
{
    public static class Extensions
    {
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, typeof(T), null);
        }

        public static T FromJson<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static string ToUrl(this string s)
        {
            return App.UrlPrefix + s;
        }
    }
}
