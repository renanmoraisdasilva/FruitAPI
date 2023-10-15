using FruitAPI.DataAccess.Models.FruitType;

namespace FruitAPI.DataAccess.Models.Fruit;

public class GetFruitDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public GetFruitTypeDTO Type { get; set; }
}
