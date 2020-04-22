using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPrattle.Services.Entities;
using iPrattle.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace iPrattle.Services
{
    public class CommunicationSvc : ICommunicationSvc
    {
        IConfiguration _config;
        protected readonly PrattleDbContext _dbContext;

        public CommunicationSvc(PrattleDbContext dbContext, IConfiguration config)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public List<ContactModel> GetContacts(Guid userId)
        {
            Log.Information($"Getting contacts for userId: {userId}");
            var contacts = _dbContext.Contacts
                    .Include(c => c.Contact)
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CreatedOn)
                    .Select(x => new ContactModel
                    {
                        Id = x.Contact.Id,
                        Name = $"{x.Contact.FirstName} {x.Contact.LastName}",
                        Username = $"{x.Contact.Username}"
                    });
            return contacts.ToList();
        }

        public List<ContactModel> GetUnknownContactsToAdd(Guid userId)
        {
            Log.Information($"Getting unknown contacts for user: {userId}");
            var currentContactIds = _dbContext.Contacts
                                    .Where(c => c.UserId == userId)
                                    .Select(c => c.ContactId)
                                    .ToList();
            currentContactIds.Add(userId);

            var availableContacts = _dbContext.Users
                                    .Where(u => !currentContactIds.Any(x => x == u.Id))
                                    .Select(c => new ContactModel
                                    {
                                        Id = c.Id,
                                        Name = $"{c.FirstName} {c.LastName}",
                                        Username = $"{c.Username}"
                                    }).ToList();

            return availableContacts;
        }

        public string AddToUserContacts(Guid userId, Guid contactId)
        {
            var existingUserContact = _dbContext.Contacts.Where(c => c.UserId == userId && c.ContactId == contactId).FirstOrDefault();
            if (existingUserContact != null)
            {
                return "Existing contact";
            }
            else
            {
                return AddContact(userId, contactId) ? null : "Unable to add contact. Please try again.";
            }
        }

        private bool AddContact(Guid userId, Guid unknownContactId)
        {
            Log.Information($"Adding {unknownContactId} to {userId}'s contacts");
            var contact = new UserContact
            {
                UserId = userId,
                ContactId = unknownContactId,
                CreatedOn = DateTime.UtcNow
            };
            _dbContext.Contacts.Add(contact);
            return _dbContext.SaveChanges() == 1;
        }

        public Task<bool> AddMessage(Message newMessage)
        {
            Log.Information($"Adding {newMessage.Body} from {newMessage.ReceiverId} to {newMessage.SenderId}");
            _dbContext.Messages.Add(newMessage);
            return Task.FromResult(_dbContext.SaveChanges() == 1);
        }

        public List<Message> GetMessages(Guid userId, Guid contactId)
        {
            Log.Information($"Getting messages for {userId}");
            var messages = _dbContext.Messages
                                    .Where(m => (m.SenderId == userId && m.ReceiverId == contactId) ||
                                            (m.SenderId == contactId && m.ReceiverId == userId))
                                    .OrderBy(m => m.CreatedOn)
                                    .ToList();

            return messages;
        }

    }
}
