using ConsoleAppFramework;
using Common.Shared.Auth;
using IApi = Dnsk.Api.IApi;

namespace Dnsk.Cli;

public class Auth
{
    private readonly IApi _api;

    public Auth(IApi api)
    {
        _api = api;
    }

    private string GetSensitiveValue(string promptText)
    {
        Console.Write(promptText);
        var val = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && val.Length > 0)
            {
                Console.Write("\b \b");
                val = val[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                val += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return val;
    }
    
    [Command("register")]
    public async Task Register([Argument] string email)
    {
        var pwd = GetSensitiveValue("Enter Password: ");
        var confirmPwd = GetSensitiveValue("Confirm Password: ");
        if (pwd != confirmPwd)
        {
            Console.WriteLine("Passwords do not match.");
        }
        await _api.Auth.Register(new Register(email, pwd));
        Console.WriteLine($"An email has been sent to {email}\nPlease use the provided link to complete account setup.");
    }
    
    [Command("login")]
    public async Task SignIn([Argument] string email, [Argument] bool rememberMe = true)
    {
        var pwd = GetSensitiveValue("Enter Password: ");
        await _api.Auth.SignIn(new SignIn(email, pwd, rememberMe));
    }
    
    [Command("logout")]
    public async Task SignOut(CancellationToken ctkn = default)
    {
        await _api.Auth.SignOut(ctkn);
    }
}