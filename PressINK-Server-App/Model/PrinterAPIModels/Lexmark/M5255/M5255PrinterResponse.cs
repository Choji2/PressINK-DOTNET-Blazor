using Web_scraper_practice_.Model.Printer_DB_Models;

namespace PressINK_Server_App.Model.PrinterAPIModels.Lexmark.M5255
{
    public class M5255PrinterResponse
    {
        public string HOSTNAME { get; set; }

        public string MODEL { get; set; }

        public bool ONLINE { get; set; }

        public string STATUS { get; set; }

        public string WARNING { get; set; }

        public string MESSAGES { get; set; }

        public int INK_LEVEL { get; set; }

        public int IMAGING { get; set; }

        public int MAINT_KIT { get; set; }
    }
}
