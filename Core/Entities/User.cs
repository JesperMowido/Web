using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : IUser<int>
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public DateTime InsertDate { get; set; }

        [NotMapped]
        public string PasswordHash { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}
