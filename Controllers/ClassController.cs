using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class ClassController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public ClassController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Class()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var classes = new List<Class>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("classget", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(new Class
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Code = reader["Code"].ToString(),
                            TotalStudents = Convert.ToInt32(reader["TotalStudents"]),
                            Medium = reader["Medium"].ToString(),
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            return Json(classes);

        }
        [HttpPost]
        public JsonResult Add([FromBody] Class request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertClass", conn))

            {
                cmd.CommandType = CommandType.StoredProcedure;

                
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@Code", request.Code);
                cmd.Parameters.AddWithValue("@TotalStudents", request.TotalStudents);
                cmd.Parameters.AddWithValue("@Medium", request.Medium);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();

            }
            
            return Json(new
            {
                message = "Class Added Successfully"
            });
        }
        [HttpDelete("Class/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteClass", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { message = "Class delete successfully." });
        }
        [HttpGet("Class/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Class obj = new Class();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("getbyid", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            obj.Id = Convert.ToInt32(reader["Id"]);
                            obj.Name = reader["Name"].ToString();
                            obj.Code = reader["Code"].ToString();
                            obj.TotalStudents = Convert.ToInt32(reader["TotalStudents"]);
                            obj.Medium = reader["Medium"].ToString();   
                            obj.Description = reader["Description"].ToString();
                        }
                    }
                }
            }

            return Json(obj);  
        }
        [HttpPut]
        public JsonResult Update([FromBody] Class request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateClass", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@Code", request.Code);
                cmd.Parameters.AddWithValue("@TotalStudents", request.TotalStudents);
                cmd.Parameters.AddWithValue("@Medium", request.Medium);
                cmd.Parameters.AddWithValue("@Description", request.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Class updated successfully." });
        }
       

    }
}
