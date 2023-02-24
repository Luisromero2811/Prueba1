using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prueba1.Server;
using Prueba1.Server.Helpers;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));


builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));
//Clases que van a identificar al usuario y al role
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//Configuracion para que el sistema use la auth mediante JsonWebTokens
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //Configuracion de JsonWebToken, el como lo vamos a manejar
    .AddJwtBearer(
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        //Validaciones de emisores y audiencia en false, en este caso no se usarán 
        ValidateIssuer = false,
        ValidateAudience = false,
        //Si validaremos el ciclo de vida del token
        ValidateLifetime = true,
        //Validamos la llave de firma del emisor el cual nos garantiza que nadie se meta con el Jwt
        ValidateIssuerSigningKey = true,
        //Configuracion de la llave
        IssuerSigningKey = new SymmetricSecurityKey(
            //Pasamos la llave secreta de un proveedor de auth
            Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"]!)),
        //Config para no tener errores de tiempo con el parametro de LifeTime
        ClockSkew = TimeSpan.Zero
    }
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

