using iPrattle.Services.Entities;
using iPrattle.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services
{
    public interface IUserSvc
    {
        UserModel Login(string username, string password);

        string Register(User user);
    }
}
