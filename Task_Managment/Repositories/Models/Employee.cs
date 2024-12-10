using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Repositories.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int EmpId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Employee Name is required.")]
        public string? EmpName { get; set;}

        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set;}

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set;}

        [MaxLength(100)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set;}

        public string? Role { get; set;}

        public IFormFile? formFile{ get; set; }
        public string? ProfileImage { get; set;}
    

    }
}