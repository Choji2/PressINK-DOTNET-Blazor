using System.ComponentModel.DataAnnotations;

namespace PressINK_Server_App.Model.API_Form_Models
{
    public class Attribute_Type
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }

}
