using KHBC.Core.Device;
using KHBC.Core.Log;
using System;
using System.Collections.Generic;
using static KHBC.LD.ARCM.DeviceApi.ArcmApiLib;

namespace KHBC.LD.ARCM
{
    public class ArcmDeviceApi : IDevice
    {
        public bool ConnectStatus { get; set; } = false;
        public bool DataChange { get; set; } = false;
        public string Message { get; set; }
        public int ErrorCode { get; set; }

        private readonly string _serviceName;
        private UInt16 _rshd = 0;

        private readonly string _ipAddress;
        private readonly int _port;
        public ArcmDeviceApi(string serviceName, string ipAddress, int port)
        {
            _serviceName = serviceName;
            _ipAddress = ipAddress;
            _port = port;
        }
        public bool Read<T>(string addr, string length, ref object data)
        {
            return true;
        }
        public bool Read<T>(string addr, int length, ref object data)
        {
            if (ConnectStatus == false)
            {
                return false;
            }

            JointVelcAccParam MaxJointVelc = new JointVelcAccParam();
            JointVelcAccParam MaxJointAcc = new JointVelcAccParam();
            double MaxLineAcc = 0.0;
            double MaxLinVelc = 0.0;
            double MaxAngleAcc = 0.0;
            double MaxAngleVelc = 0.0;
            wayPoint_S CurrentWaypoint = new wayPoint_S();
            int ToolPowerType = 0;
            ToolDynamicsParam ToolDynamicsParam = new ToolDynamicsParam();
            ToolInEndDesc ToolKinematicsParam = new ToolInEndDesc();
            int RobotState = 0;
            int WorkMode = 0;

            rs_get_global_joint_maxvelc(_rshd, ref MaxJointVelc);
            rs_get_global_joint_maxacc(_rshd, ref MaxJointAcc);
            rs_get_global_end_max_line_acc(_rshd, ref MaxLineAcc);
            rs_get_global_end_max_line_velc(_rshd, ref MaxLinVelc);
            rs_get_global_end_max_angle_acc(_rshd, ref MaxAngleAcc);
            rs_get_global_end_max_angle_velc(_rshd, ref MaxAngleVelc);
            rs_get_current_waypoint(_rshd, ref CurrentWaypoint);
            rs_get_tool_power_type(_rshd, ref ToolPowerType);
            rs_get_tool_dynamics_param(_rshd, ref ToolDynamicsParam);
            rs_get_tool_kinematics_param(_rshd, ref ToolKinematicsParam);
            rs_get_robot_state(_rshd, ref RobotState);
            rs_get_work_mode(_rshd, ref WorkMode);

            Dictionary<string, object> dataInfo = new Dictionary<string, object>();
            dataInfo["MaxJointVelc"] = MaxJointVelc;
            dataInfo["MaxJointAcc"] = MaxJointAcc;
            dataInfo["MaxLineAcc"] = MaxLineAcc;
            dataInfo["MaxLinVelc"] = MaxLinVelc;
            dataInfo["MaxAngleAcc"] = MaxAngleAcc;
            dataInfo["MaxAngleVelc"] = MaxAngleVelc;
            dataInfo["CurrentWaypoint"] = CurrentWaypoint;
            dataInfo["ToolPowerType"] = ToolPowerType;
            dataInfo["ToolDynamicsParam"] = ToolDynamicsParam;
            dataInfo["ToolKinematicsParam"] = ToolKinematicsParam;
            dataInfo["RobotState"] = RobotState;
            dataInfo["WorkMode"] = WorkMode;

            data = dataInfo;

            ErrorCode = 0;
            Message = "Success";

            return true;
        }

        public bool ExeCmd(string cmd)
        {
            return true;
        }

        public void Connect()
        {
            do
            {
                //初始化机械臂控制库
                ErrorCode = rs_initialize();
                if (ErrorCode != 0)
                {
                    Message = "初始化机械臂控制库失败";
                    break;
                }

                //创建机械臂控制上下文句柄
                ErrorCode = rs_create_context(ref _rshd);
                if (ErrorCode != 0)
                {
                    Message = "创建机械臂控制上下文句柄失败";
                    break;
                }

                //链接机械臂服务器
                ErrorCode = rs_login(_rshd, _ipAddress, _port);
                if (ErrorCode != 0)
                {
                    Message = "创建机械臂控制上下文句柄失败";
                    break;
                }

                ConnectStatus = true;
            } while (false);

            if (ConnectStatus)
            {
                Logger.Main.Info($"[{_serviceName}初始化设备成功: {_ipAddress}:{_port}");
            }
            else
            {
                Logger.Main.Info($"[{_serviceName}]{Message}: {_ipAddress}:{_port}");
            }
        }

        public void Disconnect()
        {
            rs_logout(_rshd);
            rs_destory_context(_rshd);
            _rshd = 0;
            rs_uninitialize();
        }

        public bool Write<T>(string addr, object data)
        {
            throw new NotImplementedException();
        }
    }
}
