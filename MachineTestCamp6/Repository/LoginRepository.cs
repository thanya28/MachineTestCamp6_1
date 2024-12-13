using MachineTestCamp6.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MachineTestCamp6.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly XyztechnologiesContext _context;

        public LoginRepository(XyztechnologiesContext context)
        {
            _context = context;
        }

        public JsonResult Deletelogin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Invalid user id"

                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                }
               
                if (_context == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Database coontext is not initialized"

                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                //Find the employee by id
                var existing = _context.UserRegistrations.Find(id);
                if (existing == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "User  not found"

                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                //remove

                _context.UserRegistrations.Remove(existing);



                //save changes to the database
                _context.SaveChangesAsync();


                return new JsonResult(new
                {
                    success = true,
                    message = "Userlogin details  deleted successfully"

                })
                {
                    StatusCode = StatusCodes.Status200OK
                };

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Database coontext is not initialized"

                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ActionResult<IEnumerable<UserRegistration>>> GetAllUsers()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.UserRegistrations.Include(r => r.Role).ToListAsync();
                }

                //return an empty list if context is null
                return new List<UserRegistration>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserRegistration> RegisterUserAsync(UserRegistration user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User data is null");
            }

            try
            {
              
                await _context.UserRegistrations.AddAsync(user);
                await _context.SaveChangesAsync();

                
                var registeredUser = await _context.UserRegistrations
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == user.UserId);

                return registeredUser;
            }
            catch (DbUpdateException ex)
            {
              
                throw new InvalidOperationException("An error occurred while registering the user.", ex);
            }
            catch (Exception ex)
            {
               
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async  Task<ActionResult<UserRegistration>> SearchById(int id)
        {
            try
            {
                if (_context != null)
                {
                   
                    var employee = await _context.UserRegistrations
                    .Include(role => role.Role)
                    .FirstOrDefaultAsync(u => u.UserId == id);
                    return employee;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult<UserRegistration>> Updatelogin(int id, UserRegistration userRegistration)
        {
            try
            {//check idf employee object is not null
                if (userRegistration == null)
                {
                    throw new ArgumentNullException(nameof(userRegistration), "Existingusers data is null");

                }
                // ensure context is not null
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
                //Find the employee by id
                var existingusers = await _context.UserRegistrations.FindAsync(id);
                if (existingusers == null)
                {
                    return null;
                }

                //map values with fields - update
                existingusers.Age = userRegistration.Age;
                existingusers.FullName = userRegistration.FullName;
                existingusers.Address = userRegistration.Address;
                existingusers.Username = userRegistration.Username;
                existingusers.Password = userRegistration.Password;


                //save changes to the database
                await _context.SaveChangesAsync();
                //retrive the employee with the related departement
                var login = await _context.UserRegistrations.Include(r => r.Role)
                    .FirstOrDefaultAsync(l => l.UserId == userRegistration.UserId);
                return login;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Validate User Credentials
        public async Task<UserRegistration> ValidateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username or password is invalid.");
            }

            try
            {
                var user = await _context.UserRegistrations
                    .Include(u => u.Role) 
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

                return user; 
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while validating the user.", ex);
            }
        }
    }
}
