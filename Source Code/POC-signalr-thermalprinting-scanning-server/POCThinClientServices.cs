using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using POCThinClientServices.Classes;

namespace POCThinClientServices
{
    public partial class POCThinClientServices : ServiceBase
    {
        public POCThinClientServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ApplicationLogger.Active = true;
            ApplicationLogger.LogMessage("POC Thin Client Service started...");

            //Listen to scanner reads
            ScannerHelper scanHelper = new ScannerHelper();
            scanHelper.InitializeScanner();

            //Bind the SignalR address
            string appBaseURL = ConfigurationManager.AppSettings["appBaseURL"].ToString();
            WebApp.Start(appBaseURL);
        }

        protected override void OnStop()
        {            
        }
    }
}
