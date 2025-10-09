using IApi = Dnsk.Api.IApi;

namespace Dnsk.Cli;

public class App
{
    private readonly IApi _api;

    public App(IApi api)
    {
        _api = api;
    }

    /// <summary>
    /// Get the app configuration
    /// </summary>
    public async Task GetConfig(CancellationToken ctkn = default) =>
        Io.WriteYml(await _api.App.GetConfig(ctkn));
}
