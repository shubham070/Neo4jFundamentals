using Neo4j.Driver.Mapping;

public class Person
{
    [MappingSource("name")]
    public string Name { get; set; }

    [MappingOptional]
    [MappingDefaultValue(0)]
    public string Age {get;set;}
}