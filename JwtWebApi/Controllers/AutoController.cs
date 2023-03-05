using JwtWebApi.data;
using JwtWebApi.Repositories;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml;

namespace JwtWebApi.Controllers
{
    [ApiController]
    public class AutoController : ControllerBase
    {
        public static User user = new User();
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public string currentUserName = "";



        public AutoController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("/signUp")]
        public async Task<User> CreateUser(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                Role = role
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public record class ReqData(string username, string password);

        [HttpPost("/login")]
        public async Task<ActionResult<string>> Login([FromForm] ReqData reqData)
        {
            User user = await _context.Users.SingleAsync((user) => user.Username == reqData.username);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }
            if (user.Username != reqData.username)
            {
                return BadRequest("User Not Found");
            }
            if (user.Password != reqData.password)
            {
                return BadRequest("Wrong Password");
            }
            currentUserName = user.Username;
            Console.WriteLine(currentUserName);
            string token = CreateToken(user);
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, Object>)flexible;
            dictionary.Add("access_token", token);
            dictionary.Add("token_expiry", DateTime.Now.AddDays(2));
            dictionary.Add("needTofixDate", false);
          
            return Ok(dictionary);
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            if (user.Role == "ROLE_OWNER")
            {
                claims.Add(new Claim(ClaimTypes.Role, "ROLE_MANAGER"));
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

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
               

                var res = await _heroesRepository.GetHeroByIdAsync(heroId, GetCurrentUser()?.Value);
                if (res != null)
                {
                    return Ok(res);
                }
                return NotFound();

            }
            [HttpPatch("{heroId}")]

            public async Task<IActionResult> TrainHero(string heroName)
            {

                var isValidTrain = await _heroesRepository.TrainHeroAsync(heroName, GetCurrentUser()?.Value);
                    if (isValidTrain)
                    return Ok();


                return NotFound();
            }


            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpGet("/me")]
            public ActionResult<string> GetCurrentUser()
            {
                return User?.FindFirst(ClaimTypes.Name)?.Value;

              
            }



        }
    }
}
