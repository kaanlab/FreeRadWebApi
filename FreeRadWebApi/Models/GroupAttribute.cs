﻿
namespace FreeRadWebApi.Models
{
    public class GroupAttribute
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Attribute { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
    }
}