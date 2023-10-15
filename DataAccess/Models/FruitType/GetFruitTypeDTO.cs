using System.ComponentModel.DataAnnotations;

namespace FruitAPI.DataAccess.Models.FruitType;

public class GetFruitTypeDTO
{
    [Required(ErrorMessage = "TypeId is required")]
    public long Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MinLength(25, ErrorMessage = "Description must be at least 25 characters")]
    public string Description { get; set; }
}
