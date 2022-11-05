
using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace JwtWebApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("getAll"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<List<User>>> Get()
        {
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)flexible;
            dictionary.Add("users", await _context.Users.ToArrayAsync());
            return Ok(dictionary);
        }

        [HttpGet("{id}"), Authorize(Roles = "ROLE_OWNER")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var hero = await _context.Users.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero not found");
            return Ok(hero);
        }

        [HttpPut("lock/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> changeStatusToInProcess(int id)
        {
            var mission = await _context.Users.FindAsync(id);
            if (mission == null)
                return BadRequest("Hero not found");
            mission.IsLock = true;

            await _context.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpPut("unLock/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> changeStatusToInProces(int id)
        {
            var mission = await _context.Users.FindAsync(id);
            if (mission == null)
                return BadRequest("Hero not found");
            mission.IsLock = false;

            await _context.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpPost("create"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> AddHero(User hero)
        {
            User newusr = new User();
            newusr.Name = hero.Name;
            newusr.Username = hero.Username;
            newusr.Password = hero.Password;
            newusr.Role = hero.Role;
            _context.Users.Add(newusr);
            await _context.SaveChangesAsync();

            User user = await _context.Users.SingleAsync((u) => u.Name == hero.Name);
            return Ok(user);
        }
        [HttpPut("delete/{id}"),  Authorize(Roles = "ROLE_OWNER")]
        public async Task<ActionResult<User>> deleteHero(int id)
        {
            var hero = await _context.Users.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero not found");
            _context.Users.Remove(hero);
            await _context.SaveChangesAsync();

            return Ok(hero);
        }
    }
}
