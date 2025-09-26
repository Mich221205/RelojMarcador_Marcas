using RelojMarcador_Marcas.BusinessLogic;
using RelojMarcador_Marcas.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("MySql")
    ?? throw new InvalidOperationException("Cadena de conexión 'MySql' no encontrada.");

builder.Services.AddScoped<MarcaRepository>(sp =>
    new MarcaRepository(connectionString));


builder.Services.AddScoped<MarcaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

//app.MapGet("/test-db", (MarcaRepository repo) => repo.ProbarConexion());

app.MapGet("/", context =>
{
    context.Response.Redirect("/Marcas");
    return Task.CompletedTask;
});


app.Run();
