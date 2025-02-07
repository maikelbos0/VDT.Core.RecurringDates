using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VDT.Core.RecurringDates.Examples;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

await builder.Build().RunAsync();
