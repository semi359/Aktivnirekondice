using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClankyWeb.Data;

[Index(nameof(UrlKod), IsUnique = true)]
public record Clanek
{
    [Key]
    public int Id { get; set; }

    public DateTime Datum { get; set; }

    [MaxLength(100), Required]
    public string Titulek { get; set; }

    [MaxLength(1024)]
    public string? Perex { get; set; }

    [MaxLength, DataType(DataType.MultilineText)]
    public string? Text { get; set; }


    [MaxLength(50), Required, Display(Name = "URL kód")]
    public string UrlKod { get; set; }

    public int AutorId { get; set; }
    public NUser Autor { get; set; }


    public ICollection<Komentar> Komentare { get; set; }
}
