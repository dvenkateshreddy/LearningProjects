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
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

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
            DynamicsCRM crm = new DynamicsCRM();
            crm._service = crm.ConnectToMSCRM(@"dfcni\swathiy", "Pass@123", "http://192.168.87.5:5555/spslive31b/xrmservices/2011/organization.svc");

            Guid userID = ((WhoAmIResponse)crm._service.Execute(new WhoAmIRequest())).UserId;

            if (userID != null)
            {
                //System.IO.File.Create(Environment.CurrentDirectory + "//" + DateTime.Today.ToShortDateString() + "//" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".txt");
                //System.IO.FileStream fileName = System.IO.File.Create(common.GetDirectory() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

                common.SetLogFile("Outbound");

                QueryExpression query = new QueryExpression();
                query.EntityName = "dfc_accountni";
                query.ColumnSet = new ColumnSet("dfc_name");
                query.TopCount = 10;
                query.LinkEntities.Add(new LinkEntity("dfc_accountni", "dfc_accountnirecord", "dfc_accountnirecord", "dfc_accountnirecord", JoinOperator.Inner));
                //query.Criteria.AddCondition("ticketnumber", ConditionOperator.Equal, "CAS-01548-Z5Z5F0");
                EntityCollection results = crm._service.RetrieveMultiple(query);

                foreach(Entity accountNI in results.Entities)
                {
                    common.Log(common.logFile, accountNI.GetAttributeValue<string>("dfc_name"));
                }

                common.Log(common.logFile, "completd ya");
            }
        }

        protected override void OnStop()
        {
        }
    }
}
