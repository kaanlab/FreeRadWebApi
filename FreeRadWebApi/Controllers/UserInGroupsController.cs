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
    public class UserInGroupsController : ApiController
    {
        private readonly IRepository _repository;

        public UserInGroupsController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserInGroup> Get() => _repository.GetAllUsersInGroup();

        [ResponseType(typeof(UserInGroup))]
        public async Task<IHttpActionResult> GetUserInGroup(int id)
        {
            UserInGroup userInGroup = await _repository.FindUserInGroupAsync(id);
            if (userInGroup == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя в группе с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            return Ok(userInGroup);
        }

        [ResponseType(typeof(UserInGroup))]
        public async Task<IHttpActionResult> PostUserInGroup([FromBody]UserInGroup userInGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddUserToGroup(userInGroup);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = userInGroup.Id }, userInGroup);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserInGroup(int id, [FromBody]UserInGroup userInGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInGroup.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!UserInGroupExists(id))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя в группе с Id:{id}!")
                };
                //return NotFound();
                throw new HttpResponseException(message);
            }

            try
            {
                _repository.EditUserInGroup(userInGroup);

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

        [ResponseType(typeof(UserInGroup))]
        public async Task<IHttpActionResult> DeleteUserInGroup(int id, [FromBody]UserInGroup deleteUserInGroup)
        {
            UserInGroup userInGroup = _repository.FindUserInGroup(id);
            if (userInGroup == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя в группе с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            if (userInGroup.GroupName != deleteUserInGroup.GroupName || userInGroup.UserName != deleteUserInGroup.UserName)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить пользователя из группы. Проверьте тело запроса!")
                };

                throw new HttpResponseException(message);
            }

            try
            {
                _repository.DeleteUserFromGroup(userInGroup);
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

            return Ok(userInGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserInGroupExists(int id) => _repository.GetAllUsersInGroup().Count(ug => ug.Id == id) > 0;
    }
}
