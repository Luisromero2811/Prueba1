﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Prueba1.Client;
using Prueba1.Client.Repositorios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
ConfigureServices(builder.Services);



await builder.Build().RunAsync();
void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IRepositorio, Repositorio>();
}
