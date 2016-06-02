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

        [HttpGet]
        public IEnumerable<Group> Get()
        {
            return _repository.GetAllGroups();
        }

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(int id)
        {
            Group group = await _repository.FindGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.AddGroup(group);
            await _repository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = group.Id }, group);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.Id)
            {
                return BadRequest();
            }

            if (!GroupExists(id))
            {
                return NotFound();
            }

            try
            {
                _repository.EditGroup(group);

                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> DeleteGroup(int id)
        {
            Group group = _repository.FindGroup(id);
            if (group == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DeleteGroup(group);
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
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

        private bool GroupExists(int id)
        {
            return _repository.GetAllGroups().Count(g => g.Id == id) > 0;
        }
    }
}
