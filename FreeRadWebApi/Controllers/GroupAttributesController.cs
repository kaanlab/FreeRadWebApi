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
    public class GroupAttributesController : ApiController
    {
        private readonly IRepository _repository;

        public GroupAttributesController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<GroupAttribute> Get() => _repository.GetAllGroupAttributes();

        [ResponseType(typeof(GroupAttribute))]
        public async Task<IHttpActionResult> GetGroupAttr(int id)
        {
            GroupAttribute groupAttr = await _repository.FindGroupAttrAsync(id);
            if (groupAttr == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атрибут группы с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            return Ok(groupAttr);
        }

        [ResponseType(typeof(GroupAttribute))]
        public async Task<IHttpActionResult> PostGroupAttr([FromBody]GroupAttribute groupAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddGroupAttr(groupAttr);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = groupAttr.Id }, groupAttr);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroupAttr(int id, [FromBody]GroupAttribute groupAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupAttr.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!GroupAttrExists(id))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атибут группы с Id:{id}!")
                };
                //return NotFound();
                throw new HttpResponseException(message);
            }

            try
            {
                _repository.EditGroupAttr(groupAttr);
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

        [ResponseType(typeof(GroupAttribute))]
        public async Task<IHttpActionResult> DeleteGroupAttr(int id, [FromBody]GroupAttribute deleteGroupAttr)
        {
            GroupAttribute groupAttr = _repository.FindGroupAttr(id);
            if (groupAttr == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти доп.атибуты группы с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            if (groupAttr.GroupName != deleteGroupAttr.GroupName || groupAttr.Value != deleteGroupAttr.Value)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить доп.атрибут. Проверьте тело запроса!")
                };

                throw new HttpResponseException(message);
            }

            try
            {
                _repository.DeleteGroupAttr(groupAttr);
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

            return Ok(groupAttr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupAttrExists(int id) => _repository.GetAllGroupAttributes().Count(g => g.Id == id) > 0;
    }
}
