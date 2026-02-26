using System.ComponentModel.DataAnnotations;

namespace PressINK_Server_App.Model.API_Form_Models
{
    public class API_Templates
    {
        [Key]
        public string ID {  get; set; }

        [Required]
        public string Template_Name { get; set; }

        [Required]
        public string Template_Description { get; set;}

        [Required]
        public ICollection<API_Template_Attributes> Attributes { get; set; } = new List<API_Template_Attributes>();

        [Required]
        public string Attribute_Search_Mode { get; set; }

    }
}
