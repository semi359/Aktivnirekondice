using ClankyWeb.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Pages;

public class ClanekDetailModel : PageModel
{
    readonly ApplicationDbContext DB;

    [BindProperty(SupportsGet = true)]
    public string UrlKod { get; set; }

    public Clanek Data { get; set; }


    public ClanekDetailModel(ApplicationDbContext db)
    {
        DB = db;
    }

    public async Task OnGetAsync()
    {
        Data = await DB.Clanky
            .AsNoTracking()
            .Include(x => x.Autor)
            .Include(x => x.Komentare)
            .ThenInclude(x => x.Vlozil)
            .FirstOrDefaultAsync(x => x.UrlKod == UrlKod);
    }

    public async Task OnPostAsync(string komentar)
    {
        if (!String.IsNullOrWhiteSpace(komentar))
        {
            var clanek = await DB.Clanky
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UrlKod == UrlKod);
            var currentUser = await DB.Users.AsNoTracking().FirstOrDefaultAsync(
                x => x.UserName == User.Identity.Name);
            DB.Komentare.Add(new Komentar()
            {
                ClanekId = clanek.Id,
                Text = komentar,
                VlozilId = currentUser.Id,
                Datum = DateTime.Now
            });
            DB.SaveChanges();
            Response.Redirect(Request.GetDisplayUrl());
        }
    }

    public async Task OnPostVymazatAsync(string idKomentare)
    {
        if (!User.IsInRole(NRole.Admin))
            return;
        int id = Int32.Parse(idKomentare);
        DB.Komentare.Remove(await DB.Komentare.FindAsync(id));
        await DB.SaveChangesAsync();
        Response.Redirect(Request.GetDisplayUrl());
    }
}
