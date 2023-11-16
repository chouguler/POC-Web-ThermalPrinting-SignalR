using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using POCThinClientServices.Hub;

namespace POCThinClientServices.Classes
{
    public class ScannerHelper
    {
        public ScannerHelper() { }
        public CScanner m_oScanner;
        public void InitializeScanner()
        {
            //Initialize scanner object
            m_oScanner = new CScanner();
            try
            {
                // If we called Initialize when the scanner is already active, reinitialize it.
                if (m_oScanner.Device.IsOpen) // First close the port.
                    m_oScanner.Device.Close();

                ApplicationLogger.LogMessage(String.Format("Initializing Serial Scanner: Port[{0}]", ConfigurationManager.AppSettings["ScannerCOMPort"].ToString()));

                // Initialize the Scanner object with the configuration settings.
                m_oScanner.Initialize(int.Parse(ConfigurationManager.AppSettings["ScannerCOMPort"].ToString()),
                    int.Parse(ConfigurationManager.AppSettings["ScannerBaudRate"].ToString()),
                   ConfigurationManager.AppSettings["ScannerDataBits"].ToString(),
                   ConfigurationManager.AppSettings["ScannerParity"].ToCharArray()[0],
                    ConfigurationManager.AppSettings["ScannerStopBits"].ToString());

                // Now go ahead and open the port.
                m_oScanner.Device.Open();

                //Handle scanner reply
                CSerialPortWrapper.onRead += new CSerialPortWrapper.OnRead(Callback_SerialRead);

            }
            catch (Exception ex)
            {
                if (m_oScanner != null)
                    m_oScanner.Device.Close();
                //throw new Exception("Error: " + ex.Message + ". Error in opening scanner Serial Port. Please connect the printer to assigned port.");
                ApplicationLogger.LogMessage(ex.Message);
            }
        }
        private async void Callback_SerialRead(string pPort, string pData)
        {
            if (pPort == m_oScanner.Device.PortName)
            {
                //HandleScannerRead(pData);                        
                ApplicationLogger.LogMessage("Scan Data Received: " + pData);
                pOCThinClientHub pOCThinClientHub = new POCThinClientHub();
                await pOCThinClientHub.SendSerialScannerDataToClient(pData);                                 
            }
        }
    }
}
