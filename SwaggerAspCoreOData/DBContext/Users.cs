using System;
using System.Collections.Generic;

namespace SwaggerAspCoreOData.DBContext
{
    public partial class Users
    {
        public Users()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Profile { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
