using FruitAPI.BusinessLogic.Fruit;
using FruitAPI.DataAccess.Models.Fruit;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FruitAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FruitsController : ControllerBase
{
    private readonly IBLFruit _blFruit;
    public FruitsController(IBLFruit blFruit)
    {
        _blFruit = blFruit;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFruitDTO>>> FindAll()
    {
        try
        {
            return Ok(await _blFruit.FindAll());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetFruitDTO>> FindById(long id)
    {
        try
        {
            return Ok(await _blFruit.FindById(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
    }

    [HttpGet("type/{typeId}")]
    public async Task<ActionResult<IEnumerable<GetFruitDTO>>> FindByTypeId(long typeId)
    {
        try
        {
            return Ok(await _blFruit.FindByTypeId(typeId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetFruitDTO>> SaveFruit([FromBody] AddFruitDTO fruitDTO)
    {
        try
        {
            return Ok(await _blFruit.Save(fruitDTO));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { status = 400, msg = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetFruitDTO>> UpdateFruit(long id, [FromBody] AddFruitDTO fruitDTO)
    {
        try
        {
            return Ok(await _blFruit.Update(id, fruitDTO));
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
            await _blFruit.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { status = 404, msg = ex.Message, date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") });
        }
    }
}