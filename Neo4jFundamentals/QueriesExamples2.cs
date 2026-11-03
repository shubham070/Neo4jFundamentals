using Neo4j.Driver;

public class QueriesExamples2
{
    public static async Task RunNeo(IDriver driver)
    {

        Console.WriteLine("----------- Nodes --------");

        var cypher1 = @"
                      MATCH path = (person:Person) - [actedIn:ACTED_IN] -> (movie:Movie {title: $title})
                      RETURN path, person, actedIn, movie
                      ";

        var result1 = driver.ExecutableQuery(cypher1)
                .WithParameters(new { title = "Toy Story" })
                .ExecuteAsync();


        if (result1.Result != null)
        {
          
            foreach (var record in result1.Result.Result)
            {
                var node = record["movie"].As<INode>();

                Console.WriteLine(node.ElementId);
                Console.WriteLine(node.Labels[0]);
                Console.WriteLine(node.Properties.Count);
                Console.WriteLine(node.Properties["title"].As<string>());


                Console.WriteLine("----------- Relationships --------");

                var actedIn = record["actedIn"].As<IRelationship>();

                Console.WriteLine(actedIn.ElementId);
                Console.WriteLine(actedIn.Type);
                Console.WriteLine(actedIn.Properties.Count);

                var role = actedIn.Properties["role"].As<string>();

                Console.WriteLine(actedIn.StartNodeElementId);
                Console.WriteLine(actedIn.EndNodeElementId);

                Console.WriteLine("----------- Paths --------");

                var path = record["path"].As<IPath>();

                Console.WriteLine(path.Start);
                Console.WriteLine(path.End);
                Console.WriteLine(path.Nodes.Count);
                Console.WriteLine(path.Relationships.Count);

            }
        }

    }
}