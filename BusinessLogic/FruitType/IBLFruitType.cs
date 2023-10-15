using FruitAPI.DataAccess.Models.FruitType;

namespace FruitAPI.BusinessLogic.FruitType;

public interface IBLFruitType
{
    Task<IEnumerable<GetFruitTypeDTO>> FindAll();
    Task<GetFruitTypeDTO> FindById(long id);
    Task<GetFruitTypeDTO> Save(AddFruitTypeDTO fruitTypeDTO);
    Task<GetFruitTypeDTO> Update(long id, AddFruitTypeDTO fruitTypeDTO);
    Task Delete(long id);
}
