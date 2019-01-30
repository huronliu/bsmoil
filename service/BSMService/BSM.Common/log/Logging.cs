using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Configuration;

namespace BSM.Common
{
    public class Logging
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\bsm-log.txt", Serilog.Events.LogEventLevel.Information, fileSizeLimitBytes: 10485760)
                .CreateLogger();
        }
    }
}
