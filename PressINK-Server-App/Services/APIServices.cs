using PressINK_Server_App.Model.API_Data;
using PressINK_Server_App.Model.API_Data.Return_Types;
using PressINK_Server_App.Model.API_Form_Models;
using PressINK_Server_App.Model.Auth._Models;
using PressINK_Server_App.Model.PrinterAPIModels;
using PressINK_Server_App.Model.PrinterAPIModels.Lexmark.M5255;
using ApexCharts;
using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OneOf.Types;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Web_scraper_practice_.Model.Printer_DB_Models;
using Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = System.Text.Json.JsonSerializer;



namespace PressINK_Server_App.Services;

public class APIServices : BackgroundService
{
     
    private readonly ILogger<APIServices> _logger;
    static List<PrinterInfoFull> APIList {get;set;}
    static List<PrinterInfoFull_Agile> FullPrinterList;

    public static int APICount { get; set;}
    public static int FullCount { get; set; }
    public PrinterService _PrinterService { get;set; }


   
    public APIServices(PrinterService printerService, ILogger<APIServices> logger)
    {

        _PrinterService = printerService;
        _logger = logger;
       
    }



    public async Task SetAPIData()
    {
        var list = _PrinterService.GetPrinters();
        var objList = new List<PrinterInfoFull_Agile>();
        List<Model_Attributes> attributes = await _PrinterService.GetAllAttributes();
        List<MODEL> models = await _PrinterService.GetModels();

        FullCount = 0;

        _logger.LogInformation($"Starting API Calls...{DateTime.Now}");

        foreach (var printer in list)
        {
            IPrinter stats = await API_Data_Collection(printer,attributes,models.FirstOrDefault(x=>x.MODEL_ID==printer.MODEL_ID));

            if (stats == null) {
                stats = new Error_Dis(){ 
                    HOST = printer.HOSTNAMEv4, 
                    STATUS = SD.Status__SystemNULL,
                    ERROR = SD.Status__SystemNULL_MSG 
                    };
            }

            // Adds the DB elements with the API results into one object. 
            objList.Add(new PrinterInfoFull_Agile(){ 
                printerOBJ = printer, 
                PrinterStats = stats 
            });

            FullCount++;
        }

        _logger.LogInformation($"Setting| {objList.Count()} obtained..");

        FullPrinterList = objList;

        _logger.LogInformation($"APIList Updated...{DateTime.Now}");
        foreach (var printer in FullPrinterList)
        {
            _logger.LogInformation($"Item| {printer.printerOBJ.HOSTNAMEv4} Complete...{DateTime.Now}");
        }

    }

    public async Task<IPrinter> API_Data_Collection(PRINTER_MAIN printer, List<Model_Attributes> attributes,MODEL modelObj)
    {
        var hostname = printer.HOSTNAMEv4;
        if (modelObj == null) {
            return new Error_Dis() { HOST = hostname, STATUS = SD.Status__SystemNULL, ERROR = $"Model obj_{printer.MODEL_ID} found null." };
        }

        try {

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {

                    return true;   //Is valid

                };

                using (HttpClient client = new HttpClient(handler))
                {

                    client.Timeout = TimeSpan.FromSeconds(SD.Timeout);
                    client.DefaultRequestHeaders.Add(SD.REQ_Header, SD.VALID);

                    var template = await _PrinterService.GetTemplate(modelObj.API_template);
                    
                    string mode = template.Attribute_Search_Mode;
                    Console.WriteLine(printer.QUE+": "+mode);





                    var path = string.Empty;

                    switch (mode)
                    {
                        case "SNMP":
                            path = $"https://{SD.APIServer1}:{SD.APIPort1}/endpoints/API-Template-Data/";
                            break;
                        case "HTTP":
                            path = $"https://{SD.APIServer2}:{SD.APIPort2}/endpoints/API-Template-Data/";
                            break;
                        default:
                            path = $"https://{SD.APIServer1}:{SD.APIPort1}/endpoints/API-Template-Data/";
                            break;
                    }

                    var body = await GetAPIBody(attributes.Where(x=>x.Model_ID==printer.MODEL_ID).ToList(),printer.HOSTNAMEv4);
                    string json = JsonConvert.SerializeObject(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    IPrinter sender;

                    try
                    {
                        HttpResponseMessage response = await client.PostAsync(path, content);
                        if (response.IsSuccessStatusCode)
                        {

                            var responseStream = await response.Content.ReadAsStreamAsync();  
                            
                            try
                            {

                                switch (modelObj.API_template)
                                {
                                    //This is responsible for converting the JSON response into an object of type T. All return properties are of type STRING. 
                                    //Each Template must have a define case and and class.

                                    case "MSP":
                                        sender = await JsonSerializer.DeserializeAsync<MSP>(responseStream);
                                        break;

                                    case "STA":
                                        sender = await JsonSerializer.DeserializeAsync<STA>(responseStream);
                                        break;

                                    default:
                                        sender = null;
                                        break;

                                }

                                return sender;

                            } 
                                                   
                            catch (Exception ex) {
                                Console.WriteLine(printer.QUE+":  "+ ex);
                                return new Error_Dis() { HOST = hostname, STATUS = SD.Status__SystemError, ERROR = ex.Message };

                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);

                        return new Error_Dis() { HOST= hostname, STATUS=SD.Status__SystemError,ERROR=ex.Message};
                    }
                }
            }
        } catch (Exception ex) {

            return new Error_Dis() { HOST = hostname, STATUS = SD.Status__SystemError,ERROR = ex.Message};
        }
      
        
    }

    internal async Task<List<API_Sender>> GetAPIBody(List<Model_Attributes> attributes, string hostname)
    {
        List<API_Sender> result = new();

        if (attributes != null)
        {
            result.Add(new() { ATTR_NAME = "HOST", ATTR_TYPE = "HOST", ATTR_VALUE = hostname});
            foreach (var attribute in attributes)
            {
                result.Add(new API_Sender { ATTR_NAME = attribute.Attribute_Name, ATTR_VALUE = attribute.Attribute_Value, ATTR_TYPE = attribute.Attribute_Type });
            }
        }

        return result;

    }

    // satic returns
    public List<PrinterInfoFull_Agile> GetFullList()
    {
        return FullPrinterList;

    }

    public int GetFullCount()
    {
        return FullCount;
    }




    //Host ran Task on Timer--------------------------------------------------------
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            //await SetAPIList();
            await SetAPIData();

            await Task.Delay(10000, stoppingToken);
        }      
    }


}

        

    
   
