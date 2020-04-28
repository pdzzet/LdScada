namespace KHBC.Core.Extend
{
    /// <summary>
    /// 系统级别返回模型
    /// </summary>
    public class Result<T>
    {
        /// <summary>
        /// 接口执行是否成功
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// 接口执行结果
        /// </summary>
        public string Msg;
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public T Data;
        /// <summary>
        /// 扩展
        /// </summary>
        public object Extend;

        public static Result<T> Success(T t, string msg = "")
        {
            return new Result<T>() { IsSuccess = true, Data = t, Msg = msg };
        }
        public static Result<T> Fail(string msg = "", object extend = null)
        {
            return new Result<T>() { IsSuccess = false, Msg = msg, Extend = extend };
        }

    }
    /// <summary>
    /// 系统级别返回模型
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// 接口执行是否成功
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// 接口执行结果
        /// </summary>
        public string Msg;
        /// <summary>
        /// 扩展信息
        /// </summary>
        public object Extend;
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public object Data;

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result Success(object obj = null, string msg = "")
        {
            return new Result() { IsSuccess = true, Data = obj, Msg = msg, };
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Result Fail(string msg = "", object obj = null)
        {
            return new Result() { IsSuccess = false, Data = obj, Msg = msg};
        }


    }
}
