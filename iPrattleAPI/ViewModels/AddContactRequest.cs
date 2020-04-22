using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPrattleAPI.ViewModels
{
    public class AddContactRequest
    {
        public Guid UserId { get; set; }

        public Guid ContactId { get; set; }
    }
}
