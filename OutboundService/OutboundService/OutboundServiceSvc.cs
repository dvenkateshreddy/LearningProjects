using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;

namespace OutboundService
{
    public partial class OutboundServiceSvc : ServiceBase
    {
        //public static Microsoft.Xrm.Sdk.IOrganizationService _service;
        public OutboundServiceSvc()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Common common = new Common();
            DynamicsCRM._service = DynamicsCRM.ConnectToMSCRM(@"dfcni\swathiy", "Pass@123", "http://192.168.87.5:5555/spslive31b/xrmservices/2011/organization.svc");

            Guid userID = ((WhoAmIResponse)DynamicsCRM._service.Execute(new WhoAmIRequest())).UserId;

            if (userID != null)
            {
                //System.IO.File.Create(Environment.CurrentDirectory + "//" + DateTime.Today.ToShortDateString() + "//" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".txt");

                System.IO.File.Create(common.GetDirectory() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            }
        }

        protected override void OnStop()
        {
        }
    }
}
