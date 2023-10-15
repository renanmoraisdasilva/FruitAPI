using FruitAPI.DataAccess.Models.Fruit;

namespace FruitAPI.BusinessLogic.Fruit;

public interface IBLFruit
{
    Task<IEnumerable<GetFruitDTO>> FindAll();
    Task<GetFruitDTO> FindById(long id);
    Task<IEnumerable<GetFruitDTO>> FindByTypeId(long typeId);
    Task<GetFruitDTO> Save(AddFruitDTO fruitDTO);
    Task<GetFruitDTO> Update(long id, AddFruitDTO fruitDTO);
    Task Delete(long id);
}
