using RelojMarcador.API.DataAccess;
using RelojMarcador.API.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<MarcaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

