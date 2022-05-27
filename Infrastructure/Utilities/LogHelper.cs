using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exp"></param>
        public static void Error(object msg, Exception exp = null)
        {
            if (exp == null)
                logger.Error(msg);
            else
                logger.Error(msg + "  " + exp.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exp"></param>
        public static void Debug(object msg, Exception exp = null)
        {
            if (exp == null)
                logger.Debug(msg);
            else
                logger.Debug(msg + "  " + exp.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exp"></param>
        public static void Info(object msg, Exception exp = null)
        {
            if (exp == null)
                logger.Info(msg);
            else
                logger.Info(msg + "  " + exp.ToString());
        }

        /// <summary>
        /// 打印json日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <param name="msgDetail"></param>
        /// <param name="exp"></param>
        public static void Info(string name, object msg, string msgDetail = null, Exception exp = null)
        {
            JObject obj = new JObject();
            obj["name"] = name;
            obj["msg"] =Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            obj["msgDetail"] = msgDetail;
            if (exp != null)
            {
                obj["exp"] = exp.ToString();
            }
            logger.Info(obj);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exp"></param>
        public static void Warn(object msg, Exception exp = null)
        {
            if (exp == null)
                logger.Warn(msg);
            else
                logger.Warn(msg + "  " + exp.ToString());
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="time"></param>
        /// <param name="objRemark"></param>
        public static void WriteLog(string name, object obj, long time = 0, object objRemark = null)
        {
            try
            {
                if (time > 0 && objRemark != null)
                {
                    LogInfo info = new LogInfo();
                    info.Object = obj;
                    info.ElapsedMilliseconds = time;
                    info.Des = objRemark;
                    //logger.Info("{" + name + "}", info);
                    LogEventInfo lei = new LogEventInfo(NLog.LogLevel.Info, "", "");
                    lei.Properties["ElapsedMilliseconds"] = time;
                    lei.Properties["Msg"] = info;
                    lei.Properties["MethodName"] = name;
                    lei.Level = LogLevel.Info;
                    logger.Log(lei);
                }
                else if (time > 0 && objRemark == null)
                {
                    LogTimeModel info = new LogTimeModel();
                    info.Object = obj;
                    info.ElapsedMilliseconds = time;
                    LogEventInfo lei = new LogEventInfo();
                    lei.Properties["ElapsedMilliseconds"] = time;
                    lei.Properties["Msg"] = obj;
                    lei.Properties["MethodName"] = name;
                    lei.Level = LogLevel.Info;
                    logger.Log(lei);
                    //logger.Info("{" + name + "}", info);
                }
                else if (objRemark != null)
                {
                    LogDesInfo info = new LogDesInfo();
                    info.Object = obj;
                    info.Des = objRemark;
                    logger.Info("{" + name + "}", info);
                }
                else
                {
                    logger.Info("{" + name + "}", obj);
                }

            }
            catch (Exception ex)
            {
                logger.Info("{WriteLog}", ex.StackTrace + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class LogOneModel
        {
            /// <summary>
            /// 
            /// </summary>
            public object Object { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class LogTimeModel : LogOneModel
        {
            /// <summary>
            /// 
            /// </summary>
            public long ElapsedMilliseconds { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class LogDesInfo : LogOneModel
        {
            /// <summary>
            /// 
            /// </summary>
            public object Des { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class LogInfo : LogTimeModel
        {
            /// <summary>
            /// 
            /// </summary>
            public object Des { get; set; }
        }
    }
}
