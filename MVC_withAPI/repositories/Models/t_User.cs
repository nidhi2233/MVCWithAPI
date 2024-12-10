using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace repositories.Models
{
    public class t_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int UserId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set;}

        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set;}

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public String? Password { get; set;}
        
        [MaxLength(100)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set;}

        [MaxLength(200)]
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set;}

        [MaxLength(10)]  
        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid mobile number.")]
        public string? Mobile { get; set;}
       [Required(ErrorMessage = "Image is required.")]  
          

        public IFormFile formfile { get; set;}

        [MaxLength(100)]
        [Required(ErrorMessage = "Gender is required.")]  
        public string? Gender { get; set;}
        [MaxLength(100)]
        public string? Image { get; set;}

    }
}