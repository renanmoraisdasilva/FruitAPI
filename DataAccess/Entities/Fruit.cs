namespace FruitAPI.DataAccess.Entities;

public class Fruit
{
    public Fruit()
    {
    }
    private Fruit(string name, string description, long typeId)
    {
        Name = name;
        Description = description;
        TypeId = typeId;
    }
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long TypeId { get; set; }
    public FruitType Type { get; set; }
    public static Fruit Create(string name, string description, long typeId)
    {
        return new Fruit(name, description, typeId);
    }
}
