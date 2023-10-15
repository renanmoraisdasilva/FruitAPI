using AutoMapper;
using FruitAPI.DataAccess.Context;
using FruitAPI.DataAccess.Models.FruitType;
using Microsoft.EntityFrameworkCore;

namespace FruitAPI.DataAccess.Repository.FruitType;

public class FruitTypeRepository : IFruitTypeRepository
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<DataContext> _contextFactory;
    public FruitTypeRepository(IMapper mapper, IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetFruitTypeDTO>> FindAll()
    {
        using var _context = _contextFactory.CreateDbContext();
        return await _context.FruitTypes
            .Select(fruit => _mapper.Map<GetFruitTypeDTO>(fruit))
            .ToListAsync();
    }

    public async Task<GetFruitTypeDTO> FindById(long id)
    {
        using var _context = _contextFactory.CreateDbContext();
        var fruit = await _context.FruitTypes
            .FirstOrDefaultAsync(ft => ft.Id == id) ?? throw new KeyNotFoundException("FruitType not found");

        return _mapper.Map<GetFruitTypeDTO>(fruit);
    }

    public async Task<GetFruitTypeDTO> Save(AddFruitTypeDTO fruitDTO)
    {
        using var _context = _contextFactory.CreateDbContext();

        var fruitType = Entities.FruitType.Create(fruitDTO.Name, fruitDTO.Description);

        _context.FruitTypes.Add(fruitType);

        await _context.SaveChangesAsync();

        return _mapper.Map<GetFruitTypeDTO>(fruitType);

    }

    public async Task<GetFruitTypeDTO> Update(long id, AddFruitTypeDTO fruitDTO)
    {
        using var _context = _contextFactory.CreateDbContext();

        var existingFruit = await _context.FruitTypes.FindAsync(id) ?? throw new KeyNotFoundException("FruitType not found");

        _mapper.Map(fruitDTO, existingFruit);

        await _context.SaveChangesAsync();

        return _mapper.Map<GetFruitTypeDTO>(existingFruit);
    }

    public async Task Delete(long id)
    {
        using var _context = _contextFactory.CreateDbContext();

        var existingFruit = await _context.FruitTypes.FindAsync(id) ?? throw new KeyNotFoundException("FruitType not found");

        _context.FruitTypes.Remove(existingFruit);

        await _context.SaveChangesAsync();
    }
}
