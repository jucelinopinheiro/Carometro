using CarometroV7.Data;
using CarometroV7.Data.Interfaces;
using CarometroV7.Data.Repositories;
using CarometroV7.Helper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to Data Base
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// Add services interfaces
builder.Services.AddTransient<IUsuario, UsuarioRepository>();
builder.Services.AddTransient<ICurso,CursoRepository>();
builder.Services.AddTransient<ITurma, TurmaRepository>();
builder.Services.AddTransient<IAluno, AlunoRepository>();
builder.Services.AddTransient<IMatricula, MatriculaRepository>();
builder.Services.AddTransient<IOcorrencia, OcorrenciaRepository>();
builder.Services.AddTransient<IAnexoOcorrencia, AnexoOcorrenciaRepository>();




//injetando as dependencia de sessao, quando chamar o IHttpContextAccessor vai emplementar o HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//serviços de e-mail
builder.Services.AddScoped<Email>();

// injetando as dependencias
builder.Services.AddScoped<ISessao, Sessao>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//habilitando a sessao
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
