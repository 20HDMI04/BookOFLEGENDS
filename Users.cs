using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOFLEGENDS
{
    public enum Role
    {
        Admin,
        User
    }
    internal class Users
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role RoleQ { get; set; }


        public Users(string username, string password, string email, Role roleQ)
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.RoleQ = roleQ;
        }

        public override string ToString()
        {
            string x = this.RoleQ == Role.Admin ? "Admin" : "User";
            return $"{this.Username};{this.Password};{this.Email};{x}";
        }

        public static Users FromString(string line)
        {
            string[] parts = line.Split(';');
            try
            {
                Role x = parts[3] == "Admin" ? Role.Admin : Role.User;
                return new Users(parts[0], parts[2], parts[1], x);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
