using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SecretSantaMVC.Models
{
    /*
     * Static class used to interact with the database. Functions included are:
     * - void PostComment(string title, string author, string text) for posting a new comment to the DB
     * - void DeleteComment(int commentID) for deleting a comment from the DB
     * - string GetComments() returning a string representation of all comments
     */
    public static class AccessDB
    {

        /*
         * Method for posting reviews
         */
        public static void PostComment(string title, string author, string text)
        {
            
            string tableName = "Comments";
            DateTime datePosted = DateTime.Now;
            string datePostedString = datePosted.ToString("yyyy-MM-dd");
            string sqlString =
                $"insert into {tableName}" +
                "(commentTitle, commentDatePublished, commentAuthor, commentText)" +
                " values" +
                $"('{title}', '{datePostedString}', '{author}', '{text}')";

            AccessDB.ExecuteSQL(sqlString);
        }

        /*
         * Method for deleting a comment based on its ID
         */
        public static void DeleteComment(int commentID)
        {
            string tableName = "Comments";

            String sqlString =
                $"delete from {tableName}" +
                $"where commentID='{commentID}'";
            AccessDB.ExecuteSQL(sqlString);
        }

        /*
        * Method for executing deletion & posting commands (fetching is handled by GetComments() method)
        */
        public static bool ExecuteSQL(string sqlString)
        {
            string dataSource = "LAPTOP-1NDJHM8K";                                   // Name of server
            string initialCatalog = "EKMProjectDB";                                  // Name of database
            string userID = "sa";                                                    // Connection credentials
            string password = "weders104";
            // Connection info
            string connectionData = $"Data Source={dataSource};Initial Catalog={initialCatalog};User ID={userID};Password={password}";

            SqlConnection sqlConnection = new SqlConnection(connectionData);
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlString, sqlConnection);

                SqlDataAdapter sqlAdapter = new SqlDataAdapter();                     // Used to insert, update or delete commands
                sqlAdapter.InsertCommand = command;                                   // Associate the command with the adapter object
                sqlAdapter.InsertCommand.ExecuteNonQuery();                           // Execute command in the database

                command.Dispose();
                sqlConnection.Close();

                Console.WriteLine("Action Completed");
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine("SQL Error");
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    StringBuilder errorMessages = new StringBuilder(); // Error message StringBuilder taken from Microsoft docs
                    errorMessages.Append("Index #" + i + "\n" +        // https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlexception?view=dotnet-plat-ext-3.1
                        "Message: " + e.Errors[i].Message + "\n" +
                        "LineNumber: " + e.Errors[i].LineNumber + "\n" +
                        "Source: " + e.Errors[i].Source + "\n" +
                        "Procedure: " + e.Errors[i].Procedure + "\n");
                }
                return false;
            }
        }

        /*
        * Method for retrieving the contents of a table
        */
        public static List<List<string>> GetComments()
        {

            string dataSource = "LAPTOP-1NDJHM8K";
            string initialCatalog = "EKMProjectDB";
            string userID = "sa";
            string password = "weders104";

            string tableName = "Comments";

            // Connection info
            string connectionData = $"Data Source={dataSource};Initial Catalog={initialCatalog};User ID={userID};Password={password}";
            SqlConnection sqlConnection = new SqlConnection(connectionData);

            sqlConnection.Open();

            string sqlSelectAll = $"Select * from {tableName}";                    // Our SQL statement

            SqlCommand command = new SqlCommand(sqlSelectAll, sqlConnection);      // SqlCommand performs read & write operations

            SqlDataReader dataReader = command.ExecuteReader();                    // Read row in table one-by-one

            List<List<string>> allRows = new List<List<string>>();            // StringBuilder for efficiently constructing a string

            while (dataReader.Read())                                              // Read() method goes through database row by row, and returns a boolean
            {

                Object[] columnValues = new object[dataReader.FieldCount];         // columnValues stores the content of all columns for a given row

                int fieldCount = dataReader.GetValues(columnValues);               // fieldCount is the number of fields in a given row

                List<string> thisRow = new List<string>();
                for (int i = 0; i < fieldCount; i++)                               // Iterate through the columns and append to the StringBuilder 
                {
                    Console.WriteLine("current item: " + columnValues[i].ToString());
                    thisRow.Append(columnValues[i].ToString());
                }
               
                allRows.Append(thisRow);
            }
            
            dataReader.Close();                                                    // Cleanup
            command.Dispose();
            sqlConnection.Close();

            return allRows;
        }
    }
}
