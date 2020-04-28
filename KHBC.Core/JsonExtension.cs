using System;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KHBC.Core
{
    /// <summary>
    /// json相关扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// obj转json格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonStr(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            if (obj is string)
            {
                return obj.ToString();
            }

            JsonSerializer serializer = new JsonSerializer();
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }
        /// <summary>
        /// json字符串转model(不抛异常)
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="str">json字符串</param>
        /// <returns></returns>
        public static T JsonToModel<T>(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// json字符串转jobject
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static JObject ToJObject(this string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return new JObject();
            }
            var obj = JObject.Parse(jsonStr);
            if (obj == null)
            {
                return new JObject();
            }
            return obj;
        }

        /// <summary>
        /// json字符串转model(抛异常)
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="str">json字符串</param>
        /// <returns></returns>
        public static T JsonToObj<T>(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return System.Activator.CreateInstance<T>();
            }
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 加载文件返回jobject
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static JObject LoadJsonFile(string filename)
        {
            if (File.Exists(filename))
            {
                var str = File.ReadAllText(filename, Encoding.UTF8);
                return str.ToJObject();
            }
            else
            {
                return new JObject();
            }
        }

        /// <summary>
        /// 保存到json文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void SaveToFile(this JObject obj, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.Write(obj.ToJsonStr());
                sw.Flush();
            }
        }

        /// <summary>
        /// 获取json对象的key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetKey<T>(this JObject obj, string key, T def)
        {
            if (obj.Properties().Any(x => x.Name == key))
            {
                var val = obj.Property(key)?.Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(val))
                {
                    obj.Property(key).Value = JToken.FromObject(def);
                    return def;
                }
                else
                {
                    return val.JsonToModel<T>();
                }
            }
            obj.Add(key, JToken.FromObject(def));
            return def;
        }

        /// <summary>
        /// 获取默认数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonFile"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetDefKey<T>(string jsonFile, string key, T def)
        {
            var obj = LoadJsonFile(jsonFile);
            if (obj.Properties().Any(x => x.Name == key))
            {
                var val = obj.Property(key)?.Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(val))
                {
                    obj.Property(key).Value = JToken.FromObject(def);
                    return def;
                }
                else
                    return val.JsonToModel<T>();
            }
            else
            {
                obj.Add(key, JToken.FromObject(def));
                obj.SaveToFile(jsonFile);
                return def;
            }

        }
        
        //public static string GetValue(string jsonStr, string key)
        //{
        //    if (obj.Properties().Any(x => x.Name == key))
        //    {
        //        var val = obj.Property(key)?.Value.ToString() ?? string.Empty;
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            obj.Property(key).Value = JToken.FromObject(def);
        //            return def;
        //        }
        //        else
        //            return val.JsonToModel<T>();
        //    }
        //}
        /// <summary>
        /// 设置json 对象的key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetKey<T>(this JObject obj, string key, T value)
        {
            if (obj.Properties().Any(x => x.Name == key))
            {
                obj.Property(key).Value = JToken.FromObject(value);
            }
            else
            {
                obj.Add(key, JToken.FromObject(value));

            }
        }
    }
}
