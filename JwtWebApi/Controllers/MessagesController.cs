using JwtWebApi.data;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Xml;

namespace JwtWebApi.Controllers
{
    [Route("/messages/")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DataContext _context;
        public MessagesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getAll"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<List<Messages>>> GetMessages()
        {
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)flexible;
            dictionary.Add("messages", await _context.Messages.ToArrayAsync());
            return Ok(dictionary);
        }

        [HttpPost("updateGrade/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<Messages>> updateGrade(int id, updateMsg msg)
        {
            string gread = msg.id.ToString();
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return BadRequest("Message not found");
            message.status = "done";
            message.desccription = gread;

            await _context.SaveChangesAsync();
            return Ok(message);
        }

        [HttpPost("create"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<Messages>> AddHero(Messages hero)
        {
            Messages newmsg = new Messages();
            newmsg.status = hero.status;
            newmsg.type = hero.type;
            newmsg.start_date = hero.start_date;
            newmsg.data = hero.data;
            newmsg.desccription = hero.desccription;
            newmsg.praiority = hero.praiority;
            newmsg.mission_id= hero.mission_id;

            _context.Messages.Add(newmsg);
            await _context.SaveChangesAsync();
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(newmsg.data);
            Messages user = await _context.Messages.SingleAsync((u) => u.data == hero.data);
            xd.Save("C:\\XML\\React App\\"+user.Id+".xml");
            return Ok(user);
        }

        public class updateMsg
        {
            public int id { get; set; }

        }

    }
}
