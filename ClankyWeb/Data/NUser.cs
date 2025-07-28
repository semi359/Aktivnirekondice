using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClankyWeb.Data;

[Index(nameof(NickName), IsUnique = true)]
public class NUser : IdentityUser<int>
{
    [Required, MaxLength(50)]
    public string NickName { get; set; }
}
