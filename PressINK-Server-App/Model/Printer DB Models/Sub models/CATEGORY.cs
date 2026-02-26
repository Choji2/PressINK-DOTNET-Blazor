using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_scraper_practice_.Model.Printer_DB_Models.Sub_models;
public class CATEGORY
{
    [Key][Column(TypeName = "varchar(5)")][StringLength(5, ErrorMessage = "String should be no more than 5 characters.")] public string CATEGORY_ID { get; set; }

    [Required][StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string NAME { get; set; } = null!;

    [StringLength(50, ErrorMessage = "String should be no more than 50 characters.")] public string DESCRIPTION { get; set; } = null!;

}
