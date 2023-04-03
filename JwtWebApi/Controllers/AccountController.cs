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
        public async Task<IActionResult> CreateUser(string username, string password, string role)
        {

            var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            Console.WriteLine(existingUser);
            if (existingUser != null)
              return BadRequest("Username already exists");
            else
            {
            await _userRepository.CreateUser(username, password,role);
            return Ok("UserCreatedSuccessfully");
            }

            
        }





        public record class ReqData(string username, string password);

        [HttpPost("/login")]
        public async Task<ActionResult<string>> Login([FromBody] ReqData reqData)
        {
            /////no use first
            ///var user=
            var user = await _userRepository.GetUser(reqData.username);
            //User user = await _context.Users.SingleAsync((user) => user.Username == reqData.username);
            if (user == null|| user.Username != reqData.username)
                return BadRequest("User Not Found");

            if (user.Password != reqData.password)
                return BadRequest("Wrong Password");


            ////loginHandler

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

      






    }
}
