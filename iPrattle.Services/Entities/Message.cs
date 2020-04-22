using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual User Sender { get; set; }

        public virtual User Receiver { get; set; }
    }
}
