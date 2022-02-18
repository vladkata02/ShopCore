namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Roles { get; set; }

        public string FullName { get; set; }

        public string LoginType { get; set; }

        public string Email { get; set; }

        public User()
        {
        }

        public User(string userName, string email)
        {
            this.FullName = userName;
            this.Username = userName;
            this.Email = email;
            this.Roles = "client";
            this.LoginType = "Facebook";
        }

        public User(string fullname, string userName, string password, string email)
        {
            this.FullName = fullname;
            this.Username = userName;
            this.Password = password;
            this.Email = email;
            this.Roles = "client";
            this.LoginType = "local";
        }
    }
}
