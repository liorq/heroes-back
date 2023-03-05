using JwtWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroesRepository _heroesRepository;

        public HeroesController(IHeroesRepository heroesRepository)
        {
            _heroesRepository = heroesRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> getAllHeroes()
        {
            var res = await _heroesRepository.GetAllHeroesAsync();

            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest("zeev is mniake");

        }

        [HttpGet("{heroId}")]
        public async Task<IActionResult> GetHeroById(int heroId)
        {
            var res = await _heroesRepository.GetHeroByIdAsync(heroId);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();

        }
        [HttpPatch("{heroId}")]

        public async Task<IActionResult> TrainHero(string heroName)
        {
            var isValidTrain = await _heroesRepository.TrainHeroAsync(heroName);
            if (isValidTrain)
                return Ok();


            return NotFound();
        }



    }
}
