using Microsoft.AspNetCore.Identity;

namespace taskmanager.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
