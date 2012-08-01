using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using InternationalTalentTest;
using DataAccess.Models;
using System.Data.SqlClient;

namespace DataAccess {
    internal class ITTDataAccess : DataAccessBase {

        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["ProductLib.Properties.Settings.ProductsConnectionString"].ConnectionString; } }

        public int InsertStudent(Student s) {
            return ExecuteNonQuery("InsertStudent", new SqlParameter[]{
                    CreateParameter("@firstName", SqlDbType.SmallDateTime, s.FirstName),
                    CreateParameter("@lastName", SqlDbType.SmallDateTime, s.LastName),
                    CreateParameter("@fatherName", SqlDbType.Int, s.FatherName),
                    CreateParameter("@DateOfBirth", SqlDbType.DateTime, s.DateOfBirth)
            }, CommandType.StoredProcedure);
        }
    }
}
