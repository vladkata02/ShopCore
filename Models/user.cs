using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopCore.Models
{
    public class user
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
