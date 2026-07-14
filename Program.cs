using JogadoresApi.Data;
using Microting.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("JogadoresConnection");
builder.Services.AddDbContext<JogadorContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString!)));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseDefaultFiles(); // serve wwwroot/index.html como página inicial
app.UseStaticFiles();  // serve os arquivos da pasta wwwroot (HTML/CSS/JS)

app.MapControllers();
app.Run();
