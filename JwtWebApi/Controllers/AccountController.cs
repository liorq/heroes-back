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

    public class AccountController : ControllerBase
    {
        public static User user = new User();
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHeroesRepository _heroesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;




        public AccountController(IHttpContextAccessor httpContextAccessor,IConfiguration configuration, DataContext context, IHeroesRepository heroesRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
            _heroesRepository = heroesRepository;

        }

        [HttpPost("/signUp")]
        public async Task<User> CreateUser(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                Role = role,
                Heroes = new List<Hero>(),

                ////להוסיף מזהה 
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

      




            [HttpGet("")]
            public async Task<IActionResult> getAllHeroes(){

            var res = await _heroesRepository.GetAllHeroesAsync();

            if (res?.Count > 0)
                {
                    return Ok(res);
                }
                return BadRequest("zeev is mniake");

            }



        [HttpGet("users/user/heroes")]
         public async Task<IActionResult> getAllUserHeroes()
           {
            var userId = getUserNameByToken();
            var res = await _heroesRepository.GetAllUserHeroes(userId);

            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest("zeev is mniake");

          }

        /// //from headers bearer and token without auto postmen only key= Authorization
        /// /// value exaple= Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImEiLCJleHAiOjE2NzgxOTY0MjN9.gNDgXckCC1u510ZVmR53YAKgRn5LeISD1BjxJDMoZ2dSpz3a2LKaXnbNJPWtK2nnyGYMAZKak1CspzMorjtYTg

     

        [HttpPatch("users/user/heroes/{heroName}")]
            ///istrainHeroPossible
            public async Task<IActionResult> TrainHero(string heroName)
            {
            var userId = getUserNameByToken();
            var isValidTrain = await _heroesRepository.TrainHeroAsync(heroName, userId);
                    if (isValidTrain)
                    return Ok("hero trained");


                return BadRequest("hero not trained");
            }

        [HttpPost("users/user/heroes/{nameOfHero}")]
        public async Task<IActionResult> isPossibleAddNewHero(string nameOfHero)
        {
           var userId = getUserNameByToken();
           var isHeroAdded = await _heroesRepository.AddHeroAsync(nameOfHero, userId);
            if (isHeroAdded)
                return Ok("hero added");
            else
                return BadRequest("hero already exicted");
        }

        [HttpGet("/me")]
        public string? getUserNameByToken(){
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (token == "")
                return "you dont have token send";
            
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            return decodedToken?.Claims?.ToArray()[0]?.Value;
        }

    }
}
