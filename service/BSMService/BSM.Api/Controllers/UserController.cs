using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BSM.Common.DB;
using BSM.Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSM.Api.Controllers
{
    [Route("api/users")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly BSMContext _context;
        private readonly IConfiguration _config;

        public UserController(BSMContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            
            var users = await _context.Users.ToListAsync();
            users.ForEach(u =>
            {
                u.Password = null;
            });
            return users;
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.Password = null;
            return user;
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]UserRegisterRequest userRegister)
        {
            if (await _context.Users.AnyAsync(u => u.LoginID.Equals(userRegister.LoginID, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(string.Format("Login ID {0} already exist", userRegister.LoginID));
            }
            if (await _context.Users.AnyAsync(u => u.Name.Equals(userRegister.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(string.Format("Name {0} already exist", userRegister.Name));
            }
            if (userRegister.Email != null &&
                await _context.Users.AnyAsync(u => u.Email.Equals(userRegister.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(string.Format("Email {0} already exist", userRegister.Email));
            }
            if (userRegister.MobilePhone != null &&
                await _context.Users.AnyAsync(u => u.MobilePhone.Equals(userRegister.MobilePhone, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(string.Format("Mobile number {0} already exist", userRegister.MobilePhone));
            }

            User user = new User();
            user.LoginID = userRegister.LoginID;
            user.Name = userRegister.Name;
            user.Title = userRegister.Title;
            user.IsAdmin = userRegister.IsAdmin;
            user.Email = userRegister.Email;
            user.MobilePhone = userRegister.MobilePhone;
            
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            string hashedPass = hasher.HashPassword(user, userRegister.Password);
            user.Password = hashedPass;
            user.PasswordChangedAt = DateTime.Now;
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Authenticate([FromBody][Required]UserLoginRequest credential)
        {
            //verify user exist
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginID.Equals(credential.LoginID, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Unauthorized("User does not exist");
            }
            //verify password
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            if (hasher.VerifyHashedPassword(user, user.Password, credential.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized("Password incorrect");
            } 
            else
            {
                var secret = Encoding.ASCII.GetBytes(_config.GetValue<string>("SecretKey"));
              
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("id", user.Id.ToString()),
                        new Claim("username", user.LoginID),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim("mobile", user.MobilePhone)
                    }),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                user.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();

                user.Password = null;
                UserLoginResponse response = new UserLoginResponse(user, token.RawData);
                return response;
            }
        }

        // PUT api/<controller>/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<User>> UpdateUser(long id, [FromBody]UserUpdateRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(request.LoginID))
            {
                //verify LoginID not exist
                if (await _context.Users.AnyAsync(u => 
                    u.LoginID.Equals(request.LoginID, StringComparison.OrdinalIgnoreCase) && 
                    u.Id != id))
                {
                    return BadRequest($"LoginID '{request.LoginID}' already exist");
                }
                user.LoginID = request.LoginID;
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                //verify Name not exist
                if (await _context.Users.AnyAsync(u => 
                    u.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
                    u.Id != id))
                {
                    return BadRequest($"Name '${request.Name}' already exist");
                }
                user.Name = request.Name;
            }
            if (request.Company != null)
            {
                user.Company = request.Company;
            }
            if (request.Department != null)
            {
                user.Department = request.Department;
            }
            if (request.Email != null)
            {
                user.Email = request.Email;
            }
            if (request.IsAdmin.HasValue)
            {
                user.IsAdmin = request.IsAdmin.Value;
            }
            if (request.MobilePhone != null)
            {
                user.MobilePhone = request.MobilePhone;
            }
            if (request.Title != null)
            {
                user.Title = request.Title;
            }
            if (request.Password != null)
            {
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPass = hasher.HashPassword(user, request.Password);
                user.Password = hashedPass;
                user.PasswordChangedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            user.Password = null;
            return user;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
