using FruitAPI.DataAccess.Models.Fruit;

namespace FruitAPI.DataAccess.Repository.Fruit;

public interface IFruitRepository
{
    Task<IEnumerable<GetFruitDTO>> FindAll();
    Task<GetFruitDTO> FindById(long id);
    Task<IEnumerable<GetFruitDTO>> FindByTypeId(long typeId);
    Task<GetFruitDTO> Save(AddFruitDTO fruitOTO);
    Task<GetFruitDTO> Update(long id, AddFruitDTO fruitDTO);
    Task Delete(long id);
}
