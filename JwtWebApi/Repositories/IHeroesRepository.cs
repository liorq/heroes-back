using JwtWebApi.tables;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Repositories
{
    public interface IHeroesRepository
    {
      
        Task<List<Hero>> GetAllHeroesAsync();
        Task<List<Hero>> TrainHeroByIdAsync(string name, string userName);
        Task<List<Hero>> GetHeroByIdAsync(int id, string userName);
        Task<bool> TrainHeroAsync(string name, string userName);
        Task<List<Hero>> SetAllHeroesAsync();
        Task<bool> AddHeroAsync(string nameOfHero, string userName);
    }
}
