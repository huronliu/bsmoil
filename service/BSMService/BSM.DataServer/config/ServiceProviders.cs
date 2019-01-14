using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace BSM.DataServer
{
    class ServiceProviders
    {
        private static IServiceProvider _instance;

        public static IServiceProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    ConfigServiceProvider();
                }
                return _instance;
            }
        }

        public static void ConfigServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
                        
            _instance = serviceCollection.BuildServiceProvider();

        }
    }
}
