using AutoMapper;
using FruitAPI.DataAccess.Models.FruitType;
using FruitAPI.DataAccess.Repository.FruitType;
using Microsoft.EntityFrameworkCore;

namespace FruitAPI.BusinessLogic.FruitType;

public class BLFruitType : IBLFruitType
{
    private readonly IFruitTypeRepository _fruitTypeRepository;
    public BLFruitType(IMapper mapper, IDbContextFactory<DataAccess.Context.DataContext> context)
    {
        _fruitTypeRepository = new FruitTypeRepository(mapper, context);
    }

    public async Task<IEnumerable<GetFruitTypeDTO>> FindAll()
    {
        return await _fruitTypeRepository.FindAll();
    }
    public async Task<GetFruitTypeDTO> FindById(long id)
    {
        return await _fruitTypeRepository.FindById(id);
    }
    public async Task<GetFruitTypeDTO> Save(AddFruitTypeDTO fruitTypeDTO)
    {
        return await _fruitTypeRepository.Save(fruitTypeDTO);
    }
    public async Task<GetFruitTypeDTO> Update(long id, AddFruitTypeDTO fruitTypeDTO)
    {
        return await _fruitTypeRepository.Update(id, fruitTypeDTO);
    }
    public async Task Delete(long id)
    {
        await _fruitTypeRepository.Delete(id);
    }
}
