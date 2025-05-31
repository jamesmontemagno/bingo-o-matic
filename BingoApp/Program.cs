using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BingoApp;
using BingoApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<IBrowserStorageService, IndexedDbService>();
builder.Services.AddSingleton<ILocalBrowserStorageService, LocalStorageService>();
builder.Services.AddSingleton<BingoSetService>();
builder.Services.AddSingleton<NavbarService>();
builder.Services.AddScoped<BingoCardService>();
builder.Services.AddSingleton<ThemeService>();
builder.Services.AddScoped<IGameStateService, GameStateService>();

await builder.Build().RunAsync();
