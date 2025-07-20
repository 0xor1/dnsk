using System.Text.Json;
using ConsoleAppFramework;
using IApi = Dnsk.Api.IApi;

namespace Dnsk.Cli;

class StateFilter([FromServices] State state, [FromServices] IApi api, ConsoleAppFilter next)
    : ConsoleAppFilter(next)
{
    public override async Task InvokeAsync(ConsoleAppContext ctx, CancellationToken ctkn)
    {
        try
        {
            await Next.InvokeAsync(ctx with { State = state }, ctkn);
        }
        finally
        {
            // always save state back to ensure cookie container state is current
            state.Cookies = state.CookieContainer.GetAllCookies();
            var appDataDir = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create
            );
            var stateDir = Path.Join(appDataDir, "dnsk");
            var filePath = Path.Join(stateDir, "state.json");
            var stateJson = JsonSerializer.Serialize(
                state,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(filePath, stateJson, ctkn);
        }
    }
}
