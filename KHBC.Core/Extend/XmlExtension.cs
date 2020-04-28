using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KHBC.Core.Extend
{
    /// <summary>
    /// json相关扩展
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// obj转xmlstr格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXmlStr(this object obj)
        {
            string str = string.Empty;
            if (null == obj)
                return str;
            try
            {
                var ms = new MemoryStream();
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                StreamReader sr = new StreamReader(ms);
                ms.Position = 0;
                str = sr.ReadToEnd();
                sr.Dispose();
                ms.Dispose();
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }
        /// <summary>
        /// xmlstr转model(出错返回default)
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="str">xml字符串</param>
        /// <returns></returns>
        public static T XmlToModel<T>(this string str) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(str))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// xmlstr转model(出错抛异常)
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="str">xml字符串</param>
        /// <returns></returns>
        public static T XmlToObj<T>(this string str) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(str))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将XML转换成实体对象异常", ex);
            }
        }

        /// <summary>
        /// 对象保存为xml文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveToXMLFile(this object obj, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(obj.ToXmlStr());
                    sw.Flush();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 加载xml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Result<T> LoadXMLFile<T>(string path) where T : class
        {
            try
            {
                var str = File.ReadAllText(path, Encoding.UTF8);
                var data = str.XmlToObj<T>();
                return Result<T>.Success(data);
            }
            catch (Exception e)
            {
                return Result<T>.Fail(e.Message);
            }
        }

        /// <summary>
        /// 保存XML文件
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveToFile(XmlDocument xmlDoc, string path)
        {
            try
            {
                xmlDoc.Save(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
