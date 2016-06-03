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

        [HttpGet]
        public IEnumerable<UserAttribute> Get()
        {
            return _repository.GetAllUserAttributes();
        }

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> GetUserAttr(int id)
        {
            UserAttribute userAttr = await _repository.FindUserAttrAsync(id);
            if (userAttr == null)
            {
                return NotFound();
            }

            return Ok(userAttr);
        }

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> PostUserAttr(UserAttribute userAttr)
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
        public async Task<IHttpActionResult> PutUserAttr(int id, UserAttribute userAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userAttr.Id)
            {
                return BadRequest();
            }

            if (!UserAttrExists(id))
            {
                return NotFound();
            }

            try
            {
                _repository.EditUserAttr(userAttr);

                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(UserAttribute))]
        public async Task<IHttpActionResult> DeleteUserAttr(int id)
        {
            UserAttribute userAttr = _repository.FindUserAttr(id);
            if (userAttr == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DeleteUserAttr(userAttr);
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
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

        private bool UserAttrExists(int id)
        {
            return _repository.GetAllUserAttributes().Count(u => u.Id == id) > 0;
        }
    }
}
