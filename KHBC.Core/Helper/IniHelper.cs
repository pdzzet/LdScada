namespace KHBC.Core.Helper
{
    /// <summary>
    /// ini文件帮助类
    /// </summary>
    public class IniHelper
    {
        public IniHelper(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                System.IO.File.Create(filename);
            }
            IniFile(filename);
        }
        private string _fileName;
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           int nDefault,// 如果 Key 值没有找到，则返回缺省的值是多少
           string lpFileName
           );
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           string lpDefault,// 如果 Key 值没有找到，则返回缺省的字符串的地址
           System.Text.StringBuilder lpReturnedString,// 返回字符串的缓冲区地址
           int nSize,// 缓冲区的长度
           string lpFileName
           );
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           string lpString,// 要写的字符串地址
           string lpFileName
           );

        /// <summary>
        /// 载入INI文件
        /// </summary>
        /// <param name="filename">
        /// ini文件名
        /// </param>
        public void IniFile(string filename)
        {
            _fileName = filename;
        }

        /// <summary>
        /// 获得INI文件的某一键的数值
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="def">
        /// 该键名无值时的默认值
        /// </param>
        public int GetInt(string section, string key, int def)
        {
            var res = GetPrivateProfileInt(section, key, def, _fileName);
            if (res == def)
            {
                WriteInt(section, key, def);
            }
            return res;
        }

        /// <summary>
        /// 获得INI文件的某一键的字符串
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="def">
        /// 该键名无值时的默认值
        /// </param>
        public string GetString(string section, string key, string def)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(1024);
            var res = GetPrivateProfileString(section, key, def, temp, 1024, _fileName).ToString();
            if (res == def)
            {
                WriteString(section, key, def);
            }
            return res;
        }

        /// <summary>
        /// 设置INI文件的某一键的数值
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="iVal">
        /// INI文件的键值
        /// </param>
        public void WriteInt(string section, string key, int iVal)
        {
            WritePrivateProfileString(section, key, iVal.ToString(), _fileName);
        }

        /// <summary>
        /// 设置INI文件的某一键的字符串
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="strVal">
        /// INI文件的键值
        /// </param>
        public void WriteString(string section, string key, string strVal)
        {
            WritePrivateProfileString(section, key, strVal, _fileName);
        }

        /// <summary>
        /// 删除某一个键
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        public void DelKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, _fileName);
        }

        /// <summary>
        /// 删除某一个章节
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        public void DelSection(string section)
        {
            WritePrivateProfileString(section, null, null, _fileName);
        }

    }
}
