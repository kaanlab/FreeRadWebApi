using FreeRadWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FreeRadWebApi.Controllers
{
    public class UserAttributesController : ApiController
    {
        private readonly IRepository _repository;

        public UserAttributesController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserAttribute> Get() => _repository.GetAllUserAttributes();

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> GetUserAttr(int id)
        {
            UserAttribute userAttr = await _repository.FindUserAttrAsync(id);
            if (userAttr == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атрибут пользователя с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            return Ok(userAttr);
        }

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> PostUserAttr([FromBody]UserAttribute userAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddUserAttr(userAttr);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = userAttr.Id }, userAttr);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserAttr(int id, [FromBody]UserAttribute userAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userAttr.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!UserAttrExists(id))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атибуты пользователя с Id:{id}!")
                };
                //return NotFound();
                throw new HttpResponseException(message);
            }

            try
            {
                _repository.EditUserAttr(userAttr);
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

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> DeleteUserAttr(int id, [FromBody]UserAttribute deleteUserAttr)
        {
            UserAttribute userAttr = _repository.FindUserAttr(id);
            if (userAttr == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атибуты пользователя с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            if (userAttr.UserName != deleteUserAttr.UserName || userAttr.Value != deleteUserAttr.Value)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить доп.атрибут. Проверьте тело запроса!")
                };

                throw new HttpResponseException(message);
            }

            try
            {
                _repository.DeleteUserAttr(userAttr);
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

            return Ok(userAttr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAttrExists(int id) => _repository.GetAllUserAttributes().Count(u => u.Id == id) > 0;
    }
}
