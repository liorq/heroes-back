using JwtWebApi.tables;

namespace JwtWebApi.Repositories
{
    public interface IHeroesRepository
    {
        Task<List<Hero>> SetAllHeroesAsync();
        Task<List<Hero>> GetAllHeroesAsync();
        Task<List<Hero>> GetHeroByIdAsync(int id);
        Task<List<Hero>> TrainHeroByIdAsync(string name);
        Task<bool> TrainHeroAsync(string name);
    }
}
