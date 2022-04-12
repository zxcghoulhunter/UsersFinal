using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsersChat.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DateOfRegitration { get; set; }
        public DateTime LastLogin { get; set; }
        public string Password { get; set; }
        public bool IsBanned { get; set; }
        public bool IsChecked { get; set; }
    }
    public class UserList
    {
        //use CheckBoxModel class as list 
        public List<User> Users { get; set; }
    }

}
