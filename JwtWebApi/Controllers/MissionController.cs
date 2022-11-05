using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NHibernate.Mapping;
using System.Data;
using System.Dynamic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace JwtWebApi.Controllers
{
    [Route("missions")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly DataContext _context;

        public MissionController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("getAll"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<List<Missions>>> GetMission()
        {
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)flexible;
            dictionary.Add("missions", await _context.Missions.ToArrayAsync());
            return Ok(dictionary);
        }

        [HttpGet("findById/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var hero = await _context.Missions.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero not found");
            return Ok(hero);
        }

        [HttpPut("changeToProces/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> changeStatusToInProcess(int id)
        {
            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
                return BadRequest("Hero not found");
            mission.status = "in_process";

            await _context.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpPut("changeToDone/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<User>> changeToDone(int id)
        {
            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
                return BadRequest("Hero not found");
            mission.status = "done";

            await _context.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpPut]
        public async Task<ActionResult<Missions>> updateHero(int i, Missions mission)
        {
            var hero = await _context.Missions.FindAsync(mission.Id);
            if (hero == null)
                return BadRequest("Hero not found");
            hero.the_missions = mission.the_missions;
            hero.user_id = mission.user_id;
            hero.start_date = mission.start_date;
            hero.start_work_date = mission.start_work_date;
            hero.end_date = mission.end_date;
            hero.status = mission.status;
            hero.type = mission.type;
            hero.missions_message = mission.missions_message;
            hero.note = mission.note;
            hero.photo_path = mission.photo_path;
            hero.should_end = mission.should_end;
            hero.progrem_language = mission.progrem_language;
            hero.system = mission.system;

            await _context.SaveChangesAsync();
            return Ok(hero);
        }

        [HttpPost("create"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<Missions>> AddMission(Missions mission)
        {
            Missions newmis = new Missions();
            newmis.the_missions = mission.the_missions;
            newmis.user_id = mission.user_id;
            newmis.start_date = mission.start_date;
            newmis.start_work_date = mission.start_work_date;
            newmis.end_date = mission.end_date;
            newmis.status = mission.status;
            newmis.type = mission.type;
            newmis.missions_message = mission.missions_message;
            newmis.note = mission.note;
            newmis.photo_path = mission.photo_path;
            newmis.should_end = mission.should_end;
            newmis.progrem_language= mission.progrem_language;
            newmis.system = mission.system;

            _context.Missions.Add(newmis);
            await _context.SaveChangesAsync();
            Missions d = await _context.Missions.SingleAsync((u) => u.the_missions == mission.the_missions);
            return Ok(d);
        }
    }
}
