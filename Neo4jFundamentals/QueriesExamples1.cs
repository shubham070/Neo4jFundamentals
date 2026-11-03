using System.Text.Json;
using Neo4j.Driver;
using Neo4j.Driver.Mapping;

public class QueriesExamples1
{


    

       public static async Task RunNeo(IDriver driver)
    {
        Console.WriteLine(" ['keys']  and  .AS<> Usage");

        await driver.VerifyConnectivityAsync();
        Console.WriteLine("connection successful");


        var cypher = @"
                    MATCH (p:Person {name: $name})-[r:ACTED_IN]->(m:Movie)
                    RETURN m.title AS title, r.role AS role
                    ";

        // execute a query
        var result1 = await driver.ExecutableQuery(cypher)
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

        foreach (var record in result1.Result)
        {
            Console.WriteLine(record);
        }

        Console.WriteLine("<-------------------------------------------------------------------------------->");

        var cypher2 = @"
                      MATCH (p:Person)
                      WHERE p.name CONTAINS $name
                      RETURN p.name AS name, p.age AS age
                      ";

        var result2 = await driver.ExecutableQuery(cypher2)
        .WithParameters(new { name = "Ron" })
        .ExecuteAsync();

        var person = result2.Result[0].AsObject<Person>();

        Console.WriteLine("Name = " + person.Name  + " Age = " +person.Age);

        Console.WriteLine("<-------------------------------------------------------------------------------->");

    }
}