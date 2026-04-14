namespace MaktabeGulabProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Qualification { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
    }

}
