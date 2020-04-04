using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tutorial5.DTOs.Requests;
using tutorial5.Models;

namespace tutorial5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private string ConnString = "Data Source=db-mssql;Initial Catalog=s19358;Integrated Security=True;MultipleActiveResultSets=True";
        
        [HttpPost]
        public IActionResult EnrollStudent(Student student)
        {


            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            { 
                com.Connection = con;
                com.CommandText = "Select * From Studies Where Name=@Name;";
                com.Parameters.AddWithValue("Name", student.studies);
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                com.Transaction = transaction;
    

                int idStudies ,idEnrollment;

                var dr = com.ExecuteReader();  //sending request to the databse
                if (!dr.Read()) // check whether the studies exists or not
                {

                    transaction.Rollback();
                    return BadRequest("Studies doesnt exists!");
                }
                else
                {
                    idStudies = (int)dr["idStudy"];
                }
                dr.Close();

                com.CommandText = "Select Max(StartDate) From enrollment where semester =1 and idStudy=@idStudies;";
                com.Parameters.AddWithValue("idStudies", idStudies);
                dr = com.ExecuteReader();
               
                if (dr.Read())
                {

                    dr.Close();
                    com.CommandText = "SELECT CONVERT(VARCHAR(10), getdate(), 111) 'Date';";
                    dr = com.ExecuteReader();
                    dr.Read();

                    DateTime date =DateTime.Parse(dr["Date"].ToString());

                    dr.Close();

                    com.CommandText = "Select MAX(IdEnrollment) 'maxid' From Enrollment;";
                    dr = com.ExecuteReader();
                    dr.Read();
                    idEnrollment = (int)dr["maxid"] +1;

                    dr.Close();

                    com.CommandText = "Insert into Enrollment values (@idEnrollment,1,"+idStudies+",'"+date+"');";
                    com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    com.ExecuteNonQuery();

                    dr.Close();

                    com.CommandText = "Select MAX(IdEnrollment) 'maxidEnroll' From Enrollment;";
                    dr = com.ExecuteReader();
                    dr.Read();
                    idEnrollment = (int)dr["maxidEnroll"];
                    dr.Close();
                    
                }
                else
                {                  
                    return BadRequest("ERRORRR");
                }
                dr.Close();
                com.CommandText = "Select * from Student where IndexNumber= @indexnum;";
                com.Parameters.AddWithValue("indexnum", student.IndexNumber);
                dr = com.ExecuteReader();
                if (!dr.Read()) 
                {

                    dr.Close();
                    com.CommandText = "insert into Student values (@par1,@par2,@par3,@par4,@idEnrollment);";
                    com.Parameters.AddWithValue("par1", student.IndexNumber);
                    com.Parameters.AddWithValue("par2", student.FirstName);
                    com.Parameters.AddWithValue("par3", student.LastName);
                    com.Parameters.AddWithValue("par4", student.BirthDate);
                    int affected = com.ExecuteNonQuery();
                    
                }
                else
                {
                   transaction.Rollback();
                    return BadRequest("There is a student with this number :"+ student.IndexNumber);
                }

       

                transaction.Commit();

            }

            return Created("http://localhost:50730/api/enrollments", student);


        }
    }
}