using System.Net;

namespace Dnsk.Cli;

[Serializable]
public class State
{
    public string BaseHref { get; set; } = "https://dnsk.dans-demos.com/";

    public CookieCollection Cookies { get; set; } = new();
    public CookieContainer CookieContainer { get; set; } = new();

    public Dictionary<string, string> Ctx { get; set; } = new();
}
