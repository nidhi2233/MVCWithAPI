using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int ProjectId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Project Name is required.")]
        public string? ProjectName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}