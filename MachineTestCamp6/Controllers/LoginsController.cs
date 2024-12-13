using MachineTestCamp6.Model;
using MachineTestCamp6.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace MachineTestCamp6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILoginRepository _loginRepository;

        // Dependency Injection via Constructor
        public LoginsController(IConfiguration config, ILoginRepository loginRepository)
        {
            _config = config;
            _loginRepository = loginRepository;
        }

        #region Validate Username and Password

        [AllowAnonymous]
        [HttpGet("{username}/{userpass}")]
        public async Task<IActionResult> ValidateUserAsync(string username, string userpass)
        {
            // Default response is Unauthorized
            IActionResult response = Unauthorized();
            UserRegistration validUser = await _loginRepository.ValidateUserAsync(username, userpass);

            if (validUser != null)
            {
                var tokenString = GenerateJWTToken(validUser);
                response = Ok(new
                {
                    Uname = validUser.Username,
                    RoleId = validUser.RoleId,
                    Token = tokenString
                });
            }

            return response;
        }

        #endregion

        #region User Registration

        [HttpPost("register")]
   
        public async Task<IActionResult> Register([FromBody] UserRegistration newUser)
        {
            if (newUser == null || string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                var registeredUser = await _loginRepository.RegisterUserAsync(newUser);
                return CreatedAtAction(nameof(Register), new { id = registeredUser.UserId }, registeredUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Get All Users

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<UserRegistration>>> GetAllUsers()
        {
            var employees = await _loginRepository.GetAllUsers();
            if (employees == null)
            {
                return NotFound("No users found ");
            }
            return Ok(employees);
        }

        #endregion
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserRegistration>> SearchById(int id)
        {
            var user = await _loginRepository.SearchById(id);
            if (user == null)
            {
                return NotFound("No employees found ");
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserRegistration>> Updatelogin(int id, UserRegistration register)
        {
            if (ModelState.IsValid)
            {
                var update= await _loginRepository.Updatelogin(id, register);
                if (update != null)
                {
                    return Ok(update);
                }
                else
                {
                    return NotFound();
                }

            }
            return BadRequest();

        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Deletelogin(int id)
        {
            try
            {
                var result = _loginRepository.Deletelogin(id);

                if (result == null)
                {
                    //if result indicates failure or null
                    return NotFound(new
                    {
                        success = false,
                        message = "user could not be deleted or not found"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurs" });
            }
        }

        #region Generate JWT Token

        private string GenerateJWTToken(UserRegistration validUser)
        {
            // Secret key from configuration
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // Algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            // Token descriptor
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: null,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}