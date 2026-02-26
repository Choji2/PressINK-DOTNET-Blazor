using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;
public class PLANT
{
    [Key][Column(TypeName = "varchar(5)")][Required]
    [StringLength(5, ErrorMessage = "String should be no more than 5 characters.")] public string PLANT_ID { get; set; } = null!;
    [Required]
    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string NAME { get; set; } = null!;

    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string DESCP { get; set; } = null!;


}
