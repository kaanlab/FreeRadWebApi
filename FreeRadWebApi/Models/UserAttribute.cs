﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeRadWebApi.Models
{
    public class UserAttribute
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Attribute { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
    }
}