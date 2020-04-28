namespace KHBC.LD.OLCM.DeviceApi
{
    public enum NCTYPE
    {
        TYPE_LATHE,
        TYPE_MC
    };
    /// <summary>
    /// M/C only
    /// </summary>
    public enum Mode
    {
        Empty,
        A,
        B,
        C
    }
    public enum Spindle : int
    {
        /// <summary>
        /// Std./2SP-R
        /// </summary>
        R,
        /// <summary>
        /// 2SP-L
        /// </summary>
        L
    }

}
