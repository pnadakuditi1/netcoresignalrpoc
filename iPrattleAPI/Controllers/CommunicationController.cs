using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iPrattle.Services;
using iPrattle.Services.Entities;
using iPrattleAPI.Hubs;
using iPrattleAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace iPrattleAPI.Controllers
{
    [ApiController]
    public class CommunicationController : BaseController
    {
        private readonly ICommunicationSvc _commSvc;
		private readonly IHubContext<PrattleHub> _prattleHub;

		public CommunicationController(ICommunicationSvc commSvc, 
			IConfiguration config, IHubContext<PrattleHub> prattleHub) : base(config)
        {
            Log.Information("CommunicationController Constructor");
			_commSvc = commSvc;
			_prattleHub = prattleHub;
        }

		[HttpGet]
		[Route("~/contacts")]
		public IActionResult GetContacts(Guid userId)
        {
			try
			{
				var contacts = _commSvc.GetContacts(userId);
				return Ok(contacts);
			}
			catch(Exception ex)
			{
				Log.Error($"Failed while getting contacts for user: {userId} : {ex.Message}");
			}
			return BadRequest();
        }

		[HttpGet]
		[Route("~/messages")]
		public IActionResult GetMessages(Guid userId, Guid contactId)
		{
			try
			{
				var messages = _commSvc.GetMessages(userId, contactId);
				return Ok(messages);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed while getting messages for user: {userId} : {ex.Message}");
			}
			return BadRequest();
		}

		[HttpGet]
		[Route("~/contactstoadd")]
		public IActionResult GetContactsToAdd(Guid userId)
		{
			try
			{
				var contactsToAdd = _commSvc.GetUnknownContactsToAdd(userId);
				return Ok(contactsToAdd);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed while getting contacts-to-add for user: {userId} : {ex.Message}");
			}
			return BadRequest();
		}

		[HttpPost("~/addcontact")]
		public IActionResult AddContact([FromBody]AddContactRequest model)
		{
			try
			{
				Log.Information($"Adding contact {model.ContactId} for user {model.UserId}");

				var resultMessage = _commSvc.AddToUserContacts(model.UserId, model.ContactId);
				return Ok(new
				{
					message = resultMessage
				});
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to add ontact {model.ContactId} for user {model.UserId} : {ex.Message}");
			}
			return StatusCode(500);
		}
	}
}
