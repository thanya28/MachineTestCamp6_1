using MachineTestCamp6.Model;
using Microsoft.AspNetCore.Mvc;

namespace MachineTestCamp6.Repository
{
    public interface ILoginRepository
    {
       public Task<UserRegistration> ValidateUserAsync(string username, string password);
        public  Task<UserRegistration> RegisterUserAsync(UserRegistration user);
        public Task<ActionResult<IEnumerable<UserRegistration>>> GetAllUsers();
        public Task<ActionResult<UserRegistration>> Updatelogin(int id, UserRegistration userRegistration);
        public JsonResult Deletelogin(int id);
        public Task<ActionResult<UserRegistration>> SearchById(int id);
    }
}
