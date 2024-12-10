using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using repositories.Implementations;
using repositories.Interfaces;
using repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserInterface _userRepo;
        private readonly IConfiguration _configuration;

        public UserController(IUserInterface _userRepo, IConfiguration configuration)
        {
            this._userRepo = _userRepo;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] vm_Login loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepo.Login(loginModel);
                    if (user.UserId != 0)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("Userid", user.UserId.ToString()),
                            new Claim("UserName", user.UserName)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn
                        );
                        return Ok(new
                        {
                            success = true,
                            message = "Login Success",
                            UserData = user,
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Success = false,
                            message = "User does not exist",

                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Validation failed",
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during login: " + ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred during login" });
            }
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] t_User user)
        {
            if (user.formfile != null)
            {
                var filename = user.Email + Path.GetExtension(user.formfile.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/profile_images", filename);
                user.Image = filename.ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    user.formfile.CopyTo(stream);
                }
            }
            Console.WriteLine("user.fname:" + user.UserName);
            var status = await _userRepo.Register(user);
            if (status == 1)
            {
                return Ok(new { success = true, message = "User Registered" });
            }
            else if (status == 0)
            {
                return Ok(new { success = false, message = "User Already Exists" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while Registration" });
            }
        }
    }

}