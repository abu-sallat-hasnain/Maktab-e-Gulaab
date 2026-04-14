using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class SessionController : Controller
    {
        private readonly IConfiguration _configuration;

        public SessionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Session()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var sessions = new List<Session>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllSessions", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new Session
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SessionName = reader["SessionName"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                        });
                    }
                }
            }

            return Json(sessions);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Session request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertSession", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SessionName", request.SessionName);
                cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", request.EndDate);
                cmd.Parameters.AddWithValue("@Status", request.Status);
                cmd.Parameters.AddWithValue("@Remarks", request.Remarks);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Session added successfully." });
        }

        [HttpDelete("Session/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteSession", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SessionId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Session deleted successfully." });
        }

        [HttpGet("Session/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Session obj = new Session();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetSessionById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.Id = Convert.ToInt32(reader["Id"]);
                        obj.SessionName = reader["SessionName"].ToString();
                        obj.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        obj.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        obj.Status = reader["Status"].ToString();
                        obj.Remarks = reader["Remarks"].ToString();
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Session request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateSession", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@SessionName", request.SessionName);
                cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", request.EndDate);
                cmd.Parameters.AddWithValue("@Status", request.Status);
                cmd.Parameters.AddWithValue("@Remarks", request.Remarks);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Session updated successfully." });
        }
    }
}
