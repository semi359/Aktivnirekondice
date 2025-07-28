using System.ComponentModel.DataAnnotations;

namespace ClankyWeb.Data;

public record Komentar
{
    [Key]
    public int Id { get; set; }


    public int ClanekId { get; set; }
    public Clanek Clanek { get; set; }

    public int VlozilId { get; set; }
    public NUser Vlozil { get; set; }

    public DateTime Datum { get; set; }


    [MaxLength, Required, DataType(DataType.MultilineText)]
    public string Text { get; set; }
}
