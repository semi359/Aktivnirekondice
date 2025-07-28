using ClankyWeb.Data;
using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<NUser, NRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;//true by chtelo potvrzeni registrace
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.Configure<SecurityStampValidatorOptions>(options => {
    options.ValidationInterval = TimeSpan.FromSeconds(10);
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AutorPolicy", p => p.RequireRole(NRole.Autor, NRole.Admin));
    options.AddPolicy("AdminPolicy", p => p.RequireRole(NRole.Admin));
});


builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeAreaFolder("Autor", "/", "AutorPolicy");
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminPolicy");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

await app.Services
    .GetRequiredService<IServiceScopeFactory>()
    .CreateScope()
    .ServiceProvider
    .GetService<ApplicationDbContext>()
    .Database
    .MigrateAsync();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var um = services.GetRequiredService<UserManager<NUser>>();
    var rm = services.GetRequiredService<RoleManager<NRole>>();

    string[] roles = new[] { NRole.Admin, NRole.Autor, NRole.Ctenar };
    foreach (var role in roles)
    {
        if (!await rm.RoleExistsAsync(role))
            await rm.CreateAsync(new NRole { Name = role });
    }
    if (!db.Users.Any())
    {
        var admin = new NUser
        {
            UserName = "admin@email.cz",
            Email = "admin@email.cz",
            NickName = "admin@email.cz"
        };
        await um.CreateAsync(admin, "Admin-123");
        await um.AddToRoleAsync(admin, NRole.Admin);

        var autor = new NUser
        {
            UserName = "autor@email.cz",
            Email = "autor@email.cz",
            NickName = "autor@email.cz"
        };
        await um.CreateAsync(autor, "Autor-123");
        await um.AddToRoleAsync(autor, NRole.Autor);

        var ctenar = new NUser
        {
            UserName = "ctenar@email.cz",
            Email = "ctenar@email.cz",
            NickName = "ctenar@email.cz"
        };
        await um.CreateAsync(ctenar, "Ctenar-23");
        await um.AddToRoleAsync(ctenar, NRole.Ctenar);

        db.Clanky.AddRange(new[]
        {
            new Clanek {
                Titulek = "Ukázkový èlánek 1",
                Perex   = "Krátký úvod do èlánku 1",
                Text    = "Detailní text èlánku 1 …",
                UrlKod  = "clanek-1",
                Datum   = DateTime.Today.AddDays(-2),
                AutorId = autor.Id
            },
            new Clanek {
                Titulek = "Ukázkový èlánek 2",
                Perex   = "Úvod k druhému èlánku",
                Text    = "Detailní text èlánku 2 …",
                UrlKod  = "clanek-2",
                Datum   = DateTime.Today.AddDays(-1),
                AutorId = autor.Id
            },
            new Clanek {
                Titulek = "Ukázkový èlánek 3",
                Perex   = "Perex pro tøetí èlánek",
                Text    = "Detailní text èlánku 3 …",
                UrlKod  = "clanek-3",
                Datum   = DateTime.Today,
                AutorId = autor.Id
            }
        });

        db.ReservationEvents.AddRange(new[]
        {
            new ReservationEvent {
                Title       = "Ukázková událost 1",
                Date        = DateTime.Today.AddDays(3),
                Capacity    = 10,
                Description = "Popis události 1 …"
            },
            new ReservationEvent {
                Title       = "Ukázková událost 2",
                Date        = DateTime.Today.AddDays(7),
                Capacity    = 20,
                Description = "Popis události 2 …"
            },
            new ReservationEvent {
                Title       = "Ukázková událost 3",
                Date        = DateTime.Today.AddDays(14),
                Capacity    = 15,
                Description = "Popis události 3 …"
            }
        });

        await db.SaveChangesAsync();
    }
}

app.Run();
