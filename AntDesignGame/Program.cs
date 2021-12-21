using AntDesign.JsInterop;
using AntDesignGame;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAntDesign();

builder.Services.TryAddScoped<DomEventService>();
builder.Services.TryAddTransient<IDomEventListener>((sp) =>
{
    var domEventService = sp.GetRequiredService<DomEventService>();
    return domEventService.CreateDomEventListerner();
});

await builder.Build().RunAsync();
