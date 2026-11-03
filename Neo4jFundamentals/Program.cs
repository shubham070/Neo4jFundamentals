using System.Text.Json;
using Neo4j.Driver;

class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello");
        await Program.RunNeo();
    }

    public static async Task RunNeo()
    {
        Console.WriteLine("Hello, World!");

        var configuration = await JsonSerializer.DeserializeAsync<Neo4jConfiguration>(
            File.OpenRead("appsettings.json"))
            ?? throw new InvalidOperationException("Could not load appsettings.json.");

        await using var driver = GraphDatabase.Driver(
            configuration.Uri,
            AuthTokens.Basic(configuration.Username, configuration.Password));

        await driver.VerifyConnectivityAsync();
        Console.WriteLine("connection successful");


        var cypher = @"
                    MATCH (p:Person {name: $name})-[r:ACTED_IN]->(m:Movie)
                    RETURN m.title AS title, r.role AS role
                    ";

        // execute a query
        var result = await driver.ExecutableQuery(cypher)
        .WithParameters(new { name = "Tom Hanks" })
        // specify target databse using .WithConfig this is optional
        // also can be configured to readmode because ExecutableQuery runs writemode by default
        .WithConfig(
            new QueryConfig(
                database: "4101dc89",
                routing: RoutingControl.Readers))
        //WithMap() operator this is optional
        .WithMap((record) => $" Tom Hanks Played {record["role"]} in {record["title"]} Movie")
        // specify target databse using .WithConfig this is optional
        .ExecuteAsync();

        foreach (var record in result.Result)
        {
            Console.WriteLine(record);
        }

        Console.WriteLine("disposed successfully connection closed");
    }
}

