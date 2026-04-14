namespace MaktabeGulabProject.Models
{
    
        public class Session
        {
            public int Id { get; set; }
            public string SessionName { get; set; }   
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; }        
            public string Remarks { get; set; }
        }

    

}
