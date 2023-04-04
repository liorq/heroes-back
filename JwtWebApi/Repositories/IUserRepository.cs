using JwtWebApi.tables;

namespace JwtWebApi.Repositories
{
    public interface IUserRepository
    {
        public  Task<User> GetUser(string username);
        public Task<User> CreateUser(string username, string password, string role);
         public string CreateToken(User user);
        public string? getUserNameByToken();



    }
}