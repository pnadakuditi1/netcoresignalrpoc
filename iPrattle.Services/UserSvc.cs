using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iPrattle.Services.Entities;
using iPrattle.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace iPrattle.Services
{
    public class UserSvc : IUserSvc
    {
        IConfiguration _config;
        protected readonly PrattleDbContext _dbContext;

        public UserSvc(PrattleDbContext dbContext, IConfiguration config)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public UserModel Login(string username, string password)
        {
            return Get(username);
        }

        public string Register(User user)
        {
            var existingUser = this.Get(user.Username);
            if (existingUser != null)
            {
                return "Username not available, please choose another one.";
            }
            else
            {
                return AddUser(user) ? null : "Unable to add user. Please try again";
            }
        }

        private UserModel Get(string username)
        {
            Log.Information($"Get User for username: {username}");
            return _dbContext.Users
                    .Where(u => u.Username == username)
                    .Select(x => new UserModel
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName}  {x.LastName}",
                        Username = $"{x.Username}"
                    }).FirstOrDefault();
        }

        private bool AddUser(User user)
        {
            Log.Information($"Add User with username: {user.Username}");
            _dbContext.Users.Add(user);
            return _dbContext.SaveChanges() == 1;
        }

    }
}
