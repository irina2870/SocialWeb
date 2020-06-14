using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWeb.API.Data;
using SocialWeb.API.Models;

namespace SocialWeb.API.Data {
    public interface IAuthRepository {
        Task<User> Register (User user, string password);
        Task<User> Login (string username, string passwod);
        Task<bool> UserExists (string username);
    }
}