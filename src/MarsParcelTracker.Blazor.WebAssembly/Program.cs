using MarsParcelTracker.Blazor.WebAssembly;
using MarsParcelTracker.Blazor.WebAssembly.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.ComponentModel.Design;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7168") });

//builder.Services.AddScoped<IParcelService, ParcelService>();
builder.Services.AddScoped<IParcelService, HttpParcelService>();
builder.Services.AddScoped<IMessageService, MessageService>();

await builder.Build().RunAsync();
