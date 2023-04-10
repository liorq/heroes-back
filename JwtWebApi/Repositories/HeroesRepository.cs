using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NHibernate.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Xml.Linq;

namespace JwtWebApi.Repositories
{
    public class HeroesRepository : IHeroesRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeroesRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Hero>> GetAllHeroesAsync()
        {


            List<Hero> heroes = new List<Hero> {
       new Hero("joker", "attacker", 0, "purple", 500, 500, null, 0,"boss"),
       new Hero("black_widow", "defender", 0, "black", 400, 400, null, 0,"boss"),
       new Hero("wanda", "attacker",  0, "red", 300, 300, null, 0,"boss"),
       new Hero("ant_man", "attacker", 0,  "black and red", 200, 200, null, 0,"boss"),
       new Hero("spiderman_improved", "defender", 0, "yellow and red", 252, 252, null, 0,"boss"),
       new Hero("thor", "attacker", 0, "red and gold", 100, 100, null, 0,"boss"),
       new Hero("super_man", "defender", 0, "blue and red", 59, 59, null, 0,"boss"),
       new Hero("doctor_strange", "defender", 0, "blue and red", 58, 58, null, 0,"boss"),
       new Hero("ironMan", "attacker", 0, "red and gold", 30, 30, null, 0,"boss"),
       new Hero("flash", "defender", 0, "red and yellow", 28, 28, null, 0,"boss"),
       new Hero("dark_knight", "attacker",  0, "black", 36, 36, null, 0,"boss"),
       new Hero("spiderman", "attacker", 0, "red", 25, 25, null, 0,"boss"),
       new Hero("batman", "attacker", 0, "black", 25, 25, null, 0,"boss"),
       new Hero("captain_America", "defender", 0, "red", 14, 14, null, 0,"boss")
};


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
            double powerIncrease = (DateTime.Now.Minute % 10 + 1) / 100.0;
            hero.CurrentPower = (int)(hero.CurrentPower * (1 + powerIncrease));


            hero.LastTimeHeroTrained = formattedDate;
                hero.AmountOfTimeHeroTrained++;
            

            ///להוסיף שדה של היום ראשון שהתאמנתי ושדה של הפעם האחרונה שהתאמנתי וכמות הפעמים שהתאמנתי באותו יום 
            
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> AddHeroAsync(string nameOfHero, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            var heroes = await _context.AllHeroes.FirstOrDefaultAsync(b => b.TrainerName == userName);

            IEnumerable<Hero> allHeroes = await GetAllHeroesAsync();
            var newHero =  allHeroes?.FirstOrDefault(b => b.Name == nameOfHero);

    

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


            Random random = new Random();
       
          ////id
            Hero newObj = new Hero()
            {
                Name = newHero.Name,
                Ability = newHero.Ability,
                TrainerName = userName,
                FirstDayHeroTrained = 0,
                StartingPower = newHero.StartingPower,
                CurrentPower = newHero.CurrentPower,
                SuitColors = newHero.SuitColors,
                LastTimeHeroTrained = newHero.LastTimeHeroTrained, 
                AmountOfTimeHeroTrained = 0
            };
            _context.AllHeroes.Add(newObj);
            await _context.SaveChangesAsync();
            return true;
        }
        public string? getUserNameByToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (token == "")
                return "you dont have token send";

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            return decodedToken?.Claims?.ToArray()[0]?.Value;
        }
    }

}