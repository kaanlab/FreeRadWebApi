using FreeRadWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FreeRadWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IRepository _repository;
        


        public UsersController(IRepository repository)
        {
            _repository = repository;            
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var users = _repository.GetAllUsers();
            
            return Ok(users);
        }
    }
}
