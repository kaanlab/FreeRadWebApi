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
                return NotFound();
            }

            return Ok(groupAttr);
        }

        [ResponseType(typeof(GroupAttribute))]
        public async Task<IHttpActionResult> PostGroupAttr(GroupAttribute groupAttr)
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
        public async Task<IHttpActionResult> PutGroupAttr(int id, GroupAttribute groupAttr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupAttr.Id)
            {
                return BadRequest();
            }

            if (!GroupAttrExists(id))
            {
                return NotFound();
            }

            try
            {
                _repository.EditGroupAttr(groupAttr);

                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(GroupAttribute))]
        public async Task<IHttpActionResult> DeleteGroupAttr(int id)
        {
            GroupAttribute groupAttr = _repository.FindGroupAttr(id);
            if (groupAttr == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DeleteGroupAttr(groupAttr);
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
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
