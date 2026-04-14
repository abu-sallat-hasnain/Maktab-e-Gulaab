using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    public class SectionController : Controller
    {
        private readonly IConfiguration _configuration;

        public SectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Section()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var sections = new List<Section>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllSections", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sections.Add(new Section
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Shift = reader["Shift"].ToString(),
                            Strength = Convert.ToInt32(reader["Strength"]),
                            Floor = reader["Floor"].ToString(),
                            RoomNumber = reader["RoomNumber"].ToString()
                        });
                    }
                }
            }

            return Json(sections);
        }

        [HttpPost]
        public JsonResult Add([FromBody] Section request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertSection", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@Shift", request.Shift);
                cmd.Parameters.AddWithValue("@Strength", request.Strength);
                cmd.Parameters.AddWithValue("@Floor", request.Floor);
                cmd.Parameters.AddWithValue("@RoomNumber", request.RoomNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Section Added Successfully" });
        }

        [HttpDelete("Section/DeleteRow/{id}")]
        public JsonResult DeleteRow(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteSection", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Section deleted successfully." });
        }

        [HttpGet("Section/EditRow/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Section obj = new Section();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetSectionById", conn))
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
                        obj.Shift = reader["Shift"].ToString();
                        obj.Strength = Convert.ToInt32(reader["Strength"]);
                        obj.Floor = reader["Floor"].ToString();
                        obj.RoomNumber = reader["RoomNumber"].ToString();
                    }
                }
            }

            return Json(obj);
        }

        [HttpPut]
        public JsonResult Update([FromBody] Section request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateSection", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@Name", request.Name);
                cmd.Parameters.AddWithValue("@Shift", request.Shift);
                cmd.Parameters.AddWithValue("@Strength", request.Strength);
                cmd.Parameters.AddWithValue("@Floor", request.Floor);
                cmd.Parameters.AddWithValue("@RoomNumber", request.RoomNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Section updated successfully." });
        }
    }
}
