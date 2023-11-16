using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Configuration;
using System.IO;
using System.Runtime;

namespace POCThinClientServices.Classes
{
    public class PrinterHelper
    {
        public PrinterHelper() { }
        public CPrinter m_oPrinter;

        public async Task<string> PrintLabelData(string sLabelData, string sCommunicationType)
        {
            string sUSBPrinter = "";

            try
            {
                //Write log file with message
                ApplicationLogger.Active = true;
                ApplicationLogger.LogMessage("Print Data Received: " + sLabelData);

                //Get printer type
                string sPrinterType = sCommunicationType == "Serial" ?
                    ConfigurationManager.AppSettings["SerialPrinterType"].ToString() :
                    ConfigurationManager.AppSettings["USBPrinterType"].ToString();

                int iPrinterType = CPrinterType.PrinterType(sPrinterType);
                if (iPrinterType == 0) { return "Error: Printer type " + sPrinterType + " not found!"; }

                //Support for USB, If we find a windows Zebra printer, use it if not resort to serial.
                if (sCommunicationType == "USB")
                {
                    //Get printer name
                    string sPrinterName = ConfigurationManager.AppSettings["USBPrinterName"].ToString();
                    sUSBPrinter = FindUSBPrinter(sPrinterName);
                    if (string.IsNullOrEmpty(sUSBPrinter)) { return "Error: USB Printer " + sPrinterName + " not found!"; }
                }

                //Initialize a printer object
                m_oPrinter = new CPrinter(iPrinterType, sUSBPrinter);

                //Serial printer communication
                if (string.IsNullOrEmpty(sUSBPrinter))
                {
                    // This is added to help with intermittent Intermec labels with extra/missing data.
                    m_oPrinter.Printer.Device.DtrEnable = true;
                    SetSerialPrinterSettings(m_oPrinter);
                    ApplicationLogger.LogMessage(String.Format("Initializing Serial Printer: Port[{0}], Baud Rate[{1}], {2}-{3}-{4}...", m_oPrinter.Printer.Device.PortName, m_oPrinter.Printer.Device.BaudRate, m_oPrinter.Printer.Device.DataBits, m_oPrinter.Printer.Parity, m_oPrinter.Printer.Device.StopBits));

                    if (m_oPrinter.Printer.IsOpen)
                    {
                        // Close it first
                        m_oPrinter.Printer.Close();
                    }
                    m_oPrinter.Printer.Open();
                }
                else
                    ApplicationLogger.LogMessage(String.Format("Initializing USB Printer: Name[{0}] ...", sUSBPrinter));

                //Initialize other parameters based on printer model
                switch (m_oPrinter.PrinterType)
                {
                    case CPrinterType.Monarch:
                        // set the correct settings.
                        // Use the printer object to initialize this.
                        m_oPrinter.SupplyPosition = sCommunicationType == "Serial" ? ConfigurationManager.AppSettings["SerialPrinterSupplyPosition"].ToString() :
                                                    ConfigurationManager.AppSettings["USBPrinterSupplyPosition"].ToString();
                        m_oPrinter.Initialize();
                        // No longer set backfeed since it has been known to cause labels to wrap.
                        break;

                    case CPrinterType.Intermec:
                        break;

                    case CPrinterType.Eltron:
                        //hack: verify correct settings, it seems to trigger error from printer
                        m_oPrinter.Initialize();         //m_oPrinter.SendCommand("US");           // Enable Error Reporting
                        m_oPrinter.SendCommand("eR^,0"); // Set User defined Error / Status Character
                        break;

                    case CPrinterType.ZPL:
                        break;

                    case CPrinterType.Generic:
                        break;

                    default:
                        break;
                }

                //Print label data
                m_oPrinter.SendCommand(sLabelData);
                ApplicationLogger.LogMessage("Label printing completed successfully.");

                return "Success";
            }
            catch (Exception ex)
            {
                // Notify error
                if (string.IsNullOrEmpty(sUSBPrinter) && m_oPrinter != null)
                    m_oPrinter.Printer.Close();
                throw new Exception(string.Format("Error: " + ex.Message + ". Error in opening printer port {0}.  Please connect the printer to assigned port.", sCommunicationType));
            }
            finally
            {
                if (string.IsNullOrEmpty(sUSBPrinter) && m_oPrinter != null)
                    m_oPrinter.Printer.Close();
            }
        }

        private void SetSerialPrinterSettings(CPrinter pPrinter)
        {
            //have to re-specify the port, baud ... again.
            pPrinter.Printer.PortNo = int.Parse(ConfigurationManager.AppSettings["SerialPrinterCOMPort"]);
            pPrinter.Printer.Device.BaudRate = int.Parse(ConfigurationManager.AppSettings["SerialPrinterBaudRate"]);
            pPrinter.Printer.Parity = char.Parse(ConfigurationManager.AppSettings["SerialPrinterParity"]);
            pPrinter.Printer.Device.DataBits = int.Parse(ConfigurationManager.AppSettings["SerialPrinterDataBits"]);
            pPrinter.Printer.StopBits = ConfigurationManager.AppSettings["SerialPrinterStopBits"].ToString();
        }
        private string FindUSBPrinter(string sPrinterName)
        {
            // sUSBPrinter will be populated if the printer is installed even if it is Offline or not connected.
            string sOutPrinterName = string.Empty;

            //Return given printer name
            foreach (String printer in PrinterSettings.InstalledPrinters)
            {
                if (printer.ToLower().Contains(sPrinterName.ToLower()))  // Search based on given name
                {
                    sOutPrinterName = printer;
                }
            }
            return sOutPrinterName;
        }
    }
}
