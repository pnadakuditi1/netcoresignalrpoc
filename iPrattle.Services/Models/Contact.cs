using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }
    }

    public class ContactModel : UserModel
    {
    }
}
