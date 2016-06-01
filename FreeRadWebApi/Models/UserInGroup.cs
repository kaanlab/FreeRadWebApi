using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeRadWebApi.Models
{
    public class UserInGroup
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public int Priority { get; set; }
    }
}