using CSRedis;
using KHBC.Core.BaseModels;
using KHBC.Core;
using KHBC.Core.Log;
using KHBC.LD.OLCM.DeviceApi;
using KHBC.Core.Device;
using System;

namespace KHBC.LD.OLCM
{
    /// <summary>
    /// 线程服务
    /// </summary>
    public class OlcmService : BaseService
    {
        public sealed override string ServiceName { get; set; }
        //private CSRedisClient RedisClientLocal;
        //private CSRedisClient RedisClientRemote;
        //private bool ConnectStatus = false;
        //private readonly OlcmDeviceConfModel _devConf;
        //private readonly string _deviceFullName;
        //private readonly string _cmdQueueKey;
        //private readonly string _keyDevData;
        //private readonly string _keyDevState;
        //private readonly string _keyRemoteQueue;
        //private OkumaDeviceApi _deviceApi;
        private readonly DeviceHandler _deviceHandler;
        //public OlcmService(OlcmDeviceConfModel conf)
        //{
        //    ServiceName = $"{SysConf.ModuleName}:{conf.DeviceId}";
        //    _deviceFullName = $"{SysConf.KeyPlant}:{ServiceName}";
        //    _cmdQueueKey = $"{_deviceFullName}:Q";
        //    _keyDevData = $"{_deviceFullName}:DC";
        //    _keyDevState = $"{_deviceFullName}:ST";
        //    _keyRemoteQueue = $"{SysConf.KeyPlant}:BDCI:Q";

        //    _devConf = conf;
        //}

        public OlcmService(DeviceInfo devInfo)
        {
            ServiceName = $"{SysConf.ModuleName}:{devInfo.Id}";
            _deviceHandler = new DeviceHandler(this, devInfo, new OkumaDeviceApi(ServiceName, devInfo.IpAddress, devInfo.Port));
            TimeSpan = devInfo.PollingTime;
        }
        protected override void DoWork()
        {
            _deviceHandler.Process();
        }

    //    public override void ThreadWork()
    //    {
    //        try
    //        {
    //            RedisClientLocal = new CSRedisClient(SysConf.Main.RedisLocal.ConnectStrings);
    //            Logger.Main.Info($"[{ServiceName}]初始化LOCAL REDIS CLIENT成功: \"{SysConf.Main.RedisLocal.ConnectStrings}\"");
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]初始化LOCAL REDIS CLIENT失败: \"{SysConf.Main.RedisLocal.ConnectStrings}\", {ex.Message}");
    //        }

    //        try
    //        {
    //            RedisClientRemote = new CSRedisClient(SysConf.Main.RedisRemote.ConnectStrings);
    //            Logger.Main.Info($"[{ServiceName}]初始化REMOTE REDIS CLIENT成功: \"{SysConf.Main.RedisRemote.ConnectStrings}\"");
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]初始化REMOTE REDIS CLIENT失败: {ex.Message}");
    //        }

    //        Logger.Main.Info($"[{ServiceName}]初始化设备: {_devConf.DeviceId},  {_devConf.DeviceType}, {_devConf.IpAddress}, {_devConf.NcType}");
    //        _deviceApi = new OkumaDeviceApi();

    //        base.ThreadWork();
    //    }

    //    /// <summary>
    //    /// 获取设备信息
    //    /// </summary>
    //    private void GetDataInfo()
    //    {
    //        //OlcmDataInfo info = new OlcmDataInfo
    //        //{
    //        //    PlantId = SysConf.Main.Plant.Id,
    //        //    AssemblyLineId = SysConf.Main.AssemblyLine.Id,
    //        //    DeviceId = _devConf.DeviceId,
    //        //    CreateTime = DateTime.Now
    //        //};

    //        //try
    //        //{
    //        //    info.ActualEdgeNum = 11;
    //        //    //
    //        //}
    //        //catch (Exception)
    //        //{
    //        //    Logger.Main.Warn($"[{ServiceName}]获取设备信息失败: {_devConf.DeviceId}, {_devConf.IpAddress}, {_devConf.NcType}");
    //        //    ConnectStatus = false;
    //        //    UpdateStatus(ConnectStatus);
    //        //}

    //        //Object obj = new
    //        //{
    //        //    ModuleName = ModuleName,
    //        //    ServiceName = ServiceName,
    //        //    Data = info
    //        //};

    //        //SaveObj(obj);
    //    }

    //    /// <summary>
    //    /// 更新连接状态到REDIS
    //    /// </summary>
    //    /// <param name="status">连接状态</param>
    //    private void UpdateStatus(bool status)
    //    {
    //        try
    //        {
    //            RedisClientLocal.Set(_keyDevState, status);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]LOCAL REDIS CLIENT更新设备状态失败: {ex.Message}");
    //        }

    //        try
    //        {
    //            RedisClientRemote.Set(_keyDevState, status);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]REMOTE REDIS CLIENT更新设备状态失败: {ex.Message}");
    //        }
    //    }

    //    /// <summary>
    //    /// 保存获取的到数据到REDID
    //    /// </summary>
    //    /// <param name="obj">自定义数据对象</param>
    //    private void SaveObj(Object obj)
    //    {
    //        try
    //        {
    //            RedisClientLocal.Set(_keyDevData, obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]LOCAL REDIS CLIENT Set失败: {ex.Message}");
    //        }

    //        try
    //        {
    //            RedisClientRemote.LPush(_keyRemoteQueue, obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Main.Error($"[{ServiceName}]REMOTE REDIS CLIENT LPush失败: {ex.Message}");
    //        }

    //    }

    //    /// <summary>
    //    /// 线程运行
    //    /// </summary>
    //    protected override void DoWork()
    //    {
    //        if (ConnectStatus == false)
    //        {
    //            var ncType = NCTYPE.TYPE_LATHE;
    //            if (_devConf.NcType.ToUpper() != "LATHE")
    //            {
    //                ncType = NCTYPE.TYPE_MC;
    //            }

    //            //_deviceApi.Connect(true, _devConf.IpAddress, ncType);
    //            _deviceApi.Connect();
    //            if (_deviceApi.IsConnected)
    //            {
    //                Logger.Main.Info($"[{ServiceName}]连接到设备成功: {_devConf.DeviceId}, {_devConf.IpAddress}, {_devConf.NcType}");
    //                ConnectStatus = true;
    //                UpdateStatus(ConnectStatus);

    //            }
    //            else
    //            {
    //                Logger.Main.Warn($"[{ServiceName}]连接到设备失败: {_devConf.DeviceId}, {_devConf.IpAddress}:{_devConf.NcType}, {_deviceApi.ErrMsg}");
    //                UpdateStatus(ConnectStatus);
    //            }
    //        }

    //        try
    //        {
    //            var cmd = RedisClientLocal.BRPop(_devConf.PollingTime, _cmdQueueKey);
    //            if (!string.IsNullOrWhiteSpace(cmd))
    //            {
    //                Logger.Main.Warn($"[{ServiceName}]收到控制命令: {cmd}");
    //                // TODO
    //            }
    //        }
    //        catch (Exception)
    //        {

    //        }

    //        if (ConnectStatus == true)
    //        {
    //            GetDataInfo();
    //        }
    //    }
    }
}
