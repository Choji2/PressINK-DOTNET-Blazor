using System.ComponentModel.DataAnnotations;

namespace PressINK_Server_App.Model.API_Form_Models
{
    public class API_Template_Attributes
    {
        [Key]
        public int ID { get; set; }
        public API_Templates API_Templates { get; set; }

        [Required]
        public string Attribute_Name{ get; set; }
        [Required]
        public string Attribute_Description { get; set; }
        [Required]
        public string Attribute_Type { get; set; }

    }
}
