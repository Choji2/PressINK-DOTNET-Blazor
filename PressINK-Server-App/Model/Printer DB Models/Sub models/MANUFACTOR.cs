using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_scraper_practice_.Model.Printer_DB_Models;
public class MANUFACTOR
{
    [Key][Column(TypeName = "varchar(10)")][StringLength(10, ErrorMessage = "String should be no more than 10 characters.")] public string MANUFACTOR_ID { get; set; }

    [Required][StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string NAME { get; set; } = null!;

    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string DESCP { get; set; }

}
