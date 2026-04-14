using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IConfiguration _configuration;

        public TeacherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Teacher()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var teachers = new List<Teacher>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllTeachers", conn)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Qualification = reader["Qualification"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"])
                        });
                    }
                }
            }

            return Json(teachers);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Teacher request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertTeacher", conn)) // Stored Procedure
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", request.FullName);
                cmd.Parameters.AddWithValue("@Qualification", request.Qualification);
                cmd.Parameters.AddWithValue("@ContactNumber", request.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", request.Email);
                cmd.Parameters.AddWithValue("@DepartmentId", request.DepartmentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Teacher added successfully." });
        }

        [HttpDelete("Teacher/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteTeacher", conn)) // Stored Procedure
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { message = "Teacher deleted successfully." });
        }

        [HttpGet("Teacher/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Teacher obj = new Teacher();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetTeacherById", conn)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.Id = Convert.ToInt32(reader["Id"]);
                        obj.FullName = reader["FullName"].ToString();
                        obj.Qualification = reader["Qualification"].ToString();
                        obj.ContactNumber = reader["ContactNumber"].ToString();
                        obj.Email = reader["Email"].ToString();
                        obj.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Teacher request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateTeacher", conn)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@FullName", request.FullName);
                cmd.Parameters.AddWithValue("@Qualification", request.Qualification);
                cmd.Parameters.AddWithValue("@ContactNumber", request.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", request.Email);
                cmd.Parameters.AddWithValue("@DepartmentId", request.DepartmentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Teacher updated successfully." });
        }
    }
}
