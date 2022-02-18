namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Roles { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserViewModel(int id, string userName, string password, string roles, string fullName, string email)
        {
            this.Id = id;
            this.Username = userName;
            this.Password = password;
            this.Roles = roles;
            this.FullName = fullName;
            this.Email = email;
        }
    }
}
