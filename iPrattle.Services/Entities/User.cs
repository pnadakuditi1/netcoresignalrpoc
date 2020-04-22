using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<UserContact> Contacts { get; set; }
    }
}
