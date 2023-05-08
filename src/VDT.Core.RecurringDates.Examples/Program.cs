using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace VDT.Core.RecurringDates.Examples {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            await builder.Build().RunAsync();
        }
    }
}
