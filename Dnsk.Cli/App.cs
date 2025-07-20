using Common.Shared.Auth;
using ConsoleAppFramework;
using YamlDotNet.Serialization;
using IApi = Dnsk.Api.IApi;

namespace Dnsk.Cli;

public class App
{
    private readonly IApi _api;
    private readonly ISerializer _serializer;

    public App(IApi api, ISerializer serializer)
    {
        _api = api;
        _serializer = serializer;
    }

    /// <summary>
    /// Get the app configuration
    /// </summary>
    public async Task GetConfig()
    {
        var config = await _api.App.GetConfig();
        Console.WriteLine(_serializer.Serialize(config));
    }
}
