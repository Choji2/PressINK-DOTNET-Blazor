using PressINK_Server_App.Model.API_Data.Return_Types;
using PressINK_Server_App.Model.PrinterAPIModels.Lexmark.M5255;
using Web_scraper_practice_.Model.Printer_DB_Models;

namespace PressINK_Server_App.Model.PrinterAPIModels;

public class PrinterInfoFull
{
    public PRINTER_MAIN printerOBJ { get; set; }
    public M5255PrinterResponse PrinterStats { get; set; }
}


public class PrinterInfoFull_Agile // Takes a printer obj.  and a instance of IPrinter. to build master list object. 
{
    public PRINTER_MAIN printerOBJ { get; set; }
    public IPrinter PrinterStats { get; set; }

}
