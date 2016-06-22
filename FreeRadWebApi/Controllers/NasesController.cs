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
    public class NasesController : ApiController
    {
        private readonly IRepository _repository;

        public NasesController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Nas> Get() => _repository.GetAllNas();

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetNas(int id)
        {
            Nas nas = await _repository.FindNasAsync(id);

            if (nas == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти устройство с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            return Ok(nas);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostNas([FromBody]Nas nas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddNas(nas);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = nas.Id }, nas);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNas(int id, [FromBody]Nas nas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nas.Id)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Проверьте параметры запроса! Поля не совпадают!")
                };

                throw new HttpResponseException(message);
            }

            if (!NasExists(id))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти устройство с Id:{id}!")
                };
                //return NotFound();
                throw new HttpResponseException(message);
            }

            try
            {
                _repository.EditNas(nas);

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
        public async Task<IHttpActionResult> DeleteNas(int id, [FromBody]Nas deleteNas)
        {
            Nas nas = _repository.FindNas(id);
            if (nas == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Не могу найти устройство с Id:{id}!")
                };

                throw new HttpResponseException(message);
            }

            if (nas.NasName != deleteNas.NasName || nas.Secret != deleteNas.Secret)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не могу удалить устройство. Проверьте тело запроса!")
                };

                throw new HttpResponseException(message);
            }

            try
            {
                _repository.DeleteNas(nas);
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

            return Ok(nas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NasExists(int id) => _repository.GetAllNas().Count(n => n.Id == id) > 0;
    }
}