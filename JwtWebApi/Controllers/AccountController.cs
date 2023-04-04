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
using System.Text.Json;

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
        private readonly IUserRepository _userRepository;




        public AccountController(IHttpContextAccessor httpContextAccessor,IConfiguration configuration, DataContext context, IHeroesRepository heroesRepository, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
            _heroesRepository = heroesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("/signUp")]
        public async Task<IActionResult> CreateUser([FromBody] JsonElement requestBody)
        {
            if (!requestBody.TryGetProperty("username", out JsonElement usernameElement) ||
                !requestBody.TryGetProperty("password", out JsonElement passwordElement) ||
                !requestBody.TryGetProperty("role", out JsonElement roleElement))
            {
                return BadRequest("Invalid request body");
            }

            string username = usernameElement.GetString();
            string password = passwordElement.GetString();
            string role = roleElement.GetString();

            var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            Console.WriteLine(existingUser);
            if (existingUser != null)
                return BadRequest("Username already exists");
            else
            {
                await _userRepository.CreateUser(username, password, role);
                return Ok("UserCreatedSuccessfully");
            }
        }


        public record class ReqData(string username, string password);

        [HttpPost("/login")]
        public async Task<ActionResult<string>> Login([FromBody] ReqData reqData)
        {
          
            var user = await _userRepository.GetUser(reqData.username);
            if (user == null|| user.Username != reqData.username)
                return BadRequest("User Not Found");

            if (user.Password != reqData.password)
                return BadRequest("Wrong Password");


            string token =  _userRepository.CreateToken(user);
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, Object>)flexible;
            dictionary.Add("access_token", token);
            dictionary.Add("token_expiry", DateTime.Now.AddDays(2));
            dictionary.Add("needTofixDate", false);

            return Ok(dictionary);
        }

    }
}
