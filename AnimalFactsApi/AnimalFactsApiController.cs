using AnimalFactsApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace AnimalFactsApi
{
    [Route("api/v1.0/facts")]
    [ApiController]
    public class AnimalFactsApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var newAnimalFact = new AnimalFact("name", "source", "text", "media link", "wiki link");
            return Ok(newAnimalFact);
        }

        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return "value";
        // }
    }
}