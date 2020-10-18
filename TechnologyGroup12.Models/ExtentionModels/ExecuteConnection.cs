﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TechnologyGroup12.Models.ExtentionModels
{
    public static class ExecuteConnection
    {
        // "Server=localhost\\SQLEXPRESS;Database=TechnologyGroup12DB;Trusted_Connection=True;MultipleActiveResultSets=true"
 

        public static SqlConnection Connect(string Server, string DatabaseTable, string UserName, string Password)
        {
            string connString = @"Data Source=" + Server + ";Initial Catalog="
                       + DatabaseTable + ";Integrated Security=True;User ID=" + UserName + ";Password=" + Password;

            return new SqlConnection(connString);
        }

        public static SqlConnection Connect(string Server, string databasename)
        {
            string connString = @"Data Source=" + Server + ";Initial Catalog="
                       + databasename + ";Integrated Security=True";
            return new SqlConnection(connString);
        }
        public static SqlConnection Connect(string Server, string Username, string password)
        {
            string connString = @"Data Source=" + Server + ";Integrated Security=True;User ID =" + Username + "; Password =" + password;
            return new SqlConnection(connString);
        }
    }
}
