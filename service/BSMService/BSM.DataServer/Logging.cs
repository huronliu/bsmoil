﻿using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Configuration;

namespace BSM.DataServer
{
    public class Logging
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\bsm-log.txt", Serilog.Events.LogEventLevel.Debug, fileSizeLimitBytes: 10485760)
                .CreateLogger();
        }
    }
}
