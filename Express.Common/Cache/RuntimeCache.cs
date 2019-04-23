using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Collections;

namespace Express.Common.WebCache
{
    /// <summary>
    /// 缓存的过期时间类型
    /// </summary>
    public enum CacheExpiresType
    {
        /// <summary>
        /// 绝对时间
        /// </summary>
        Absolute,

        /// <summary>
        /// 相对时间
        /// </summary>
        Sliding
    }

    /// <summary>
    /// 缓存时长
    /// </summary>
    public enum CacheTime
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = -1,

        /// <summary>
        /// 永不移除
        /// </summary>
        NotRemovable = 4,

        /// <summary>
        /// 长时间
        /// </summary>
        Long = 3,

        /// <summary>
        /// 正常时间
        /// </summary>
        Normal = 2,

        /// <summary>
        /// 短时间
        /// </summary>
        Short = 1,
    }

    /// <summary>
    /// 运行时缓存
    /// </summary>
    public class RuntimeCache
    {
        #region Add Methods
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Add<T>(string key, T value)
        {
            Add(key, value, CacheTime.Default, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间<see cref="OnlineJudge.Enums.CacheTime"/></param>
        public static void Add<T>(string key, T value, CacheTime cacheTime)
        {
            Add(key, value, cacheTime, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间<see cref="OnlineJudge.Enums.CacheTime"/></param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            Add(key, value, cacheTime, cacheExpiresType, null, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间<see cref="OnlineJudge.Enums.CacheTime"/></param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间<see cref="OnlineJudge.Enums.CacheTime"/></param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="cacheItemPriority">优先级</param>
        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间<see cref="OnlineJudge.Enums.CacheTime"/></param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="cacheItemPriority">优先级</param>
        /// <param name="callback">回调</param>
        public static void Add<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            DateTime absoluteExpiration = GetAbsoluteExpiration(cacheTime, cacheExpiresType);
            TimeSpan slidingExpiration = GetSlidingExpiration(cacheTime, cacheExpiresType);

            if (cacheTime == CacheTime.NotRemovable) cacheItemPriority = CacheItemPriority.NotRemovable;

            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, cacheItemPriority, callback);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(秒)</param>
        public static void Add<T>(string key, T value, int cacheTime)
        {
            Add(key, value, cacheTime, CacheExpiresType.Sliding, null, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(秒)</param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        public static void Add<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType)
        {
            Add(key, value, cacheTime, cacheExpiresType, null, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(秒)</param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        public static void Add<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(秒)</param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="cacheItemPriority">优先级</param>
        public static void Add<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            Add(key, value, cacheTime, cacheExpiresType, dependencies, cacheItemPriority, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">缓存时间(秒)</param>
        /// <param name="cacheExpiresType">缓存过期时间<see cref="OnlineJudge.Enums.CacheExpiresType"/></param>
        /// <param name="dependencies">缓存依赖</param>
        /// <param name="cacheItemPriority">优先级</param>
        /// <param name="callback">回调</param>
        public static void Add<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType, CacheDependency dependencies, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback callback)
        {
            DateTime absoluteExpiration;

            if (cacheExpiresType == CacheExpiresType.Sliding) absoluteExpiration = Cache.NoAbsoluteExpiration;
            else absoluteExpiration = DateTime.Now.AddSeconds(cacheTime);

            TimeSpan slidingExpiration;
            if (cacheExpiresType == CacheExpiresType.Absolute) slidingExpiration = Cache.NoSlidingExpiration;
            else slidingExpiration = TimeSpan.FromSeconds(cacheTime);


            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, cacheItemPriority, callback);
        }

        #endregion

        /// <summary>
        /// 返回指定的缓存，如果不存在将导致异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 尝试返回指定的缓存
        /// </summary>
        /// <typeparam name="T">缓存内容的类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="value">缓存的内容</param>
        /// <returns>是否存在这个缓存</returns>
        public static bool TryGetValue<T>(string key, out T value)
        {
            object temp = HttpRuntime.Cache[key];

            if (temp != null && temp is T)
            {
                value = (T)temp;
                return true;
            }

            value = default(T);
            return false;
        }
        /// <summary>
        /// 按开头搜索缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="default">默认值</param>
        /// <returns>缓存列表</returns>
        public static List<T> GetBySearch<T>(string key, T @default)
        {
            List<T> lst = new List<T>();
            List<string> keys = findKeyStartWith(key);
            if (keys != null)
            {
                foreach (string k in keys)
                {
                    T val;
                    TryGetValue(k, out val);
                    if (val == null) val = @default;
                    lst.Add(val);
                }
            }
            return lst;
        }

        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains<T>(string key)
        {
            object value = HttpRuntime.Cache[key];

            if (value == null)
                return false;

            else if (value is T)
                return true;

            return false;
        }

        /// <summary>
        /// 清除系统中所有缓存
        /// </summary>
        public static void Clear()
        {
            List<string> keys = new List<string>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                keys.Add(elem.Key.ToString());
            }

            foreach (string key in keys)
            {
                try
                {
                    HttpRuntime.Cache.Remove(key);
                }
                catch { }
            }
        }

        /// <summary>
        /// 移除指定的缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        public static void Remove(string key)
        {
            try
            {
                HttpRuntime.Cache.Remove(key);
            }
            catch { }
        }

        /// <summary>
        /// 移除指定开头的所有缓存
        /// </summary>
        /// <param name="startText"></param>
        public static void RemoveBySearch(string startText)
        {
            List<string> removeKeys = findKeyStartWith(startText);

            if (removeKeys == null) return;

            foreach (string removeKey in removeKeys)
            {
                try
                {
                    HttpRuntime.Cache.Remove(removeKey);
                }
                catch { }
            }
        }

        private static List<string> findKeyStartWith(string startText)
        {
            if (string.IsNullOrEmpty(startText)) return null;

            List<string> removeKeys = new List<string>();

            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                string cacheKey = elem.Key.ToString();

                if (cacheKey.StartsWith(startText, StringComparison.OrdinalIgnoreCase))
                    removeKeys.Add(cacheKey);
            }

            return removeKeys;
        }

        private static DateTime GetAbsoluteExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Sliding)
                return Cache.NoAbsoluteExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return DateTime.Now.AddMinutes(5);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return DateTime.Now.AddMinutes(10);
                case CacheTime.Long:
                    return DateTime.Now.AddMinutes(20);
            }
        }

        private static TimeSpan GetSlidingExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Absolute)
                return Cache.NoSlidingExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return TimeSpan.FromMinutes(5);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return TimeSpan.FromMinutes(10);
                case CacheTime.Long:
                    return TimeSpan.FromMinutes(20);
            }
        }
    }
}