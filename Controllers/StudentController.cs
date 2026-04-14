using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Student()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllStudents", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RollNumber = reader["RollNumber"].ToString(),
                            Name = reader["Name"].ToString(),
                            CNIC = reader["CNIC"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            ContactNumber = reader["ContactNumber"].ToString()
                        });
                    }
                }
            }

            return Json(students);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Student request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RollNumber", request.RollNumber);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@CNIC", request.CNIC);
                cmd.Parameters.AddWithValue("@DateOfBirth", request.DateOfBirth);
                cmd.Parameters.AddWithValue("@ContactNumber", request.ContactNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Student Added Successfully" });
        }

        [HttpDelete("Student/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Student Deleted Successfully" });
        }

        [HttpGet("Student/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Student obj = new Student();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetStudentById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.Id = Convert.ToInt32(reader["Id"]);
                        obj.RollNumber = reader["RollNumber"].ToString();
                        obj.Name = reader["Name"].ToString();
                        obj.CNIC = reader["CNIC"].ToString();
                        obj.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                        obj.ContactNumber = reader["ContactNumber"].ToString();
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Student request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@RollNumber", request.RollNumber);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@CNIC", request.CNIC);
                cmd.Parameters.AddWithValue("@DateOfBirth", request.DateOfBirth);
                cmd.Parameters.AddWithValue("@ContactNumber", request.ContactNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Student Updated Successfully" });
        }
    }
}
