using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class UserConstants
    {
        public static List<User> Users = new List<User>()
        {
            new User() { UserName = "dmiljevic", Password = "password", Role = "Employee", Email="duje.miljevic", FirstName="duje", LastName="miljevic"},
            new User() {UserName = "jnincevic", Password = "losPassword", Role = "Employee", Email="duje.miljevic", FirstName="duje", LastName="miljevic"},
        };
    }
}
