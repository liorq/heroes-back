using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using NHibernate.Util;
using System.Linq;
using System.Xml.Linq;

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
        new Hero { Name = "Superman", Ability = "Super strength, flight, invulnerability", Id = null, FirstDayHeroTrained = 1978, StartingPower = 100, CurrentPower = 95, SuitColors = "Red, blue, yellow", LastTimeHeroTrained = "Yesterday" ,TrainerName="manger"},
        new Hero { Name = "Batman", Ability = "Peak physical and mental conditioning, martial arts, detective skills", Id = null, FirstDayHeroTrained = 1939, StartingPower = 80, CurrentPower = 70, SuitColors = "Black, gray", LastTimeHeroTrained = "Last week",TrainerName="manger" },
        new Hero { Name = "Wonder Woman", Ability = "Super strength, flight, invulnerability, Lasso of Truth", Id = null, FirstDayHeroTrained = 1941, StartingPower = 90, CurrentPower = 85, SuitColors = "Red, blue, gold", LastTimeHeroTrained = "Yesterday",TrainerName="manger" },
        new Hero { Name = "Spider-Man", Ability = "Super strength, agility, spider-sense, web-slinging", Id = null, FirstDayHeroTrained = 1962, StartingPower = 70, CurrentPower = 60, SuitColors = "Red, blue", LastTimeHeroTrained = "Last week" ,TrainerName="manger"},
        new Hero { Name = "Iron Man", Ability = "Powered suit with weapons and flight capabilities", Id = null, FirstDayHeroTrained = 1963, StartingPower = 85, CurrentPower = 80, SuitColors = "Red, gold", LastTimeHeroTrained = "Yesterday" ,TrainerName="manger"}
    };

            foreach (var hero in heroes)
            {
                _context.AllHeroes.Add(hero);
                Console.WriteLine(hero.Name);
            }
            await _context.SaveChangesAsync(); // persist changes to the database

            return heroes;
        }

        public async Task<List<Hero>> GetAllHeroesAsync()
        {
            ///intiti framework
            ///כל מה שמתשנה בקונטקסט משתנה גם בפיירמוורק
            var heroes = await _context.AllHeroes.ToListAsync();
            if (heroes.Count == 0)
                await SetAllHeroesAsync();

            foreach (var hero in heroes)
            {
                Console.WriteLine(hero.Id);
            }
            return heroes;
        }

       
        public async Task<List<Hero>> GetAllUserHeroes(string userName)
        {
            var heroes = _context.AllHeroes.Where(u => u.TrainerName == userName);
            return heroes.OrderByDescending(h => h.CurrentPower).ToList();
        }


        /// ברגע שאני עובר אוטנטיקציה אז הוא ידע מהטוקן את האיידי שלך 
        /// ואז לפי האיידי 


        public async Task<bool> TrainHeroAsync(string name, string userName)
        {
            var heroes =  _context.AllHeroes.Where(u => u.TrainerName == userName);
            var hero = heroes.FirstOrDefault(u => u.Name == name);

            if (heroes == null|| hero==null)
                return false;

            string formattedDate = DateTime.Today.ToString("yyyy-MM-dd");


            Random random = new Random();
            if (hero.AmountOfTimeHeroTrained == 5 && hero.LastTimeHeroTrained == formattedDate)
                return false;
            if (hero.LastTimeHeroTrained != formattedDate)
            {
                hero.AmountOfTimeHeroTrained = 0;
                hero.LastTimeHeroTrained = formattedDate;

            }
            
                if (hero.AmountOfTimeHeroTrained == null)
                    hero.AmountOfTimeHeroTrained = 0;

                double v = (1 + random.NextDouble() * 0.1);
                hero.CurrentPower = hero.CurrentPower * v;
                Console.WriteLine(hero.CurrentPower);
                hero.LastTimeHeroTrained = formattedDate;
                hero.AmountOfTimeHeroTrained++;
            

            ///להוסיף שדה של היום ראשון שהתאמנתי ושדה של הפעם האחרונה שהתאמנתי וכמות הפעמים שהתאמנתי באותו יום 
            
            await _context.SaveChangesAsync();
            ///להוסיף שקר אם זה לא הצליח
            return true;

        }

        public async Task<bool> AddHeroAsync(string nameOfHero, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            var heroes = await _context.AllHeroes.FirstOrDefaultAsync(b => b.TrainerName == userName);

            var allHeroes = _context.AllHeroes.Where(h=>h.TrainerName=="manger");
            var newHero = await allHeroes.FirstOrDefaultAsync(b => b.Name == nameOfHero);

            if (newHero == null || user == null)
                return false;

            if (heroes != null && heroes.Name == nameOfHero)
            {
                return false;
            }

            if (user?.Heroes == null)
            {
                user.Heroes = new List<Hero>();
            }

            ///make new hero
            Hero newObj = new Hero()
            {
                Name = newHero.Name,
                Ability= newHero.Ability,
                TrainerName= userName,
                FirstDayHeroTrained=0,
                StartingPower=newHero.StartingPower,
                CurrentPower=newHero.CurrentPower,
                SuitColors=newHero.SuitColors,
                LastTimeHeroTrained=newHero.LastTimeHeroTrained,
            };
            _context.AllHeroes.Add(newObj);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}