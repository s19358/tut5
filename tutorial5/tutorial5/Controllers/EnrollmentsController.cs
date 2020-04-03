using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tutorial5.Models;

namespace tutorial5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private string ConnString = "Data Source=db-mssql;Initial Catalog=s19358;Integrated Security=True";

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {

                //var index = $"s{new Random().Next(1, 20000)}";
                com.Connection = con;
                com.CommandText = "insert into Student values(@par1,@par2,@par3,@par4,1);";
                com.Parameters.AddWithValue("par1", student.IndexNumber);
                com.Parameters.AddWithValue("par2", student.FirstName);
                com.Parameters.AddWithValue("par3", student.LastName);
                com.Parameters.AddWithValue("par4", student.BirthDate);

                con.Open();
                int affected = com.ExecuteNonQuery();
                return Ok(affected);
            }
            return Ok();


        }
    }
}