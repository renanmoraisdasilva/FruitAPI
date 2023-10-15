using AutoMapper;
using FruitAPI.DataAccess.Entities;
using FruitAPI.DataAccess.Models.Fruit;
using FruitAPI.DataAccess.Models.FruitType;

namespace FruitAPI.DataAccess;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Fruit, GetFruitDTO>();
        CreateMap<Fruit, AddFruitDTO>();
        CreateMap<AddFruitDTO, Fruit>();
        CreateMap<AddFruitDTO, GetFruitDTO>();


        CreateMap<FruitType, GetFruitTypeDTO>();
        CreateMap<FruitType, AddFruitTypeDTO>();
        CreateMap<AddFruitTypeDTO, FruitType>();
        CreateMap<GetFruitTypeDTO, GetFruitTypeDTO>();
    }
}
