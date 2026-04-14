using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IConfiguration _configuration;

        public SubjectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Subject()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var subjects = new List<Subject>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllSubjects", conn)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Code = reader["Code"].ToString(),
                            Name = reader["Name"].ToString(),
                            CreditHours = Convert.ToInt32(reader["CreditHours"]),
                            Type = reader["Type"].ToString(),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            return Json(subjects);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Subject request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Code", request.Code);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@CreditHours", request.CreditHours);
                cmd.Parameters.AddWithValue("@Type", request.Type);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Subject added successfully." });
        }

        [HttpDelete("Subject/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Subject deleted successfully." });
        }

        [HttpGet("Subject/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Subject obj = new Subject();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetSubjectById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.Id = Convert.ToInt32(reader["Id"]);
                        obj.Code = reader["Code"].ToString();
                        obj.Name = reader["Name"].ToString();
                        obj.CreditHours = Convert.ToInt32(reader["CreditHours"]);
                        obj.Type = reader["Type"].ToString();
                        obj.Description = reader["Description"].ToString();
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Subject request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@Code", request.Code);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@CreditHours", request.CreditHours);
                cmd.Parameters.AddWithValue("@Type", request.Type);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Subject updated successfully." });
        }
    }
}
