using Microsoft.AspNetCore.Mvc;
using Parcial1_P4_JoseManuel.Services;

namespace Parcial1_P4_JoseManuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberController(NumbersService numbersService) : ControllerBase
{
    [HttpGet("{numero}")]
    public async Task<IActionResult> Calcular(double numero)
    {
        var resultado = numero + numero;

        await numbersService.SaveNumber(numero, resultado);

        return Ok(resultado);
    }

    [HttpGet("historial")]
    public async Task<IActionResult> Historial()
    {
        var records = await numbersService.GetNumbers();

        return Ok(records);
    }
}