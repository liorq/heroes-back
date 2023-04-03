using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using JwtWebApi.data;

using NHibernate.Util;
using System.Linq;
using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                Role = role,
                Heroes = new List<Hero>(),
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(string username)
        {
            return  await _context.Users.SingleOrDefaultAsync(user => user.Username == username);
        }
    }
}
