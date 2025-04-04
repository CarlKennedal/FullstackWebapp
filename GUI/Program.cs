using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using FullstackWebapp.GUI.Services;
using GUI;

namespace FullstackWebapp.GUI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7278");
        });

        builder.Services.AddScoped<ApiService>();
        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddBlazoredLocalStorage();

        await builder.Build().RunAsync();
    }
}