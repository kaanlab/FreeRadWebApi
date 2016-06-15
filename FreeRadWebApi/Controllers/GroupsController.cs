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
    public class GroupsController : ApiController
    {
        private readonly IRepository _repository;

        public GroupsController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Group> Get() => _repository.GetAllGroups();

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(int id)
        {
            Group group = await _repository.FindGroupAsync(id);

            if (group == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти группу с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            return Ok(group);
        }

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> PostGroup([FromBody]Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddGroup(group);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroup(int id, [FromBody]Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!GroupExists(id))
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
                _repository.EditGroup(group);

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

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> DeleteGroup(int id, [FromBody]Group deleteGroup)
        {
            Group group = _repository.FindGroup(id);
            if (group == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти пользователя с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            if (group.GroupName != deleteGroup.GroupName || group.Value != deleteGroup.Value)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить группу. Проверьте тело запроса!")
                };

                throw new HttpResponseException(message);
            }

            try
            {
                _repository.DeleteGroup(group);
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

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id) => _repository.GetAllGroups().Count(g => g.Id == id) > 0;
    }
}
