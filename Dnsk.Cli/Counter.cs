using ConsoleAppFramework;
using Dnsk.Api;

namespace Dnsk.Cli;

public class Counter
{
    private readonly IApi _api;

    public Counter(IApi api)
    {
        _api = api;
    }

    /// <summary>
    /// Increment your counter
    /// </summary>
    public async Task Increment() => Io.WriteYml(await _api.Counter.Increment());

    /// <summary>
    /// Decrement your counter
    /// </summary>
    public async Task Decrement() => Io.WriteYml(await _api.Counter.Decrement());

    /// <summary>
    /// Get a users counter, defaults to yours
    /// </summary>
    /// <param name="user">The user id to get the counter for</param>
    public async Task Get([Argument] string? user = null) =>
        Io.WriteYml(await _api.Counter.Get(new() { User = user }));
}
