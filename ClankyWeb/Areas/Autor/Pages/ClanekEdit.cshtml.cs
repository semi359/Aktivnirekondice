using ClankyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Areas.Autor.Pages;

public class ClanekEditModel : PageModel
{
    readonly ApplicationDbContext DB;
   
    [BindProperty(SupportsGet = true)]
    public int IdClanku { get; set; }

    [BindProperty]
    public Clanek Data { get; set; }


    public ClanekEditModel(ApplicationDbContext db)
    {
        DB = db;
    }

    public async Task OnGetAsync()
    {
        if (IdClanku == 0)
        {
            Data = new Clanek() { Datum = DateTime.Today };
            return;
        }
        else
        {
            Data = await DB.Clanky
                .AsNoTracking()
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == IdClanku);

            if (!User.IsInRole(NRole.Admin) &&
                Data.Autor.UserName != User.Identity.Name)
                Response.Redirect($"/clanek/{Data.UrlKod}");
        }
    }

    public async Task OnPostAsync()
    {
        if (IdClanku != Data?.Id)
            return;

        try
        {
            if (IdClanku == 0)
            {
                var currentUser = await DB.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.UserName == User.Identity.Name);

                Data.AutorId = currentUser.Id;
                await DB.Clanky.AddAsync(Data);
            }
            else
            {
                var clanek = await DB.Clanky
                    .AsNoTracking()
                    .Include(x => x.Autor)
                    .FirstOrDefaultAsync(x => x.Id == IdClanku);
                Data.Autor = clanek.Autor;

                if (clanek.Autor.UserName != User.Identity.Name)
                    throw new Exception("Nemáte oprávnìní k úpravì tohoto èlánku");

                DB.Clanky.Update(Data);
            }
            await DB.SaveChangesAsync();
            Response.Redirect($"/clanek/{Data.UrlKod}");
        }
        catch (Exception ex)
        {
            if (!Utils.IsUniqueException(ex, "Clanky", "UrlKod"))
                throw;
            ModelState.AddModelError(string.Empty, $"URL kód '{Data.UrlKod}' je již obsazený");
        }
    }

    public async Task OnPostVymazatAsync(string idClanku)
    {
        if (IdClanku != Convert.ToInt32(idClanku))
            return;
        DB.Clanky.Remove(await DB.Clanky.FindAsync(IdClanku));
        await DB.SaveChangesAsync();
        Response.Redirect($"/");
    }
}
