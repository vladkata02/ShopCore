using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShopCore.Services.ViewModel
{
    public class RegisterViewModel : Controller
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [System.Web.Mvc.Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
