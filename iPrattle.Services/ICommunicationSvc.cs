using iPrattle.Services.Entities;
using iPrattle.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iPrattle.Services
{
    public interface ICommunicationSvc
    {
        List<ContactModel> GetUnknownContactsToAdd(Guid userId);

        string AddToUserContacts(Guid userId, Guid unknownContactId);

        List<ContactModel> GetContacts(Guid userId);

        List<Message> GetMessages(Guid userId, Guid contactId);

        Task<bool> AddMessage(Message message);
    }
}
