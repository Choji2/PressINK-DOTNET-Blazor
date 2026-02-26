using PressINK_Server_App.Model.PrinterPingModel;

namespace PressINK_Server_App.Services;

public class PingInjectServices : PingServices
{
    public PingInjectServices(PrinterService printerService, ILogger<APIServices> logger) : base(printerService, logger)
    {
    }

    public List<PingModel> GetPingList()
    {
        return ReturnList();
    }

}
