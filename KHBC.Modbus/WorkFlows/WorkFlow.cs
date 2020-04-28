using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using KHBC.Core.Extend;
using KHBC.Modbus.Enums;

namespace KHBC.Modbus.WorkFlows
{
    /// <summary>
    /// 工作流配置
    /// </summary>
    [XmlRoot("WorkFlows")]
    [Serializable]
    public class WorkFolwConfig
    {
        public WorkFlow[] WorkFlows { get; set; }
    }

    /// <summary>
    /// 工作流程对象
    /// </summary>
    [Serializable]
    public class WorkFlow
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// plc超时时间(s)
        /// </summary>
        [XmlAttribute]
        public int PLCTimeOut { get; set; }
        /// <summary>
        /// Action超时时间
        /// </summary>
        [XmlAttribute]
        public int ActionTimeOut { get; set; }
        /// <summary>
        /// Plc设备名
        /// </summary>
        [XmlAttribute]
        public string PlcDeviceName { get; set; }
        /// <summary>
        /// 逻辑块组
        /// </summary>
        [XmlElement("Block")]
        public List<FlowBlock> Blocks { get; set; }
        /// <summary>
        /// 监听组
        /// </summary>
        [XmlElement("Listen")]
        public List<PlcListen> Listens { get; set; }
        /// <summary>
        /// 是否使用多线程
        /// </summary>
        [XmlAttribute]
        public bool MultiThread { get; set; }
    }


    /// <summary>
    /// plc监听
    /// </summary>
    [Serializable]
    public class PlcListen
    {
        /// <summary>
        /// 监听名字
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// plc开始地址名字
        /// </summary>
        [XmlAttribute]
        public string StartAddressName { get; set; }
        /// <summary>
        /// 监听值
        /// </summary>
        [XmlIgnore]
        public string Value { get; set; }
        /// <summary>
        /// 触发模块
        /// </summary>
        [XmlAttribute]
        public string OnBlockName { get; set; }
        /// <summary>
        /// 触发模块
        /// </summary>
        [XmlAttribute]
        public string OffBlockName { get; set; }
        /// <summary>
        /// 多个触发模块
        /// </summary>
        [XmlElement("Item")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// 监听地址长度
        /// </summary>
         [XmlAttribute]
        public int Len { get; set; } = 1;
    }

    [Serializable]
    public class Item
    {
        /// <summary>
        /// 位置
        /// </summary>
        [XmlAttribute]
        public int Index { get; set; }
        /// <summary>
        /// 标记位含义
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 监听值
        /// </summary>
        [XmlIgnore]
        public string Value { get; set; }
        /// <summary>
        /// 触发模块
        /// </summary>
        [XmlAttribute]
        public string OnBlockName { set; get; }
        /// <summary>
        /// 触发模块
        /// </summary>
        [XmlAttribute]
        public string OffBlockName { set; get; }

    }

    /// <summary>
    /// 工作流程逻辑模块
    /// </summary>
    [Serializable]
    public class FlowBlock
    {
        /// <summary>
        /// 流程名字
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Area { get; set; }
        /// <summary>
        /// 流程步骤
        /// </summary>
        [XmlElement("Step")]
        public List<Step> Steps { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        [XmlIgnore]
        public bool IsRunning { get; set; }

        /// <summary>
        /// 信号
        /// </summary>
         [XmlIgnore]
        public AutoResetEvent ExecEvent { get; set; } = new AutoResetEvent(false);

    }

    /// <summary>
    /// 工作流步骤
    /// </summary>
    [Serializable]
    public class Step
    {
        /// <summary>
        /// plc名或者无名
        /// </summary>
        [XmlAttribute]
        public string PlcAddressName { get; set; }
        /// <summary>
        /// 执行函数
        /// </summary>
        [XmlAttribute]
        public string ActionName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }


        [XmlAttribute]
        public int TimeOut { get; set; }
        ///// <summary>
        ///// 失败模式
        ///// </summary>
        //[XmlAttribute]
        //public FailMode FailMode { get; set; } = FailMode.FailExit;
        /// <summary>
        /// 操作类型
        /// </summary>
        [XmlAttribute]
        public ActionTypes Type { get; set; } = ActionTypes.None;

        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlIgnore]
        public Result Result { get; set; } = Result.Success();

        /// <summary>
        /// 序号
        /// </summary>
        [XmlIgnore]
        public int Seq { get; set; }

        /// <summary>
        /// 执行完成
        /// </summary>
         [XmlIgnore]
        public Action Perform { get; set; }
    }



}
