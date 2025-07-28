using ClankyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Pages;

public class IndexModel : PageModel
{
    readonly ApplicationDbContext DB;

    public List<Clanek> Data { get; set; } 

    public IndexModel(ApplicationDbContext db)
    {
        DB = db;
    }

    public async Task OnGetAsync()
    {
        Data = await DB.Clanky
            .AsNoTracking()
            .Include(c => c.Autor)
            .OrderByDescending(c => c.Datum)
            .ToListAsync();
    }
}
