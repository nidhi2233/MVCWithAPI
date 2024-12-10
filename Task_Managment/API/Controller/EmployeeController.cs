using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.Implementations;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeInterface _empRepo;
        private readonly IConfiguration _configuration;
        public ResponseModel<string> _response;
        public EmployeeController(EmployeeInterface _empRepo, IConfiguration _configuration)
        {
            this._empRepo = _empRepo;
            this._configuration = _configuration;
            _response = new ResponseModel<string>();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] Login login)
        {
            if (ModelState.IsValid)
            {
                var response = await _empRepo.Login(login);
                if (response.success)
                {
                    var claims = new[]{
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("EmpId",response.data!.EmpId.ToString()),
                        new Claim("Email",response.data!.Email),
                        new Claim("Role",response.data!.Role!),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn
                    );

                    string redirectUrl = "";
                    if (response.data.Role == "Admin")
                    {
                        redirectUrl = "/Admin/Dashboard";  // Example admin dashboard
                    }
                    else if (response.data.Role == "Employee")
                    {
                        redirectUrl = "/Employee/Dashboard";  // Example employee dashboard
                    }
                    return Ok(new { response.success, response.message, response.data, token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return BadRequest(new { response.success, response.message, response.data });
                }
            }
            else
            {
                return BadRequest(_response);
            }
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddEmployee([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.formFile != null)
                {
                    var fileName = employee.Email + Path.GetExtension(employee.formFile.FileName);
                    var filPath = Path.Combine("../MVC/wwwroot/profile_image", fileName);
                    employee.ProfileImage = fileName.ToString();
                    using (var stream = new FileStream(filPath, FileMode.Create))
                    {
                        employee.formFile.CopyTo(stream);
                    }

                }
                var response = await _empRepo.AddEmployee(employee);
                if (response.success)
                {

                    Console.WriteLine("user.fname:" + employee.EmpName);
                    return Ok(new { response.success, response.message, response.data });
                }
                else
                {
                    return BadRequest(new { response.success, response.message, response.data });
                }
            }
            else
            {
                return BadRequest(_response);
            }
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _empRepo.DeleteEmployee(id);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }

        [HttpGet("GetAllEmployee")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployee()
        {
            var response = await _empRepo.GetAllEmployee();
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }

        }

        [HttpGet("GetEmployeeById/{id}")]
        // [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var response = await _empRepo.GetEmployeeById(id);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }

        }

        [HttpPut("Update")]

        public async Task<IActionResult> UpdateEmployee([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.formFile != null && employee.formFile.Length > 0)
                {
                    var fileName = employee.Email + Path.GetExtension(employee.formFile.FileName);
                    var filPath = Path.Combine("../MVC/wwwroot/profile_image", fileName);
                    employee.ProfileImage = fileName.ToString();
                    using (var stream = new FileStream(filPath, FileMode.Create))
                    {
                        await employee.formFile.CopyToAsync(stream);
                    }
                }
                var response = await _empRepo.UpdateEmployee(employee);
                if (response.success)
                {

                    Console.WriteLine("user.fname:" + employee.EmpName);
                    return Ok(new { response.success, response.message, response.data });
                }
                else
                {
                    return BadRequest(new { response.success, response.message, response.data });
                }
            }
            else
            {
                return BadRequest(_response);
            }

        }
    }
}