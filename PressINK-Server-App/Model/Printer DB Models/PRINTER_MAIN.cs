using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_scraper_practice_.Model.Printer_DB_Models;

[Index(nameof(QUE), IsUnique = true)]
public class PRINTER_MAIN
{
    [Key]public int ID { get; set; }
    [Required][Column(TypeName = "varchar(15)")]
    [StringLength(15, ErrorMessage = "String should be no more than 15 characters. EX. 000.000.000.000")] public string HOSTNAMEv4 { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(50)")] 
    [StringLength(50,ErrorMessage ="String should be no more than 50 characters.")]public string ASSET_NUMBER { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string QUE { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    [StringLength(20, ErrorMessage = "String should be no more than 20 characters.")] public string LOCATION { get; set; }

    [Required][Column(TypeName = "varchar(10)")]
    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string MODEL_ID { get; set; }

    [Required]
    [Column(TypeName = "varchar(5)")]
    [StringLength(5, ErrorMessage = "String should be no more than 5 characters.")] public string PLANT_ID { get; set; }

    [Required]
    [Column(TypeName = "varchar(5)")]
    [StringLength(5, ErrorMessage = "String should be no more than 5 characters.")] public string CATEGORY_ID { get; set; }

}
