using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class TimingController : Controller
    {
        private readonly IConfiguration _configuration;

        public TimingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Timing()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var timings = new List<Timing>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllTimings", conn)) // Stored procedure name
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        timings.Add(new Timing
                        {
                            TimingId = Convert.ToInt32(reader["TimingId"]),
                            DayOfWeek = reader["DayOfWeek"].ToString(),
                            StartTime = (TimeSpan)reader["StartTime"],
                            EndTime = (TimeSpan)reader["EndTime"],
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            return Json(timings);
        }
    }
}
