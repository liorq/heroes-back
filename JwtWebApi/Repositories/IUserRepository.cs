using JwtWebApi.tables;

namespace JwtWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(string username, string password, string role);
        public  Task<User> GetUser(string username);

    }
}