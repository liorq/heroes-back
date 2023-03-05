using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            List<Hero> heroes = new List<Hero> {
        new Hero { Name = "Superman", Ability = "Super strength, flight, invulnerability", Id = null, FirstDayHeroTrained = 1978, StartingPower = 100, CurrentPower = 95, SuitColors = "Red, blue, yellow", LastTimeHeroTrained = "Yesterday" },
        new Hero { Name = "Batman", Ability = "Peak physical and mental conditioning, martial arts, detective skills", Id = null, FirstDayHeroTrained = 1939, StartingPower = 80, CurrentPower = 70, SuitColors = "Black, gray", LastTimeHeroTrained = "Last week" },
        new Hero { Name = "Wonder Woman", Ability = "Super strength, flight, invulnerability, Lasso of Truth", Id = null, FirstDayHeroTrained = 1941, StartingPower = 90, CurrentPower = 85, SuitColors = "Red, blue, gold", LastTimeHeroTrained = "Yesterday" },
        new Hero { Name = "Spider-Man", Ability = "Super strength, agility, spider-sense, web-slinging", Id = null, FirstDayHeroTrained = 1962, StartingPower = 70, CurrentPower = 60, SuitColors = "Red, blue", LastTimeHeroTrained = "Last week" },
        new Hero { Name = "Iron Man", Ability = "Powered suit with weapons and flight capabilities", Id = null, FirstDayHeroTrained = 1963, StartingPower = 85, CurrentPower = 80, SuitColors = "Red, gold", LastTimeHeroTrained = "Yesterday" }
    };

                foreach (var hero in heroes)
                {
                    _context.Heroes.Add(hero);
                Console.WriteLine(hero.Name);
                }
                await _context.SaveChangesAsync(); // persist changes to the database
       
            return heroes;
        }

        public async Task<List<Hero>> GetAllHeroesAsync()
        {
            ///intiti framework
            ///כל מה שמתשנה בקונטקסט משתנה גם בפיירמוורק
            var heroes = await _context.Heroes.ToListAsync();
            if(heroes.Count == 0)
            await SetAllHeroesAsync();

            foreach (var hero in heroes)
            {
                Console.WriteLine(hero.Id);
            }
            return heroes;
        }

        public async Task<List<Hero>> GetHeroByIdAsync(int id, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var hero =  user?.Heroes?.ToList();
            if (hero == null)
            {
                throw new ArgumentException("Hero not found.");
            }
            return hero;
        }

        /// <summary>
        /// route my heroes 
        /// ברגע שאני עובר אוטנטיקציה אז הוא ידע מהטוקן את האיידי שלך 
        /// ואז לפי האיידי 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<Hero>> TrainHeroByIdAsync(string name, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);

            var hero = user?.Heroes?.Where(b => b.Name == name);
            Random random = new Random();
            foreach (var Hero in hero)
            {
                double v = (1 + random.NextDouble() * 0.1);
                Hero.CurrentPower = Hero.CurrentPower * v;
                Console.WriteLine(Hero.CurrentPower);

            }
            return (List<Hero>)hero;
        }
        public async Task<bool> TrainHeroAsync(string name, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);

            var heroes = user?.Heroes?.Where(b => b.Name == name);

            Random random = new Random();
            foreach (var hero in heroes)
            {
                double v = (1 + random.NextDouble() * 0.1);
                hero.CurrentPower = hero.CurrentPower * v;
                Console.WriteLine(hero.CurrentPower);
            }
            await _context.SaveChangesAsync();
                ///להוסיף שקר אם זה לא הצליח
            return true;


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
