using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopCore.ViewModel
{
    public class ItemViewModel
    {
        public Guid ItemId { get; set; }
        public int CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemBrand { get; set; }
        public string Description { get; set; }
        public decimal ItemPrice { get; set; }
        public byte[] Image { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<SelectListItem> CategorySelectListItem { get; set; }
    }
}