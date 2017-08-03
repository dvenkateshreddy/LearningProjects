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
                    common.SetLogFile("Outbound");

                    QueryExpression query = new QueryExpression();
                    query.EntityName = "wrmm_payment";
                    query.ColumnSet = new ColumnSet(true);
                    query.TopCount = 10;

                    #region WorkingCode
                    //query.LinkEntities.Add(new LinkEntity("wrmm_payment", "dfc_accountnirecord", "dfc_accountnirecord", "dfc_accountnirecordid", JoinOperator.Inner));
                    //query.LinkEntities[0].EntityAlias = "temp1";
                    //query.Criteria.AddCondition("dfc_accountnirecord", "dfc_invoicenumber", ConditionOperator.Equal, "WSP0005378");
                    #endregion

                    //query.AddLink("dfc_accountnirecord", "dfc_accountnirecord", "dfc_accountnirecordid", JoinOperator.Inner);
                    //query.LinkEntities[0].EntityAlias = "temp1";
                    //query.Criteria.AddCondition("temp1", "dfc_invoicenumber", ConditionOperator.Equal, "WSP0005378");
                    //query.LinkEntities.Add(new LinkEntity("dfc_accountnirecord", "dfc_accountni", "dfc_accountni", "dfc_accountniid", JoinOperator.Inner));
                    //query.LinkEntities[1].EntityAlias = "temp2";
                    //query.Criteria.AddCondition("temp2", "dfc_isexported", ConditionOperator.Equal, 0);
                    //query.Criteria.AddCondition("temp2", "dfc_name", ConditionOperator.Contains, "ap_supplier_inv_dfc_20022017102512.csv");


                    //LinkEntity le1 = new LinkEntity("wrmm_payment", "dfc_accountnirecord", "dfc_accountnirecord", "dfc_accountnirecordid", JoinOperator.Inner);
                    //le1.Columns = new ColumnSet("dfc_invoicenumber");
                    //LinkEntity le2 = new LinkEntity("dfc_accountnirecord", "dfc_accountni", "dfc_accountni", "dfc_accountniid", JoinOperator.Inner);
                    //le2.Columns = new ColumnSet("dfc_name", "dfc_isexported");

                    //le1.LinkEntities.Add(le2);                

                    //query.LinkEntities.Add(le1);




                    query.AddLink("dfc_accountnirecord", "dfc_accountnirecord", "dfc_accountnirecordid", JoinOperator.Inner);
                    query.LinkEntities[0].AddLink("dfc_accountni", "dfc_accountni", "dfc_accountniid", JoinOperator.Inner);

                    FilterExpression fe = new FilterExpression(LogicalOperator.And);

                    FilterExpression fe1 = new FilterExpression(LogicalOperator.And);
                    FilterExpression fe2 = new FilterExpression(LogicalOperator.And);


                    fe1.Conditions.Add(new ConditionExpression("dfc_isexported", ConditionOperator.Equal, false));
                    fe2.Conditions.Add(new ConditionExpression("dfc_name", ConditionOperator.Contains, "ap_supplier_inv_dfc_20022017102512"));

                    fe.AddFilter(fe1);
                    fe.AddFilter(fe2);

                    


                    query.LinkEntities[0].EntityAlias = "temp1";
                    query.LinkEntities[0].LinkEntities[0].EntityAlias = "temp2";

                    query.LinkEntities[0].LinkEntities[0].LinkCriteria = fe;

                    //query.LinkEntities[0].LinkEntities[0].LinkCriteria.AddCondition("dfc_isexported", ConditionOperator.Equal, false);
                    //query.LinkEntities[0].LinkEntities[0].LinkCriteria.AddCondition("dfc_name", ConditionOperator.Contains, "ap_supplier_inv_dfc_20022017102512");

                    //query.Criteria.AddCondition(query.LinkEntities[0].LinkEntities[0].EntityAlias, "dfc_isexported", ConditionOperator.Equal, false);
                    //query.Criteria.AddCondition(query.LinkEntities[0].LinkEntities[0].EntityAlias, "dfc_name", ConditionOperator.Contains, "ap_supplier_inv_dfc_20022017102512.csv");


                    //query.Criteria.AddCondition()

                    //  Query using ConditionExpression and FilterExpression
                    //ConditionExpression condition1 = new ConditionExpression();
                    //condition1.AttributeName = "dfc_invoicenumber";
                    //condition1.Operator = ConditionOperator.Equal;
                    //condition1.Values.Add("WSP0005378");

                    //FilterExpression filter1 = new FilterExpression();
                    //filter1.Conditions.Add(condition1);

                    //query.Criteria.AddFilter(filter1);

                    //dfc_isexported dfc_name wrmm_invoicenumber
                    //query.LinkEntities[0].Columns.AddColumns("dfc_name", "dfc_isexported");

                    EntityCollection results = crm._service.RetrieveMultiple(query);

                    foreach (Entity payment in results.Entities)
                    {
                        string value = payment.GetAttributeValue<EntityReference>("wrmm_supplementarypayment").Name + " - " + payment.GetAttributeValue<DateTime>("wrmm_paymentperiodfrom").ToString() + " - " +
                            payment.GetAttributeValue<DateTime>("wrmm_paymentperiodto").ToString() + " - " + payment.GetAttributeValue<DateTime>("wrmm_scheduledissuedate").ToString();

                        common.Log(common.logFile, value);
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
