using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.Modbus.Enums
{
    public enum ActionTypes
    {
        /// <summary>
        /// 无plc操作
        /// </summary>
        None,
        /// <summary>
        /// 读取数据
        /// </summary>
        Read,
        /// <summary>
        /// 读取并且比较数据
        /// </summary>
        Compare,
        /// <summary>
        /// 写入数据
        /// </summary>
        Write,
        /// <summary>
        /// 监听数据，等待数据和设置值一样
        /// </summary>
        Wait
    }

    public enum PlcDataType
    {
        /// <summary>
        /// short类型
        /// </summary>
        BIT,
        /// <summary>
        /// string 型
        /// </summary>
        WORD,
        /// <summary>
        /// string 型
        /// </summary>
        DWORD,
        /// <summary>
        /// 小数
        /// </summary>
        FLOAT
    }

}
