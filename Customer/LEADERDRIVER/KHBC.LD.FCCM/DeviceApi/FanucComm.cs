using System;

namespace KHBC.LD.FCCM.DeviceApi
{
    public class FanucComm
    {

        //返回对象ob中字段data
        public static object GetField(object ob, string data)
        {
            Type type = ob.GetType();
            return type.GetField(data).GetValue(ob);
        }

        //byte 转string
        public static string GetString(byte bt)
        {
            byte[] bts = new byte[1];
            bts[0] = bt;
            return BitConverter.ToString(bts);
        }
        //根据地址码和地址号,返回完整的显示信息
        private static string GetPmcAdd(short a, ushort b)
        {
            var tempa = Enum.GetName(typeof(Tempa), a);
            var tempb = b.ToString().PadLeft(4, '0');
            return tempa + tempb;
        }
        /// <summary>
        /// 根据alm_grp 编号 返回 提示内容 简
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static string GetAlmgrp(int no)
        {
            return Enum.GetName(typeof(AlmGrpCode), no);
        }
    }

    #region enum

    //根据地址码和地址号，返回完整的显示信息
    public enum Tempa
    {
        G,
        F,
        Y,
        X,
        A,
        R,
        T,
        K,
        C,
        D,
        N
    }

    //AlarmGroup Code
    public enum AlmGrpCode : int
    {
        SW,
        PW,
        IO,
        PS,
        OT,
        OH,
        SV,
        SR,
        MC,
        SP,
        DS,
        IE,
        BG,
        SN,
        reserved,
        EX,
        PC = 19

    }


    #endregion

}
