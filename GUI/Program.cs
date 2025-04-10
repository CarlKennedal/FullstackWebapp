using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using FullstackWebapp.GUI.Services;
using System.Net.Http.Headers;
using System.Text.Json;
using GUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var appSettingsResponse = await http.GetAsync("appsettings.Development.json");
var appSettingsJson = await appSettingsResponse.Content.ReadAsStringAsync();
var appSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(appSettingsJson);
var apiBaseUrl = appSettings["ApiBaseUrl"].GetString();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
});

builder.Services.AddScoped<ApiService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLogging();

await builder.Build().RunAsync();