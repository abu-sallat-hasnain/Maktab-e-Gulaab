using System.ComponentModel.DataAnnotations;

namespace MaktabeGulabProject.Models
{
    public class Class
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int TotalStudents { get; set; }
        [Required]
        public string Medium { get; set; }
        [Required]
        public string Description { get; set; }
    }

}
