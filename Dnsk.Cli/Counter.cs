using ConsoleAppFramework;
using Dnsk.Api;
using YamlDotNet.Serialization;

namespace Dnsk.Cli;

public class Counter
{
    private readonly IApi _api;
    private readonly ISerializer _serializer;

    public Counter(IApi api, ISerializer serializer)
    {
        _api = api;
        _serializer = serializer;
    }

    /// <summary>
    /// Increment your counter
    /// </summary>
    public async Task Increment()
    {
        var c = await _api.Counter.Increment();
        Console.WriteLine(_serializer.Serialize(c));
    }

    /// <summary>
    /// Decrement your counter
    /// </summary>
    public async Task Decrement()
    {
        var c = await _api.Counter.Decrement();
        Console.WriteLine(_serializer.Serialize(c));
    }

    /// <summary>
    /// Get a users counter, defaults to yours
    /// </summary>
    /// <param name="user">The user id to get the counter for</param>
    public async Task Get([Argument] string? user = null)
    {
        var c = await _api.Counter.Get(new() { User = user });
        Console.WriteLine(_serializer.Serialize(c));
    }
}
