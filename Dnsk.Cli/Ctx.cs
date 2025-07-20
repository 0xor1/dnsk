using ConsoleAppFramework;
using IApi = Dnsk.Api.IApi;

namespace Dnsk.Cli;

public class Ctx
{
    private readonly IApi _api;

    public Ctx(IApi api)
    {
        _api = api;
    }

    [Command("register")]
    public async Task Register() { }
}
