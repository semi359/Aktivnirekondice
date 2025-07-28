using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClankyWeb.Data;

namespace ClankyWeb.Areas.Admin.Pages;

public class UsersModel : PageModel
{
    public List<UserDataForAdmin> Data { get; set; }

    readonly ApplicationDbContext DB;
    readonly UserManager<NUser> userManager;
    readonly RoleManager<NRole> roleManager;

    public UsersModel(
        ApplicationDbContext db,
        UserManager<NUser> userManager,
        RoleManager<NRole> roleManager
        )
    {
        DB = db;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task LoadUsers()
    {
        Data = (await DB.Users
            .AsNoTracking()
            .OrderBy(x => x.NickName).ToListAsync())
           .Select(x => new UserDataForAdmin()
           {
               UserName = x.UserName,
               NickName = x.NickName,
               Email = x.Email,
               BanEnds = x.LockoutEnd
           }).ToList();

        // Role a uživatelé (jejich userName) v ní
        var roles = roleManager.Roles.Select(x => x.Name);
        var usersInRole = new Dictionary<string, string[]>();
        foreach (var role in roles)
            usersInRole.Add(role, (await userManager.GetUsersInRoleAsync(role))
                .Select(x => x.UserName).ToArray());

        // Pøidání pole rolí k uživatelùm
        foreach (var user in Data)
            user.Roles = usersInRole.Where(x => x.Value.Contains(user.UserName))
                .Select(x => x.Key).ToArray();
    }

    public async Task OnGet()
    {
        await LoadUsers();
    }

    public async Task OnPostRoleAsync(string userName, string roleName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (await userManager.IsInRoleAsync(user, roleName))
            await userManager.RemoveFromRoleAsync(user, roleName);
        else 
            await userManager.AddToRoleAsync(user, roleName);
        await LoadUsers();
    }

    delegate DateTimeOffset? UpravEndDelegate(DateTimeOffset? end);

    async Task Banovani(string user, UpravEndDelegate upravEnd)
    {
        var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == user);
        if (userDb != null)
        {
            var end = userDb.LockoutEnd;
            end = upravEnd(end);
            await userManager.SetLockoutEndDateAsync(userDb, end);
            if (end != null && end > DateTimeOffset.Now)
                await userManager.UpdateSecurityStampAsync(userDb);
        }
        await LoadUsers();
    }
  

    public async Task OnPostBanBanAsync(string user)
    {
        await Banovani(user, (end) => {
            return DateTimeOffset.MaxValue;
        });
    }

    public async Task OnPostBanMilostAsync(string user)
    {
        await Banovani(user, (end) => {
            return null;
        });
    }
}

public record UserDataForAdmin
{
    public string NickName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }
    public DateTimeOffset? BanEnds { get; set; }

}
