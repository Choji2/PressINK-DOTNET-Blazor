using PressINK_Server_App.Model.PrinterPingModel;
using DB_SCHEMA.Data.PrinterContexts;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Web_scraper_practice_.Model.Printer_DB_Models;

namespace PressINK_Server_App.Services;

public class PingServices : BackgroundService
{
    private readonly ILogger<APIServices> _logger;
    private PrinterService _PrinterService { get; set; }
    static  List<PingModel> pinglist = new();

    public PingServices(PrinterService printerService, ILogger<APIServices> logger)
    {
        _logger = logger;
        _PrinterService = printerService;
        
    }

    public  async Task<PingModel> PingPrinter(PRINTER_MAIN printer)
    {
        var pingModel = new PingModel();
        using(var ping= new Ping())
        {
            int timeout = 1500;

            pingModel.PRINTER_MAIN = printer;
            try
            {
                PingReply res = await ping.SendPingAsync(printer.HOSTNAMEv4.Trim(), timeout);
                pingModel.Host = printer.HOSTNAMEv4;
                pingModel.response_time = res.RoundtripTime;
                pingModel.response_status = res.Status.ToString();

                if (res.Status.ToString() == SD.Success_string)
                {
                    pingModel.Online = true;
                }

                else
                {
                    pingModel.Online = false;
                }

                return pingModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                PingReply res = await ping.SendPingAsync("001.002.004.002", timeout);
                pingModel.Host = printer.HOSTNAMEv4;
                pingModel.response_time = res.RoundtripTime;
                pingModel.response_status = res.Status.ToString();

                if (res.Status.ToString() == SD.Success_string)
                {
                    pingModel.Online = true;
                }
                else
                {
                    pingModel.Online = false;
                }

                return pingModel;
            }          
        }      
    }

    public async Task<List<PingModel>> GetPingData()
    {
        List<PRINTER_MAIN> printlist = _PrinterService.GetPrinters();
        var pingList= new List<PingModel>();

        foreach(var printer in printlist)
        {
            pingList.Add(await PingPrinter(printer));
        }

        return pingList;
    }

    public async Task SetPingList()
    {
        var tempL = await GetPingData();
        pinglist = tempL;
    }
    public List<PingModel> ReturnList()
    {
        return pinglist;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SetPingList();
            _logger.LogInformation("Ping List Set");
            await Task.Delay(60000, stoppingToken);
        }
    }
}
