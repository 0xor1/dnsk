using Common.Client;
using Common.Server.Test;
using Dnsk.Api;
using Dnsk.Db;
using Dnsk.Eps;
using Radzen;
using S = Dnsk.I18n.S;

namespace Dnsk.Client.Test;

public class BaseTests : IDisposable
{
    protected readonly RpcTestRig<DnskDb, Api.Api> RpcTestRig;
    protected readonly List<TestPack> TestPacks = new();

    public BaseTests()
    {
        RpcTestRig = new RpcTestRig<DnskDb, Api.Api>(S.Inst, DnskEps.Eps, c => new Api.Api(c));
    }

    protected async Task<TestPack> NewTestPack(string name)
    {
        var (api, _, _) = await RpcTestRig.NewApi(name);
        var ctx = new TestContext();
        var l = new Localizer(S.Inst);
        var ns = new NotificationService();
        Common.Client.Client.Setup(ctx.Services, l, S.Inst, ns, (IApi)api);
        var tp = new TestPack(api, ctx);
        TestPacks.Add(tp);
        return tp;
    }

    public void Dispose()
    {
        RpcTestRig.Dispose();
        TestPacks.ForEach(x => x.Ctx.Dispose());
    }
}

public record TestPack(IApi Api, TestContext Ctx);
