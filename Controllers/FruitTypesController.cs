using FruitAPI.BusinessLogic.FruitType;
using FruitAPI.DataAccess.Models.FruitType;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FruitAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FruitTypesController : ControllerBase
{
    private readonly IBLFruitType _blFruitType;
    public FruitTypesController(IBLFruitType blFruitType)
    {
        _blFruitType = blFruitType;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFruitTypeDTO>>> FindAll()
    {
        try
        {
            return Ok(await _blFruitType.FindAll());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetFruitTypeDTO>> FindById(long id)
    {
        try
        {
            var fruit = await _blFruitType.FindById(id);
            return Ok(fruit);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetFruitTypeDTO>> SaveFruit([FromBody] AddFruitTypeDTO fruitDTO)
    {
        try
        {
            return Ok(await _blFruitType.Save(fruitDTO));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { status = 400, msg = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetFruitTypeDTO>> UpdateFruit(long id, [FromBody] AddFruitTypeDTO fruitDTO)
    {
        try
        {
            return Ok(await _blFruitType.Update(id, fruitDTO));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { status = 400, msg = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFruit(long id)
    {
        try
        {
            await _blFruitType.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
    }
}