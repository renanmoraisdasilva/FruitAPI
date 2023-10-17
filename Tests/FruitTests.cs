using AutoMapper;
using FruitAPI.DataAccess.Context;
using FruitAPI.DataAccess.Models.Fruit;
using FruitAPI.DataAccess.Models.FruitType;
using FruitAPI.DataAccess.Repository.Fruit;
using FruitAPI.DataAccess.Repository.FruitType;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace FruitAPI.Tests;

[TestClass]
public class FruitTests
{
    private readonly IDbContextFactory<DataContext> _contextFactory;
    private readonly IMapper _mapper;
    const long nonExistingId = 99;
    const string validDescription = "Valid Description - Valid Description";
    const string invalidDescription = "Short Description";

    public FruitTests()
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<DataContext>(options => options.UseInMemoryDatabase("TestDatabase"));

        services.AddAutoMapper(typeof(FruitAPI.DataAccess.AutoMapper));

        var serviceProvider = services.BuildServiceProvider();

        _contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    [TestMethod]
    public async Task GetFruit_Should_ReturnFruitFromDatabase()
    {
        // Arrange
        using var context = _contextFactory.CreateDbContext();
        var fruitType = CreateFruitType("Banana Type", validDescription);
        var fruit = CreateFruit("Banana", validDescription, fruitType);
        var savedFruit = await SaveFruit(fruit);

        // Act
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);
        var retrievedFruit = await fruitRepository.FindById(savedFruit.Id);

        // Assert
        Assert.IsNotNull(retrievedFruit);
        Assert.AreEqual(savedFruit.Id, retrievedFruit.Id);
        Assert.AreEqual(fruit.Name, retrievedFruit.Name);
        Assert.AreEqual(fruit.Description, retrievedFruit.Description);
    }

    [TestMethod]
    public async Task AddFruit_Should_AddFruitToDatabase()
    {
        // Arrange
        var fruitType = CreateFruitType("Apple Type", validDescription);
        var fruit = CreateFruit("Apple", validDescription, fruitType);

        // Act
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);
        var savedFruit = await fruitRepository.Save(fruit);

        // Assert

        using var assertContext = _contextFactory.CreateDbContext();
        var addedFruit = await assertContext.Fruits.Include(fruit => fruit.Type)
                   .FirstOrDefaultAsync(s => s.Id == savedFruit.Id);

        Assert.IsNotNull(addedFruit);
        Assert.IsNotNull(addedFruit.Type);

        Assert.AreEqual(fruit.Name, addedFruit.Name);
        Assert.AreEqual(fruit.Description, addedFruit.Description);
        Assert.AreEqual(fruit.TypeId, addedFruit.TypeId);
    }

    [TestMethod]
    public async Task UpdateFruit_Should_UpdateFruitInDatabase()
    {
        // Arrange
        using var context = _contextFactory.CreateDbContext();
        var fruitType = CreateFruitType("Cherry Type", validDescription);
        var fruit = CreateFruit("Cherry", validDescription, fruitType);
        var savedFruit = await SaveFruit(fruit);

        // Act
        var updatedFruit = new AddFruitDTO
        {
            Name = "Updated Cherry",
            Description = "Updated Description",
            TypeId = savedFruit.Type.Id,
        };

        var fruitRepository = new FruitRepository(_mapper, _contextFactory);
        await fruitRepository.Update(savedFruit.Id, updatedFruit);

        // Assert
        using var assertContext = _contextFactory.CreateDbContext();
        var retrievedFruit = await assertContext.Fruits.FindAsync(savedFruit.Id);

        Assert.IsNotNull(retrievedFruit);
        Assert.AreEqual(updatedFruit.Name, retrievedFruit.Name);
        Assert.AreEqual(updatedFruit.Description, retrievedFruit.Description);
    }

    [TestMethod]
    public async Task DeleteFruit_Should_RemoveFruitFromDatabase()
    {
        // Arrange
        using var context = _contextFactory.CreateDbContext();
        var fruitType = CreateFruitType("Grapes Type", validDescription);
        var fruit = CreateFruit("Grapes", validDescription, fruitType);
        var savedFruit = await SaveFruit(fruit);

        // Act
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);
        await fruitRepository.Delete(savedFruit.Id);

        // Assert
        using var assertContext = _contextFactory.CreateDbContext();
        var deletedFruit = await assertContext.Fruits.FindAsync(savedFruit.Id);

        Assert.IsNull(deletedFruit);
    }

    [TestMethod]
    public async Task AddInvalidFruit_Should_ThrowValidationException()
    {
        // Arrange
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);

        // Act and Assert
        Assert.ThrowsException<ValidationException>(() =>
        {
            var fruitType = CreateFruitType("Apple Type", invalidDescription);
            var fruit = CreateFruit("Apple", invalidDescription, fruitType);
        });
    }

    [TestMethod]
    public async Task GetFruit_Should_ThrowKeyNotFoundException()
    {
        // Arrange
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => fruitRepository.FindById(nonExistingId));
    }

    [TestMethod]
    public async Task UpdateNonExistentFruit_Should_ThrowKeyNotFoundException()
    {
        // Arrange
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);
        var fruitType = CreateFruitType("Cherry Type", validDescription);
        var fruit = CreateFruit("Cherry", validDescription, fruitType);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => fruitRepository.Update(nonExistingId, fruit));
    }

    [TestMethod]
    public async Task DeleteNonExistentFruit_Should_ThrowKeyNotFoundException()
    {
        // Arrange
        var fruitRepository = new FruitRepository(_mapper, _contextFactory);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => fruitRepository.Delete(nonExistingId));
    }

    private AddFruitTypeDTO CreateFruitType(string name, string description)
    {
        var fruitType = new AddFruitTypeDTO
        {
            Name = name,
            Description = description
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(fruitType);

        if (!Validator.TryValidateObject(fruitType, context, validationResults, true))
        {
            throw new ValidationException(validationResults[0].ErrorMessage);
        }

        return fruitType;
    }

    private AddFruitDTO CreateFruit(string name, string description, AddFruitTypeDTO fruitType)
    {
        var fruit = new AddFruitDTO
        {
            Name = name,
            Description = description,
            TypeId = SaveFruitType(fruitType).Id
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(fruitType);

        if (!Validator.TryValidateObject(fruitType, context, validationResults, true))
        {
            throw new ValidationException(validationResults[0].ErrorMessage);
        }

        return fruit;
    }

    private async Task<GetFruitTypeDTO> SaveFruitType(AddFruitTypeDTO fruitType)
    {
        var repository = new FruitTypeRepository(_mapper, _contextFactory);
        return await repository.Save(fruitType);
    }

    private async Task<GetFruitDTO> SaveFruit(AddFruitDTO fruit)
    {
        var repository = new FruitRepository(_mapper, _contextFactory);
        return await repository.Save(fruit);
    }
}
