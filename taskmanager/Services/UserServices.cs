using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using taskmanager.Model;
using Task = System.Threading.Tasks.Task;
using TaskStatus = System.Threading.Tasks.TaskStatus;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace taskmanager.Services
{
    public class UserServices
    {
        private readonly UserManager<User> _userManager;
        public UserServices(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                throw new Exception("Email already exists");
            }

            return await _userManager.CreateAsync(user, password);
        }
      
    }
    




}

