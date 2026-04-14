using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class ResultController : Controller
    {
        private readonly IConfiguration _configuration;

        public ResultController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Result()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var results = new List<Result>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllResults", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new Result
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StudentRollNumber = reader["StudentRollNumber"].ToString(),
                            SubjectCode = reader["SubjectCode"].ToString(),
                            MarksObtained = Convert.ToDouble(reader["MarksObtained"]),
                            TotalMarks = Convert.ToDouble(reader["TotalMarks"]),
                            Grade = reader["Grade"].ToString()
                        });
                    }
                }
            }

            return Json(results);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Result request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertResult", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentRollNumber", request.StudentRollNumber);
                cmd.Parameters.AddWithValue("@SubjectCode", request.SubjectCode);
                cmd.Parameters.AddWithValue("@MarksObtained", request.MarksObtained);
                cmd.Parameters.AddWithValue("@TotalMarks", request.TotalMarks);
                cmd.Parameters.AddWithValue("@Grade", request.Grade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Result Added Successfully" });
        }

        [HttpDelete("Result/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteResult", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ResultId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Result Deleted Successfully" });
        }

        [HttpGet("Result/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Result obj = new Result();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetResultById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.Id = Convert.ToInt32(reader["Id"]);
                        obj.StudentRollNumber = reader["StudentRollNumber"].ToString();
                        obj.SubjectCode = reader["SubjectCode"].ToString();
                        obj.MarksObtained = Convert.ToDouble(reader["MarksObtained"]);
                        obj.TotalMarks = Convert.ToDouble(reader["TotalMarks"]);
                        obj.Grade = reader["Grade"].ToString();
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Result request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateResult", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@StudentRollNumber", request.StudentRollNumber);
                cmd.Parameters.AddWithValue("@SubjectCode", request.SubjectCode);
                cmd.Parameters.AddWithValue("@MarksObtained", request.MarksObtained);
                cmd.Parameters.AddWithValue("@TotalMarks", request.TotalMarks);
                cmd.Parameters.AddWithValue("@Grade", request.Grade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Result Updated Successfully" });
        }
    }
}
