using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Department()
        {
            return View();
        }

      
        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllDepartments", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            Description = reader["Description"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
            }
            return Json(departments);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Department request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentName", request.DepartmentName);
                cmd.Parameters.AddWithValue("@DepartmentCode", request.DepartmentCode);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Department Added Successfully" });
        }

        [HttpDelete("Department/Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Department deleted successfully." });
        }

        [HttpGet("Department/GetById/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Department obj = new Department();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetDepartmentById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentId", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                        obj.DepartmentName = reader["DepartmentName"].ToString();
                        obj.DepartmentCode = reader["DepartmentCode"].ToString();
                        obj.Description = reader["Description"].ToString();
                        obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut("Department/Update")]
        public JsonResult Update([FromBody] Department request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentId", request.DepartmentId);
                cmd.Parameters.AddWithValue("@DepartmentName", request.DepartmentName);
                cmd.Parameters.AddWithValue("@DepartmentCode", request.DepartmentCode);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Department Updated Successfully" });
        }
    }
}
