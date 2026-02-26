using System.ComponentModel.DataAnnotations;
using Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;

namespace PressINK_Server_App.Model.API_Form_Models
{
    public class Model_Attributes
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Attribute_Name { get; set; }

        [Required]
        public string Attribute_Type { get; set; }
        [Required]
        public string Attribute_Value { get; set; } = null;

        [Required]
        public string Model_ID { get; set; }

    }
}
