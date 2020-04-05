using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using tutorial5.Models;

namespace tutorial5.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        private string ConnString = "Data Source=db-mssql;Initial Catalog=s19358;Integrated Security=True;MultipleActiveResultSets=True";
        public void EnrollStudent(Student student)
        {

        }

        public void PromoteStudent(Enrollment enrollment)
        {
         


            }
        }
    }

