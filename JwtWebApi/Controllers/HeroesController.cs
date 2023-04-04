using JwtWebApi.data;
using JwtWebApi.Repositories;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace JwtWebApi.Controllers
{
    [ApiController]
    public class HeroesController : ControllerBase
    {
        public static User user = new User();
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHeroesRepository _heroesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;




        public HeroesController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DataContext context, IHeroesRepository heroesRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
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
            return BadRequest("");

        }



        [HttpGet("users/{user}/heroes")]
        public async Task<IActionResult> getAllUserHeroes()
        {
            var userId =  _heroesRepository.getUserNameByToken();
            if (userId != null){
              var res = await _heroesRepository.GetAllUserHeroes(userId);

                if (res != null&& userId!=null)
                return Ok(res);
            }
          
            return BadRequest("");

        }

       


        [HttpPatch("users/{user}/heroes/{heroName}")]
        ///istrainHeroPossible
        public async Task<IActionResult> TrainHero(string heroName)
        {
            var userId =  _heroesRepository.getUserNameByToken();
            Console.WriteLine(userId);
            var isValidTrain = await _heroesRepository.TrainHeroAsync(heroName, userId);
            if (isValidTrain)
                return Ok("hero trained");


            return BadRequest("hero not trained");
        }


        [HttpPost("users/{user}/heroes/{nameOfHero}")]
        public async Task<IActionResult> isPossibleAddNewHero(string nameOfHero)
        {
            var userId =  _heroesRepository.getUserNameByToken();
            var isHeroAdded = await _heroesRepository.AddHeroAsync(nameOfHero, userId);
            if (isHeroAdded)
                return Ok("hero added");
            else
                return BadRequest("hero already exicted");
        }

    }
}
