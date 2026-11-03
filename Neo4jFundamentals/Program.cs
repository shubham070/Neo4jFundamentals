using System.Text.Json;
using Neo4j.Driver;

class Program
{
    public static async Task Main()
    {
        var configuration = await JsonSerializer.DeserializeAsync<Neo4jConfiguration>(
            File.OpenRead("appsettings.json"))
            ?? throw new InvalidOperationException("Could not load appsettings.json.");

        await using var driver = GraphDatabase.Driver(
            configuration.Uri,
            AuthTokens.Basic(configuration.Username, configuration.Password));

        Console.WriteLine("Welcome Running the first Example simple datatype mapping and Object Data Type");
        await QueriesExamples1.RunNeo(driver);

        Console.WriteLine("Welcome Running the Second Example");
        await QueriesExamples2.RunNeo(driver);

        Console.WriteLine("Welcome Running the Third Example");
        await QueriesExamples3.RunNeo(driver);

        await driver.DisposeAsync();
        Console.WriteLine("disposed successfully connection closed");
    }
}

