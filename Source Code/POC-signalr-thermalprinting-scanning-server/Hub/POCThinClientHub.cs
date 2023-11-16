using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using POCThinClientServices.Classes;

namespace POCThinClientServices.Hub
{
    [HubName("POCThinClientHub")]
    public class POCThinClientHub : Hub<IPOCThinClientHub>
    {
        public POCThinClientHub() { }
        #region Serial Scanner
        //Initialize Serial Scanner
        public async Task<string> InitializeSerialScanner(string COMPort, string BaudRate, string DataBits, string StopBits, string Parity)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                bool bUpdated = false;

                if (!string.IsNullOrEmpty(COMPort)) { bUpdated = true; settings["ScannerCOMPort"].Value = COMPort; };
                if (!string.IsNullOrEmpty(BaudRate)) { bUpdated = true; settings["ScannerBaudRate"].Value = BaudRate; };
                if (!string.IsNullOrEmpty(DataBits)) { bUpdated = true; settings["ScannerDataBits"].Value = DataBits; };
                if (!string.IsNullOrEmpty(StopBits)) { bUpdated = true; settings["ScannerStopBits"].Value = StopBits; };
                if (!string.IsNullOrEmpty(Parity)) { bUpdated = true; settings["ScannerParity"].Value = Parity; };

                //Update configuration file
                if (bUpdated)
                {
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                // Handle error and send error message to the client
                var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
                await context.Clients.All.handleErrorMessage(ex.Message);
                return ex.Message;
            }
        }
        //GetScannerDataBySerial
        public async Task SendSerialScannerDataToClient(string trackingNumber)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
            try
            {
                //Call client method() - getScannerDataBySerial
                await context.Clients.All.getScannerDataBySerial(trackingNumber);
            }
            catch (Exception ex)
            {
                // Handle error and send error message to the client
                await context.Clients.All.handleErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Serial Printer
        //Initialize Serial Printer
        public async Task<string> InitializeSerialPrinter(string COMPort, string BaudRate, string DataBits, string StopBits, string Parity, string PrinterType, string SupplyPosition)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                bool bUpdated = false;

                if (!string.IsNullOrEmpty(COMPort)) { bUpdated = true; settings["SerialPrinterCOMPort"].Value = COMPort; };
                if (!string.IsNullOrEmpty(BaudRate)) { bUpdated = true; settings["SerialPrinterBaudRate"].Value = BaudRate; };
                if (!string.IsNullOrEmpty(DataBits)) { bUpdated = true; settings["SerialPrinterDataBits"].Value = DataBits; };
                if (!string.IsNullOrEmpty(StopBits)) { bUpdated = true; settings["SerialPrinterStopBits"].Value = StopBits; };
                if (!string.IsNullOrEmpty(Parity)) { bUpdated = true; settings["SerialPrinterParity"].Value = Parity; };
                if (!string.IsNullOrEmpty(PrinterType)) { bUpdated = true; settings["SerialPrinterType"].Value = PrinterType; };
                if (!string.IsNullOrEmpty(SupplyPosition)) { bUpdated = true; settings["SerialPrinterSupplyPosition"].Value = SupplyPosition; };

                //Update configuration file
                if (bUpdated)
                {
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
                return "Success";
            }

            catch (Exception ex)
            {
                // Handle error and send error message to the client
                var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
                await context.Clients.All.handleErrorMessage(ex.Message);
                return ex.Message;
            }
        }
        //PrintLabelDataBySerial
        public async Task<string> PrintLabelDataBySerial(string labelData)
        {
            try
            {
                PrinterHelper printHelper = new PrinterHelper();
                return await printHelper.PrintLabelData(labelData, "Serial");
            }
            catch (Exception ex)
            {
                // Handle error and send error message to the client
                var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
                await context.Clients.All.handleErrorMessage(ex.Message);
                return ex.Message;
            }
        }
        #endregion

        #region USB Printer
        //Initialize USB Printer
        public async Task<string> InitializeUSBPrinter(string PrinterName, string PrinterType, string SupplyPosition)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                bool bUpdated = false;

                if (!string.IsNullOrEmpty(PrinterName)) { bUpdated = true; settings["USBPrinterName"].Value = PrinterName; };
                if (!string.IsNullOrEmpty(PrinterType)) { bUpdated = true; settings["USBPrinterType"].Value = PrinterType; };
                if (!string.IsNullOrEmpty(SupplyPosition)) { bUpdated = true; settings["USBPrinterSupplyPosition"].Value = SupplyPosition; };

                //Update configuration file
                if (bUpdated)
                {
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
                return "Success";
            }

            catch (Exception ex)
            {
                // Handle error and send error message to the client
                var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
                await context.Clients.All.handleErrorMessage(ex.Message);
                return ex.Message;
            }
        }
        //PrintLabelDataByUSB
        public async Task<string> PrintLabelDataByUSB(string labelData)
        {
            try
            {
                PrinterHelper printHelper = new PrinterHelper();
                return await printHelper.PrintLabelData(labelData, "USB");
            }
            catch (Exception ex)
            {
                // Handle error and send error message to the client
                var context = GlobalHost.ConnectionManager.GetHubContext<POCThinClientHub>();
                await context.Clients.All.handleErrorMessage(ex.Message);
                return ex.Message;
            }
        }
        #endregion
    }
}
