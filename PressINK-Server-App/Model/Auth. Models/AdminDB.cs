using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PressINK_Server_App.Model.Auth._Models
{
    public class AdminDB
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string USER_NAME { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LAST_NAME { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FIRST_NAME { get; set; }

    }
}
