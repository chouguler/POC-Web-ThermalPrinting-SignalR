using System.Threading.Tasks;

namespace POCThinClientServices.Hub
{
    public interface IPOCThinClientHub
    {

        Task<string> InitializeSerialScanner(string COMPort, string BaudRate, string DataBits, string StopBits, string Parity);
        Task SendSerialScannerDataToClient(string trackingNumber);
        Task<string> InitializeSerialPrinter(string COMPort, string BaudRate, string DataBits, string StopBits, string Parity, string PrinterType, string SupplyPosition);
        Task<string> PrintLabelDataBySerial(string labelData);
        Task<string> InitializeUSBPrinter(string PrinterName, string PrinterType, string SupplyPosition);
        Task<string> PrintLabelDataByUSB(string labelData);

    }
}