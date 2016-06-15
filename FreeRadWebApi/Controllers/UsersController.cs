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

        public IEnumerable<User> Get() => _repository.GetAllUsers();

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await _repository.FindUserAsync(id);

            if (user == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя с Id:{id}!")
                };
                
                throw new HttpResponseException(message);
            }
                        
            return Ok(user);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser([FromBody]User user)
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
        public async Task<IHttpActionResult> PutUser(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!UserExists(id))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя с Id:{id}!")
                };
                //return NotFound();
                throw new HttpResponseException(message);
            }

            try
            {
                _repository.EditUser(user);

                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.ToString())
                };

                throw new HttpResponseException(message);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id, [FromBody]User deleteUser)
        {
            User user = _repository.FindUser(id);
            if (user == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя с Id:{id}!")
                };
                
                throw new HttpResponseException(message);
            }

            if (user.UserName != deleteUser.UserName || user.Value != deleteUser.Value)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить пользователя. Проверьте тело запроса!")
                };
                
                throw new HttpResponseException(message);                
            }

            try
            {
                _repository.DeleteUser(user); 
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.ToString())
                };
                
                throw new HttpResponseException(message);
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

        private bool UserExists(int id) => _repository.GetAllUsers().Count(u => u.Id == id) > 0;
    }
}
