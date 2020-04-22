using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services.Entities
{
    public class UserContact
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ContactId { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }

        public virtual User Contact { get; set; }
    }
}
