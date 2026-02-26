using PressINK_Server_App.Model.PrinterAPIModels;

namespace PressINK_Server_App.Services
{
    public class APIInjectServices : APIServices
    {
        public APIInjectServices(PrinterService printerService, ILogger<APIServices> logger) : base(printerService, logger)
        {
        }

        public List<PrinterInfoFull_Agile> GetFullPrinterObjects()
        {
            return GetFullList();
        }

        public int GetFullCount()
        {
            return FullCount;
        }

    }
}
