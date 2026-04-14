using MaktabeGulabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MaktabeGulabProject.Controllers
{
    [Route("Fee")]
    public class FeeController : Controller
    {
        private readonly IConfiguration _configuration;

        public FeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Fee()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public JsonResult GetAll()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var fees = new List<Fee>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllFees", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fees.Add(new Fee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StudentRollNumber = reader["StudentRollNumber"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            Status = reader["Status"].ToString(),
                            DueDate = Convert.ToDateTime(reader["DueDate"]),
                            PaidDate = reader["PaidDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["PaidDate"])
                        });
                    }
                }
            }
            return Json(fees);
        }

        [HttpPost("Add")]
        public JsonResult Add([FromBody] Fee request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("InsertFee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentRollNumber", request.StudentRollNumber);
                cmd.Parameters.AddWithValue("@Amount", request.Amount);
                cmd.Parameters.AddWithValue("@Status", request.Status);
                cmd.Parameters.AddWithValue("@DueDate", request.DueDate);
                cmd.Parameters.AddWithValue("@PaidDate", (object?)request.PaidDate ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Fee record added successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteFee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Fee record deleted successfully." });
        }

        [HttpGet("GetById/{id}")]
        public JsonResult GetById(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            Fee obj = new Fee();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetFeeById", conn))
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
                        obj.Amount = Convert.ToDecimal(reader["Amount"]);
                        obj.Status = reader["Status"].ToString();
                        obj.DueDate = Convert.ToDateTime(reader["DueDate"]);
                        obj.PaidDate = reader["PaidDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["PaidDate"]);
                    }
                }
            }
            return Json(obj);
        }

        [HttpPut("Update")]
        public JsonResult Update([FromBody] Fee request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateFee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", request.Id);
                cmd.Parameters.AddWithValue("@StudentRollNumber", request.StudentRollNumber);
                cmd.Parameters.AddWithValue("@Amount", request.Amount);
                cmd.Parameters.AddWithValue("@Status", request.Status);
                cmd.Parameters.AddWithValue("@DueDate", request.DueDate);
                cmd.Parameters.AddWithValue("@PaidDate", (object?)request.PaidDate ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { message = "Fee record updated successfully." });
        }
    }
}
