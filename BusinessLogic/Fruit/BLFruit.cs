using AutoMapper;
using FruitAPI.DataAccess.Models.Fruit;
using FruitAPI.DataAccess.Repository.Fruit;
using Microsoft.EntityFrameworkCore;

namespace FruitAPI.BusinessLogic.Fruit;

public class BLFruit : IBLFruit
{
    private readonly IFruitRepository _fruitRepository;
    public BLFruit(IMapper mapper, IDbContextFactory<DataAccess.Context.DataContext> context)
    {
        _fruitRepository = new FruitRepository(mapper, context);
    }

    public async Task<IEnumerable<GetFruitDTO>> FindAll()
    {
        return await _fruitRepository.FindAll();
    }

    public async Task<GetFruitDTO> FindById(long id)
    {
        return await _fruitRepository.FindById(id);
    }

    public async Task<IEnumerable<GetFruitDTO>> FindByTypeId(long typeId)
    {
        return await _fruitRepository.FindByTypeId(typeId);
    }

    public async Task<GetFruitDTO> Save(AddFruitDTO fruitDTO)
    {
        return await _fruitRepository.Save(fruitDTO);
    }

    public async Task<GetFruitDTO> Update(long id, AddFruitDTO fruitDTO)
    {
        return await _fruitRepository.Update(id, fruitDTO);
    }

    public async Task Delete(long id)
    {
        await _fruitRepository.Delete(id);
    }
}
