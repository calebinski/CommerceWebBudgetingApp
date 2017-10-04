// Cameron L'Ecuyer
// Last Modified: 2/17/17

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace Tester
{
    public class DataAbstract
    {
        /*
         * 
         * Currently there is very little error checking
         * it would be advised to use try/catch blocks
         * when executing the code
         * 
         * 
         * 
         * 
         * 
         */
        private string connectionString;
        private SqlConnection connect;

        public DataAbstract()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["authenticateEntities"].ConnectionString;
            connect = new SqlConnection(connectionString);
        } // end default constructor

        // Supply the first name, last name, username, and password
        // username must be unique, no duplicates are allowed
        // Returns true if successfully added user
        public Boolean addUser(string firstName, string lastName, string username, string password)
        {
            if (firstName == "" || lastName == "" || username == "" || password == "")
            {
                return false;
            } // end if

            string insertSQL = "INSERT INTO Users (FirstName, LastName, Username, Password) VALUES ('";
            insertSQL += firstName + "', '" + lastName + "', '" + username + "', '" + password + "')";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            return true;
        } // end addUser

        // Supply the username and password
        // returns true if password matches what is in the database
        public int login(string username, string password)
        {
            string selectSQL = "SELECT UserID, Password FROM Users WHERE Username = '" + username + "'";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            SqlDataReader reader;

            connect.Open();
            reader = cmd.ExecuteReader();


            reader.Read();

            if (reader.HasRows && reader["Password"].ToString() == password)
            {

                int ID = (int)reader["UserID"];
                reader.Close();
                connect.Close();
                return ID;
            }
            else
            {
                reader.Close();
                connect.Close();
                return -1;
            } // end if        
        } // end login

        // Supply the name of the category
        // The name must be unique, no duplicates allowed
        // returns true if successful
        public Boolean addCategory(string name)
        {
            string insertSQL = "INSERT INTO Categories (Name) VALUES ('" + name + "')";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if (num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addCategory

        // Supply categoryID, userID, goal amount, and end Date
        // returns true if successful
        public Boolean addGoal(int categoryID, int userID, double totalAmt, DateTime endDate)
        {
            string insertSQL = "INSERT INTO Goals (CategoryID, UserID, CurrentAmt, TotalAmt, StartDate, EndDate, Completed) VALUES (";
            insertSQL += categoryID + ", " + userID + ", " + 0 + ", " + totalAmt + ", '" + DateTime.Now.ToString("yyyyMMdd") + "', '" + endDate.ToString("yyyyMMdd") + "', '" + 0 + "')";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if (num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addGoal

        // Supply the userID
        // returns a DataSet of all the Goals associated with the userID
        public DataSet returnGoals(int userID)
        {
            string selectSQL = "SELECT * FROM Goals WHERE UserID = " + userID;

            SqlCommand cmd = new SqlCommand(selectSQL, connect);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();

            adapter.Fill(temp);

            connect.Close();

            return temp;
        } // end returnGoals
    } // end class
} // end namespace