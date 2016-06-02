using FreeRadWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FreeRadWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IRepository _repository;

        public UsersController(IRepository repository)
        {
            _repository = repository;            
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
           return _repository.GetAllUsers();           
        }

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await _repository.FindUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddUser(user);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!UserExists(id))
            {
                return NotFound();
            }

            try
            {
                _repository.EditUser(user);

                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = _repository.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DeleteUser(user); 
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            
            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return _repository.GetAllUsers().Count(u => u.Id == id) > 0;
        }
    }
}
