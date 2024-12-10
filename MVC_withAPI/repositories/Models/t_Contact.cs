using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace repositories.Models
{
    public class t_Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
         public int? ContactId { get; set; }
        public int UserId { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Contact name is required.")]
        public string ContactName { get; set;}
        
        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        // [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set;}

        [MaxLength(500)] 
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set;}

        [MaxLength(10)] 
        [Required(ErrorMessage = "Mobile is required.")]
        public string? Mobile { get; set;}  

        public IFormFile? formfile { get; set;}

        [MaxLength(100)] 
        [Required(ErrorMessage = "Group is required.")]
        public string? Group { get; set;}

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set;}

        [MaxLength(100)] 
        public string? Image{ get; set;}
    }
}