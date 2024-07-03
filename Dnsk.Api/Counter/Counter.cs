﻿using Common.Shared;
using MessagePack;

namespace Dnsk.Api.Counter;

public interface ICounterApi
{
    public Task<Counter> Get(Get get, CancellationToken ctkn = default);
    public Task<Counter> Increment(CancellationToken ctkn = default);
    public Task<Counter> Decrement(CancellationToken ctkn = default);
}

public class CounterApi : ICounterApi
{
    private readonly IRpcClient _client;

    public CounterApi(IRpcClient client)
    {
        _client = client;
    }

    public Task<Counter> Get(Get req, CancellationToken ctkn = default) =>
        _client.Do(CounterRpcs.Get, req, ctkn);

    public Task<Counter> Increment(CancellationToken ctkn = default) =>
        _client.Do(CounterRpcs.Increment, Nothing.Inst, ctkn);

    public Task<Counter> Decrement(CancellationToken ctkn = default) =>
        _client.Do(CounterRpcs.Decrement, Nothing.Inst, ctkn);
}

public static class CounterRpcs
{
    public static readonly Rpc<Get, Counter> Get = new("/counter/get");
    public static readonly Rpc<Nothing, Counter> Increment = new("/counter/increment");
    public static readonly Rpc<Nothing, Counter> Decrement = new("/counter/decrement");
}

[MessagePackObject]
public record Counter([property: Key(0)] string User, [property: Key(1)] uint Value);

[MessagePackObject]
public record Get([property: Key(0)] string User);
