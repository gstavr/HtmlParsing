using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HtmlParserProgram
{
    class DataBase
    {

        public DataTable companiesDataTable = new DataTable();

        public DataBase()
        {
            this.companiesDataTable = GetUrlPage();
        }

        private static DataTable GetUrlPage()
        {
            //<!--<varValue>Data Source=ANL-PAPASTERGIO; Initial Catalog = essp; User Id = sa; Password = epsilonsa;</varValue>-->
            string connString = string.Format("Data Source=ANL-PAPASTERGIO; Initial Catalog = Odds; User Id = sa; Password = epsilonsa;");
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.ConnectionString = connString;
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

    }
}
