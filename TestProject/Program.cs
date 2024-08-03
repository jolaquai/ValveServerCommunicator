using ValveServerCommunicator;

namespace TestProject;

internal class Program
{
    public static async Task Main()
    {
        using var client = ValveServerClient.Create("82.165.203.125", 25568);
        var players = await client.QueryRulesAsync();

        await Task.Delay(-1);
    }
}
