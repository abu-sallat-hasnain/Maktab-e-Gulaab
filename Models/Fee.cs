namespace MaktabeGulabProject.Models
{
    public class Fee
    {
        public int Id { get; set; }
        public string StudentRollNumber { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }       
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
    }

}
