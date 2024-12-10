using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int? TaskId { get; set; }

        public int ProjectId { get; set; }

        public int EmpId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int EstimatedDays { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        public Project? project { get; set; }
        public Employee?  employee{ get; set; }
    }
}