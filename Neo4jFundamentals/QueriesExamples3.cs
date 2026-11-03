using Neo4j.Driver;

public class QueriesExamples3
{
    public static async Task RunNeo(IDriver driver)
    {

        Console.WriteLine("-----Temporal-types--combination--of---Dates-and-Times------");

        Console.WriteLine(); Console.WriteLine();

        Console.WriteLine("---Writing--Temporal-types------");

        var cypher1 = @"CREATE (e:Event {
                        startsAt: $datetime,
                        createdAt: datetime($dtstring),
                        updatedAt: datetime()                   
                      })";

        await driver.ExecutableQuery(cypher1)
                     .WithParameters(new
                     {
                         datetime = new ZonedDateTime(
                            new DateTime(2024, 5, 15, 14, 30, 0),
                            "Europe/London"
                        ),
                         dtstring = "2024-05-15T14:30:00+02:00"
                     })
                     .ExecuteAsync();

        Console.WriteLine("---Reading--Temporal-types------");

        var cypher2 = @"RETURN date() AS date,
                        time() AS time,
                        datetime() AS datetime,
                        toString(datetime()) AS asString
                        ";

        var result = await driver.ExecutableQuery(cypher2)
                                  .ExecuteAsync();

        foreach (var record in result.Result)
        {
            var date = record["date"].As<LocalDate>();
            var time = record["time"].As<OffsetTime>();
            var datetime = record["datetime"].As<ZonedDateTime>();
            var asString = record["asString"].As<string>();
            Console.WriteLine("date = " + date);
            Console.WriteLine("time = " + time);
            Console.WriteLine("dateTime = " + datetime);
            Console.WriteLine("asString = " + asString);
        }



        Console.WriteLine("---Working With Durations------");

        var eventLength = new Duration(0, 0, 5400, 0); //1 hour 30 mins

        var cypher3 = @"CREATE (e:Event {
                      duration: $eventLength,
                      interval: duration('PT1H30M')
                      })";

        var result2 = await driver.ExecutableQuery(cypher3)
                                  .WithParameters(new
                                  {
                                      eventLength = eventLength
                                  })
                                  .ExecuteAsync();



            Console.WriteLine(result2.Result.FirstOrDefault());
      
    }
}
