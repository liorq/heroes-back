using JwtWebApi.data;
using JwtWebApi.tables;

namespace JwtWebApi.Repositories
{
    public class HeroesRepository : IHeroesRepository
    {
        private readonly DataContext _context;
        public HeroesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Hero>> SetAllHeroesAsync()
        {




            //_context=


            return null;

        }
        public async Task<List<Hero>> GetAllHeroesAsync()
        {

            ///intiti framework
            ///כל מה שמתשנה בקונטקסט משתנה גם בפיירמוורק
            var heroes = await _context.Heroes.ToListAsync();
            await TrainHeroByIdAsync("liot");
            foreach (var hero in heroes)
            {
                Console.WriteLine(hero.Id);
            }
            return heroes;
        }

        public async Task<List<Hero>> GetHeroByIdAsync(int id)
        {
            var hero = await _context.Heroes.Where(b => b.Id == id).ToListAsync();
            return hero;
        }


        public async Task<List<Hero>> TrainHeroByIdAsync(string name)
        {
            var hero = await _context.Heroes.Where(b => b.Name == name).ToListAsync();
            Random random = new Random();
            foreach (var Hero in hero)
            {
                double v = (1 + random.NextDouble() * 0.1);
                Hero.CurrentPower = Hero.CurrentPower * v;
                Console.WriteLine(Hero.CurrentPower);

            }
            return hero;
        }
        public async Task<bool> TrainHeroAsync(string name)
        {

            var hero = await _context.Heroes.Where(b => b.Name == name).ToListAsync();
            Random random = new Random();
            foreach (var Hero in hero)
            {
                double v = (1 + random.NextDouble() * 0.1);
                Hero.CurrentPower = Hero.CurrentPower * v;
                Console.WriteLine(Hero.CurrentPower);
                return true;
            }
            return false;
        }
        //public async Task<bool> AddHeroAsync(string nameOfHero, string userName)
        //{
        //var user = await _context.Users.Where(b => b.UserName == userName).ToListAsync();
        //var heroes = await user.Where(b => b.heroes).ToListAsync();


        //    foreach (var hero in heroes)
        //    {
        //        if (hero.Name == nameOfHero)
        //        {
        //            return false;
        //        }


        //    }
        //    return true;
    }
}
