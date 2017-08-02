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
            Functionality();
        }

        private static void Functionality()
        {
            Common common = new Common();
            DynamicsCRM crm = new DynamicsCRM();

            try
            {
                crm._service = crm.ConnectToMSCRM();

                Guid userID = ((WhoAmIResponse)crm._service.Execute(new WhoAmIRequest())).UserId;

                if (userID != null)
                {
                    //System.IO.File.Create(Environment.CurrentDirectory + "//" + DateTime.Today.ToShortDateString() + "//" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".txt");
                    //System.IO.FileStream fileName = System.IO.File.Create(common.GetDirectory() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

                    common.SetLogFile("Outbound");

                    QueryExpression query = new QueryExpression();
                    query.EntityName = "dfc_accountnirecord";
                    query.ColumnSet = new ColumnSet("dfc_name");
                    query.TopCount = 10;
                    query.LinkEntities.Add(new LinkEntity("dfc_accountni", "dfc_accountnirecord", "dfc_accountni", "dfc_accountni", JoinOperator.Inner));
                    //query.Criteria.AddCondition("ticketnumber", ConditionOperator.Equal, "CAS-01548-Z5Z5F0");
                    EntityCollection results = crm._service.RetrieveMultiple(query);

                    foreach (Entity accountNI in results.Entities)
                    {
                        common.Log(common.logFile, accountNI.GetAttributeValue<string>("dfc_name"));
                    }

                    common.Log(common.logFile, "completd ya");
                }
            }
            catch (System.Net.WebException ex)
            {
                crm.ReConnectToMSCRM();
                Functionality();
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                crm.ReConnectToMSCRM();
                Functionality();
            }
            catch (Exception ex)
            {
                common.Log(common.logFile, ex.ToString());
            }
            finally
            {
            }
        }

        protected override void OnStop()
        {
        }
    }
}
