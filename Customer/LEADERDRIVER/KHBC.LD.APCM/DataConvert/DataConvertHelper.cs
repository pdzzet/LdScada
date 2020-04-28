using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.LD.APCM
{
    internal class DataConvertHelper
    {
        /// <summary>
        /// byte[]数组转二进制字符数组 char[]
        /// </summary>
        /// <param name="bts">byte[]</param>
        /// <returns></returns>
        public static char[] BytesToCharArrayb2(byte[] bts)
        {
            string b2str = "";
            string hexstr = BitConverter.ToString(bts, 0);
           
            string[] hexstrs = hexstr.Split('-');
            foreach (string str in hexstrs)
            {
                b2str += HexString2BinString(str);
            }
            b2str = b2str.Replace(" ", "");
            //反转字符串
            return b2str.Reverse().ToArray();
        }
        //16进制字符串转二进制字符串
        static string HexString2BinString(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4} ", v2);
            }
            return result;
        }
    }
}
