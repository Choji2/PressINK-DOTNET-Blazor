using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;
public class MODEL
{
    [Key][Required][Column(TypeName = "varchar(10)")]
    [StringLength(20, ErrorMessage = "String should be no more than 20 characters.")] public string MODEL_ID { get; set; } 


    [Required]
    [Column(TypeName = "varchar(20)")]
    [StringLength(20, ErrorMessage = "String should be no more than 20 characters.")] public string MODEL_NAME { get; set; }


    [Required][Column(TypeName = "varchar(10)")] public string MANUFACTOR_ID { get; set; }


    [Column(TypeName = "varchar(50)")]
    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string DESCP { get; set; } = null!;

    public string LINK { get; set; } = null;

    [Required]
    public string API_template { get; set; }

    


}
