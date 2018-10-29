using System.Runtime.CompilerServices;

namespace TrainingCsharpGft.Api
{
    class LogHelper
    {
        public static log4net.ILog GetLogger([CallerFilePath]string filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
