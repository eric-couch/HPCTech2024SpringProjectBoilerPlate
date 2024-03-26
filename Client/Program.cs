using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HPCTech2024SpringProjectBoilerPlate.Client;
using Syncfusion.Blazor;
using HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;
using Blazored.LocalStorage;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE4Mzc1MkAzMjM1MmUzMDJlMzBJRW5EMEpOWXlLOFVTTWlHdTYvMzZmWDhsSlZxNHdlODJOcE9nZ3RNQXR3PQ== ");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("HPCTech2024SpringProjectBoilerPlate.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HPCTech2024SpringProjectBoilerPlate.ServerAPI"));

builder.Services.AddScoped<IUserHttpRepository, UserMoviesHttpRepository>();
builder.Services.AddApiAuthorization();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
