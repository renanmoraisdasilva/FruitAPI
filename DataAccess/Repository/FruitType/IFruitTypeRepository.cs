using FruitAPI.DataAccess.Models.FruitType;

namespace FruitAPI.DataAccess.Repository.FruitType;

public interface IFruitTypeRepository
{
    Task<IEnumerable<GetFruitTypeDTO>> FindAll();
    Task<GetFruitTypeDTO> FindById(long id);
    Task<GetFruitTypeDTO> Save(AddFruitTypeDTO fruitTypeDTO);
    Task<GetFruitTypeDTO> Update(long id, AddFruitTypeDTO fruitTypeDTO);
    Task Delete(long id);
}
