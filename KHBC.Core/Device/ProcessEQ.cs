using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHBC.Core.Device
{
    class ProcessEQ
    {
        string module;
        String id;
        string ip;
        int port;
        EQstate state;//0:Waiting, -1:OffLine, 1:Running, 3:Stop
        bool alarm;
        string alarmid;
        string alarmmessage;
        string cpversion;
        string crrentprogram;
        int workcount;

        public string Id { get => id; set => id = value; }
        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public EQstate State { get => state; set => state = value; }
        public bool Alarm { get => alarm; set => alarm = value; }
        public string Cpversion { get => cpversion; set => cpversion = value; }
        public string Crrentprogram { get => crrentprogram; set => crrentprogram = value; }
        public string Alarmid { get => alarmid; set => alarmid = value; }
        public string Alarmmessage { get => alarmmessage; set => alarmmessage = value; }
        public int Workcount { get => workcount; set => workcount = value; }
        public string Module { get => module; set => module = value; }
    }

    class RobotEQ
    {
        string module;
        String id;
        string ip;
        int port;
        EQstate state;//0:Waiting, -1:OffLine, 1:Running, 3:Stop
        bool alarm;
        string alarmid;
        string alamrmessage;
        string cpversion;
        string currentprogram;

        public string Id { get => id; set => id = value; }
      
        public int Port { get => port; set => port = value; }
        public string IP { get => ip; set => ip = value; }
        public EQstate State { get => state; set => state = value; }
        public bool Alarm { get => alarm; set => alarm = value; }
        public string Cpversion { get => cpversion; set => cpversion = value; }
        public string Currentprogram { get => currentprogram; set => currentprogram = value; }
        public string Alarmid { get => alarmid; set => alarmid = value; }
        public string Alamrmessage { get => alamrmessage; set => alamrmessage = value; }
        public string Module { get => module; set => module = value; }
    }

    public enum EQstate : int
    {
        Waiting,
        OffLine,
        Running,
        Resume,
        Stop
    }
}
