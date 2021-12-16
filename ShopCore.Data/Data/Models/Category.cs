using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopCore.Models
{
    
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; } 
        public string CategoryName { get; set; }
    }
}
