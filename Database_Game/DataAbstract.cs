// Created By: Cameron L'Ecuyer
// Last Modified: 2/26/17

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
        private const int DATA_ERROR_VALUE = -1;
        private string connectionString;
        private SqlConnection connect;

        public DataAbstract()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CommBank"].ConnectionString;
            connect = new SqlConnection(connectionString);
        } // end default constructor

        // Supply the first name, last name, username, and password
        // username must be unique, no duplicates are allowed
        // Returns number of rows added, or returns error value if username already exists
        // Throws exception if parameters are empty or null
        public int addUser(string firstName, string lastName, string username, string password)
        {
            // checks values for valid data
            if(firstName == "" || firstName == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "First Name");
            } // end if

            if (lastName == "" || lastName == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Last Name");
            } // end if

            if (username == "" || username == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Username");
            } // end if

            if (password == "" || password == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Password");
            } // end if

            // calls data input function
            return addUserChecked(firstName, lastName, username, password);
        } // end addUser

        // Supply the username and password
        // returns the user id or an error value
        // throws excpetions if paramters are empty or null
        public int login(string username, string password)
        {
            // checks values for valid data
            if (username == "" || username == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Username");
            } // end if

            if (password == "" || password == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Password");
            } // end if

            return loginChecked(username, password);
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
        public Boolean addGoal(int acctNumber, string name, double totalAmt, DateTime endDate, string transferType, int transferAmt)
        {
            string insertSQL = "INSERT INTO Goals (AcctNumber, Name, CurrentAmt, TotalAmt, StartDate, EndDate, Completed, TransferType, TransferAmt) VALUES (";
            insertSQL += acctNumber + ", '" + name +", " + 0 + "', " + totalAmt + ", '" + DateTime.Now.ToString("yyyyMMdd") + "', '" + endDate.ToString("yyyyMMdd") + "', '"+ 0 + "', '"
                + transferType + "'," + transferAmt + ")";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if(num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addGoal

        // Supply the userID
        // returns a DataSet of all the Goals associated with the userID
        public DataSet returnGoals(int acctNumber)
        {
            string selectSQL = "SELECT * FROM Goals WHERE AcctNumber = " + acctNumber;

            SqlCommand cmd = new SqlCommand(selectSQL, connect);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();

            adapter.Fill(temp);

            connect.Close();

            return temp;
        } // end returnGoals

        // Inserts data from parameters into sql statements to add a user to the database
        // fistName, lastName, username, and password are required to input a user
        // returns the number of rows affected, or returns an error code if the username is taken
        private int addUserChecked(string firstName, string lastName, string username, string password)
        {
            // builds the select sql string
            string selectSQL = "SELECT * FROM Users WHERE Username = @Username";

            SqlCommand selectCmd = new SqlCommand(selectSQL, connect);
            SqlDataReader reader;

            // adds the passed in username to the sql string
            selectCmd.Parameters.AddWithValue("@Username", username);

            connect.Open();
            reader = selectCmd.ExecuteReader();

            // if the reader returns rows return error value
            if (reader.HasRows)
            {
                connect.Close();
                return DATA_ERROR_VALUE;
            } // end if

            connect.Close();

            // builds the insert sql string
            string insertSQL = "INSERT INTO Users (FirstName, LastName, Username, Password) VALUES (";
            insertSQL += "@FirstName, @LastName, @Username, @Password)";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);

            // adds the passed in values to the sql string
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            // holds the number of rows added
            int added = 0;

            // attempts to add the row to the database
            connect.Open();
            added = cmd.ExecuteNonQuery();
            connect.Close();

            return added;
        } // end addUserChecked

        // Queries the database to see if username and password match
        // if username does not exist, or password is wrong returns error value
        private int loginChecked(string username, string password)
        {
            // builds the select sql string
            string selectSQL = "SELECT UserID, Password FROM Users WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            SqlDataReader reader;

            // adds the passed in username to the sql string
            cmd.Parameters.AddWithValue("@Username", username);

            connect.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                if (reader["Password"].ToString() == password)
                {
                    int id = (int)reader["UserID"];
                    reader.Close();
                    connect.Close();
                    return id;
                } // end if
            } // end if      

            reader.Close();
            connect.Close();
            return DATA_ERROR_VALUE;       
        } // end loginChecked
    } // end class
} // end namespace