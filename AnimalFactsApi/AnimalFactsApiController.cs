using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace AnimalFactsApi
{
    [Route("api/v1.0/facts")]
    [ApiController]
    public class AnimalFactsApiController(IFactRepository factRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string? id) => await factRepository.getFact(id).ToActionResult();
    }
}