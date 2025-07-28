using Microsoft.AspNetCore.Identity;

namespace ClankyWeb.Data;

public class NRole : IdentityRole<int>
{
    public const string Admin = "admin";
    public const string Autor = "author";
    public const string Ctenar = "reader";

    public NRole() { }
    public NRole(string roleName) : base(roleName) { }
}
