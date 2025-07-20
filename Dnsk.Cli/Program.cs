﻿using System.Text.Json;
using Common.Shared;
using ConsoleAppFramework;
using Dnsk.Api;
using Dnsk.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ZLogger;

var app = ConsoleApp.Create();
app.ConfigureServices(services =>
{
    services.AddLogging(b =>
    {
        b.ClearProviders();
        b.AddZLoggerConsole(x => x.LogToStandardErrorThreshold = LogLevel.Error);
        b.SetMinimumLevel(LogLevel.Information);
    });
    services.AddScoped<ISerializer>(s =>
        new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build()
    );
    services.AddScoped<State>(s =>
    {
        var state = new State();
        var appDataDir = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolderOption.Create
        );
        var stateDir = Path.Join(appDataDir, "dnsk");
        Directory.CreateDirectory(stateDir);
        var stateFilePath = Path.Join(stateDir, "state.json");
        if (!File.Exists(stateFilePath))
        {
            File.Create(stateFilePath).Close();
        }

        var stateStr = File.ReadAllText(stateFilePath);
        if (!stateStr.IsNullOrEmpty())
        {
            state = JsonSerializer.Deserialize<State>(stateStr);
            if (state == null || state.BaseHref.IsNullOrEmpty())
            {
                state = new State();
            }
        }
        state.CookieContainer.Add(state.Cookies);

        return state;
    });
    services.AddScoped<IRpcClient>(s =>
    {
        var state = s.GetRequiredService<State>();
        var l = s.GetRequiredService<ILogger<IRpcClient>>();
        return new RpcHttpClient(
            state.BaseHref,
            new HttpClient(
                new HttpClientHandler()
                {
                    CookieContainer = state.CookieContainer,
                    UseCookies = true,
                }
            ),
            (s) =>
            {
                l.LogError(s);
            },
            true
        );
    });
    services.AddScoped<IApi, Api>();
});
app.UseFilter<StateFilter>();
app.Add<App>("app");
app.Add<Auth>("auth");
app.Add<Counter>("counter");
await app.RunAsync(args);
