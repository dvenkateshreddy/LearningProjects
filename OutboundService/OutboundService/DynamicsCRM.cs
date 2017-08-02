using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OutboundService
{
    class DynamicsCRM
    {
        public IOrganizationService _service;
        public static int retry = 0;

        internal void ReConnectToMSCRM()
        {
            if(retry < int.Parse(System.Configuration.ConfigurationManager.AppSettings["RetryCount"]))
            {
                retry++;
                ConnectToMSCRM();
            }
        }

        internal IOrganizationService ConnectToMSCRM()
        {
            //try
            //{
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = ConfigurationManager.AppSettings["UserName"];
                credentials.UserName.Password = ConfigurationManager.AppSettings["Password"];
                Uri serviceUri = new Uri(ConfigurationManager.AppSettings["SoapOrgServiceUri"]);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                _service = (IOrganizationService)proxy;
                return _service;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error while connecting to CRM " + ex.Message);
            //    Console.ReadKey();
            //    return null;
            //}
        }
    }
}
