namespace BoardAutoTesting.Log
{
    internal static class Logger
    {
        public static EzLogger Glog;

        public static void Log(string filePathPre = @"D:\BoardTest_")
        {
            Glog = new EzLogger(true, (uint)EzLogger.Level.All, filePathPre);
            Glog.Start();
            Glog.Success("-----------------------");
            Glog.Success("日志启动ok");
        }

        public static void LogClose()
        {
            Glog.Success("日志关闭");
            Glog.Stop();
        } 
    }
}