using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HtmlParserProgram
{
    class DataBase
    {
        // Scaffold-DbContext "Server=DEV-STAVROU\SQLEXPRESS;Database=Odds;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
        //Scaffold-DbContext "Server=GINOS\SQLEXPRESS03;Database=Odds;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
        public DataTable companiesDataTable = new DataTable();
        //public string _connString = string.Format("Data Source=DEV-STAVROU\\SQLEXPRESS;Initial Catalog=Odds;Integrated Security=True;MultipleActiveResultSets=true");// string.Format("Data Source=ANL-PAPASTERGIO; Initial Catalog = Odds; User Id = sa; Password = epsilonsa;");
        public string _connString = string.Format("Data Source=GINOS\\SQLEXPRESS03;Initial Catalog=Odds;Integrated Security=True;MultipleActiveResultSets=true");// string.Format("Data Source=ANL-PAPASTERGIO; Initial Catalog = Odds; User Id = sa; Password = epsilonsa;");
        public DataBase()
        {
            //this._connString = this._connString;
            this.companiesDataTable = GetUrlPage();
        }

        private DataTable GetUrlPage()
        {
            //<!--<varValue>Data Source=ANL-PAPASTERGIO; Initial Catalog = essp; User Id = sa; Password = epsilonsa;</varValue>-->
            //string connString = _connString;//string.Format("Data Source=ANL-PAPASTERGIO; Initial Catalog = Odds; User Id = sa; Password = epsilonsa;");
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(this._connString))
            {
                con.ConnectionString = this._connString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM Companies";
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }

            return dt;
        }

        public int X_getGID(string tableName)
        {
            int _id = 0;
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                using (SqlConnection con = new SqlConnection(this._connString))
                {
                    con.ConnectionString = this._connString;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = string.Format(@"DECLARE @RC int, @tableName varchar(50)
                                                    SET @tableName = @tableNameParam;
                                                    -- TODO: Set parameter values here.
                                                    EXECUTE @RC = [dbo].[X_getGID] 
                                                       @tableName
                                                       select @RC");
                    cmd.Parameters.AddWithValue("@tableNameParam", tableName);
                    _id = (int)cmd.ExecuteScalar();
                    con.Close();
                }
            }

            return _id;
        }

    }
}
