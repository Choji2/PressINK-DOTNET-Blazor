using Web_scraper_practice_.Model.Printer_DB_Models;

namespace PressINK_Server_App.Model.PrinterPingModel

{
    public class PingModel
    {

        public string Host { get; set; }
        public bool Online { get; set; }
        public long response_time { get; set; }
        public string response_status { get; set; }

        public PRINTER_MAIN PRINTER_MAIN { get; set; }



    }
}
