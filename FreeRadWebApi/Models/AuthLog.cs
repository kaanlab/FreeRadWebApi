using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeRadWebApi.Models
{
    public class AuthLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string Reply { get; set; }

        public DateTime AuthDate { get; set; }
    }
}