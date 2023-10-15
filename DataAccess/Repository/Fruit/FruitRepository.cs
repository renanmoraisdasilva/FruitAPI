using AutoMapper;
using FruitAPI.DataAccess.Context;
using FruitAPI.DataAccess.Models.Fruit;
using Microsoft.EntityFrameworkCore;

namespace FruitAPI.DataAccess.Repository.Fruit;

public class FruitRepository : IFruitRepository
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<DataContext> _contextFactory;
    public FruitRepository(IMapper mapper, IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetFruitDTO>> FindAll()
    {
        using var _context = _contextFactory.CreateDbContext();
        return await _context.Fruits
            .Include(f => f.Type)
            .Select(fruit => _mapper.Map<GetFruitDTO>(fruit))
            .ToListAsync();
    }

    public async Task<GetFruitDTO> FindById(long id)
    {
        using var _context = _contextFactory.CreateDbContext();
        var fruit = await _context.Fruits
            .Include(f => f.Type)
            .FirstOrDefaultAsync(fruit => fruit.Id == id) ?? throw new KeyNotFoundException("Fruit not found");

        return _mapper.Map<GetFruitDTO>(fruit);
    }

    public async Task<IEnumerable<GetFruitDTO>> FindByTypeId(long typeId)
    {
        using var _context = _contextFactory.CreateDbContext();
        return await _context.Fruits
            .Where(fruit => fruit.TypeId == typeId)
            .Include(f => f.Type)
            .Select(fruit => _mapper.Map<GetFruitDTO>(fruit))
            .ToListAsync();
    }

    public async Task<GetFruitDTO> Save(AddFruitDTO fruitDTO)
    {
        using var _context = _contextFactory.CreateDbContext();

        var fruitType = await _context.FruitTypes.FindAsync(fruitDTO.TypeId) ?? throw new KeyNotFoundException("FruitType not found");

        var fruit = Entities.Fruit.Create(fruitDTO.Name, fruitDTO.Description, fruitDTO.TypeId);

        _context.Fruits.Add(fruit);

        await _context.SaveChangesAsync();

        return _mapper.Map<GetFruitDTO>(fruit);

    }

    public async Task<GetFruitDTO> Update(long id, AddFruitDTO fruitDTO)
    {
        using var _context = _contextFactory.CreateDbContext();

        var type = await _context.FruitTypes.FindAsync(fruitDTO.TypeId) ?? throw new KeyNotFoundException("FruitType not found");

        var existingFruit = await _context.Fruits.Include(f => f.Type).FirstOrDefaultAsync(f => f.Id == id) ?? throw new KeyNotFoundException("Fruit not found");

        existingFruit.Name = fruitDTO.Name;
        existingFruit.Description = fruitDTO.Description;
        existingFruit.TypeId = fruitDTO.TypeId;
        existingFruit.Type = type;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetFruitDTO>(existingFruit);
    }

    public async Task Delete(long id)
    {
        using var _context = _contextFactory.CreateDbContext();

        var existingFruit = await _context.Fruits.FindAsync(id) ?? throw new KeyNotFoundException("Fruit not found");

        _context.Fruits.Remove(existingFruit);

        await _context.SaveChangesAsync();
    }
}
