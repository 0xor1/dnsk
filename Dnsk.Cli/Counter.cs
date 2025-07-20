using ConsoleAppFramework;
using Dnsk.Api;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dnsk.Cli;

public class Counter
{
    private readonly IApi _api;
    private readonly ISerializer _serializer;

    public Counter(IApi api)
    {
        _api = api;
        _serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }
    
    [Command("increment")]
    public async Task Increment()
    {
        var c = await _api.Counter.Increment();
        Console.WriteLine(_serializer.Serialize(c));
    }
    
    [Command("decrement")]
    public async Task Decrement()
    {
        var c = await _api.Counter.Decrement();
        Console.WriteLine(_serializer.Serialize(c));
    }

    [Command("get")]
    public async Task Get([Argument] string? user = null)
    {
        var c = await _api.Counter.Get(new (){User = user});
        Console.WriteLine(_serializer.Serialize(c));
    }
}