using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using REST03LiteDB.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace REST03LiteDB.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PartnersController : ControllerBase
    {
        string DbName = @"MyData.db";

        // GET: api/partners
        [HttpGet]
        public ActionResult<IEnumerable<Partner>> Get()
        {
            // Open database (or create if not exits)
            using (var db = new LiteDatabase(DbName))
            {
                var partners = db.GetCollection<Partner>("partners");
                return partners.FindAll().ToList();
            }
        }

        // GET api/partners/5
        [HttpGet("{id}")]
        public ActionResult<Partner> Get(int id)
        {
            using (var db = new LiteDatabase(DbName))
            {
                var partner = db.GetCollection<Partner>("partners").FindById(id);
                if (partner == null)
                {
                    return NotFound();
                }
                return new ObjectResult(partner);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<Partner> Post(Partner partner)
        {
            if (!partner.Valid())
            {
                return BadRequest();
            }

            using (var db = new LiteDatabase(DbName))
            {
                var partners = db.GetCollection<Partner>("partners");
                //Проверка Контрагента на наличие в БД по параметру Name
                if (partners.Exists(x => x.Name == partner.Name) || partners.Exists(x => x.Id == partner.Id))
                {
                    return BadRequest();
                }

                partners.Insert(partner); 
            }
            return Ok(partner);
        }

        // PUT api/partners/5
        [HttpPut("{id}")]
        public ActionResult<Partner> Put(Partner partner)
        {
            if (!partner.Valid())
            {
                return BadRequest();
            }

            using (var db = new LiteDatabase(DbName))
            {
                var partners = db.GetCollection<Partner>("partners");
                var target = partners.FindById(partner.Id);

                if (target == null)
                {
                    return NotFound();
                }
                if (partners.Exists(x => x.Name == partner.Name))
                {
                    return BadRequest();
                }

                partners.Update(partner);
                return Ok(partner);
            }        
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult<Partner> Delete(int id)
        {
            using (var db = new LiteDatabase(DbName))
            {
                var partners = db.GetCollection<Partner>("partners");
                var target = partners.FindById(id);

                if (target == null)
                {
                    return NotFound();
                }
                partners.Delete(id);
                return Ok(target);
            }
        }
    }
}
