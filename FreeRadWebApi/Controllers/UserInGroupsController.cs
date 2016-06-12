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
                return NotFound();
            }

            return Ok(userInGroup);
        }

        [ResponseType(typeof(UserInGroup))]
        public async Task<IHttpActionResult> PostUserInGroup(UserInGroup userInGroup)
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
        public async Task<IHttpActionResult> PutGroup(int id, UserInGroup userInGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInGroup.Id)
            {
                return BadRequest();
            }

            if (!UserInGroupExists(id))
            {
                return NotFound();
            }

            try
            {
                _repository.EditUserInGroup(userInGroup);

                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(UserInGroup))]
        public async Task<IHttpActionResult> DeleteGroup(int id)
        {
            UserInGroup userInGroup = _repository.FindUserInGroup(id);
            if (userInGroup == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DeleteUserFromGroup(userInGroup);
                await _repository.SaveAsync();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
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
