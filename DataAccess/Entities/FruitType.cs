namespace FruitAPI.DataAccess.Entities;

public class FruitType
{
    public FruitType()
    {
    }
    private FruitType(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();

    public static FruitType Create(string name, string description)
    {
        return new FruitType(name, description);
    }
}
