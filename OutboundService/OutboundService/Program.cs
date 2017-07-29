using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OutboundService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            OutboundServiceSvc myService = new OutboundServiceSvc();
            myService.OnDebug();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OutboundServiceSvc()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
