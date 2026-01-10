using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Asistencia.Web;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Asistencia.Web.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddBlazorBootstrap();

// HTTP Client
builder.Services.AddTransient<JwtHeaderHandler>();
builder.Services.AddHttpClient("AsistenciaApi", client => client.BaseAddress = new Uri("https://localhost:7210"))
    .AddHttpMessageHandler<JwtHeaderHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AsistenciaApi"));

await builder.Build().RunAsync();
