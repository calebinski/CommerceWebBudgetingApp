using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace Tester
{
    public class DataAbstract
    {
        /* TODO:
         * need checkID function -- check to see if the id exists
         * need updateGoalDescription
         */

        private const int DATA_ERROR_VALUE = -1;
        private string connectionString;
        private SqlConnection connect;

        // Sets up the connection string
        public DataAbstract()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["authenticateEntities"].ConnectionString;
            connect = new SqlConnection(connectionString);
        } // end default constructor

        // Adds a user
        // Username must be unique, no duplicates are allowed
        // Returns number of rows added, or returns error value if username already exists
        // Throws exception if parameters are empty or null
        public int addUser(string firstName, string lastName, string username, string password)
        {
            // checks values for valid data
            if (firstName == "" || firstName == null)
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

        // Inserts data from parameters into sql statements to add a user to the database
        // fistName, lastName, username, and password are required to input a user
        // Returns the number of rows affected, or returns an error code if the username is taken
        private int addUserChecked(string firstName, string lastName, string username, string password)
        {
            // builds the select sql string
            string selectSQL = "SELECT * FROM Users WHERE Username = @Username";

            SqlCommand selectCmd = new SqlCommand(selectSQL, connect);
            selectCmd.Parameters.AddWithValue("@Username", username);

            SqlDataReader reader;
            connect.Open();

            reader = selectCmd.ExecuteReader();

            if (reader.HasRows)
            {
                connect.Close();
                return DATA_ERROR_VALUE;
            } // end if

            connect.Close();

            string insertSQL = "INSERT INTO Users (FirstName, LastName, Username, Password) VALUES ("
                + "@FirstName, @LastName, @Username, @Password)";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            // holds the number of rows added
            int added = 0;

            connect.Open();
            added = cmd.ExecuteNonQuery();
            connect.Close();

            return added;
        } // end addUserChecked

        // Adds an account
        // Returns number of rows added
        // Throws exception if parameters are below minimum value
        public int addAccount(int userID, string acctType, long acctNumber, double balance)
        {
            // checks values for valid data
            if (userID < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserID");
            } // end if

            if (acctType == "" || acctType == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "AcctType");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            if (balance < 0)
            {
                throw new System.ArgumentException("Invalid range", "Balance");
            } // end if

            string insertSQL = "INSERT INTO Accounts (AcctNumber, UserID, Balance, AccountType) VALUES ("
                + "@acctNumber, @userID, @balance, @acctType)";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@balance", balance);
            cmd.Parameters.AddWithValue("@acctType", acctType);

            // holds rows added
            int added = 0;

            connect.Open();
            added = cmd.ExecuteNonQuery();
            connect.Close();

            return added;
        } //end addAccount

        // Adds a category
        // Returns positive number if successful, or error value if the category exists
        // Throws excpetions if paramter is empty or null
        public int addCategory(string name, int userID=-1)
        {
            string selectSQL = "SELECT * FROM Categories WHERE Name = @name";
            if (userID != -1) selectSQL += " AND UserID = @uid";
            SqlCommand selectCmd = new SqlCommand(selectSQL, connect);
            SqlDataReader reader;

            selectCmd.Parameters.AddWithValue("@name", name);
            selectCmd.Parameters.AddWithValue("@uid", userID);

            connect.Open();
            reader = selectCmd.ExecuteReader();

            if (reader.HasRows)
            {
                connect.Close();
                return DATA_ERROR_VALUE;
            } // end if

            connect.Close();

            string insertSQL = "INSERT INTO Categories (Name";
            if (userID != -1) insertSQL += ", UserID";
            insertSQL += ") Values (@name";
            if (userID != -1) insertSQL += ", @uid";
            insertSQL += ")";
            
            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@uid", userID);
            // holds rows added
            int added = 0;

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return added;
        } //end addCategory

        // Adds a goal associated with an account
        // Returns true if successful
        // Throws excpetions if some values are not valid
        public Boolean addGoal(long acctNumber, string name, double totalAmt, DateTime endDate, string transferType, double transferAmt, string description)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            if (name == "" || name == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Name");
            } // end if

            if (totalAmt < 0)
            {
                throw new System.ArgumentException("Invalid range", "TotalAmt");
            } // end if

            if (transferType == "" || transferType == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "TransferType");
            } // end if

            if (transferAmt < 0)
            {
                throw new System.ArgumentException("Invalid range", "TransferAmt");
            } // end if

            string insertSQL = "INSERT INTO Goals (AcctNumber, Name, CurrentAmt, TotalAmt, StartDate, EndDate, Completed, TransferType, TransferAmt, Description) VALUES ("
                + "@acctNumber, @name, " + 0 + ", @totalAmt, '" + DateTime.Now.ToString("yyyyMMdd") + "', @endDate, '" + 0 + "', "
                + "@transferType, @transferAmt, @description)";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@totalAmt", totalAmt);
            cmd.Parameters.AddWithValue("@endDate", endDate.ToString("yyyyMMdd"));
            cmd.Parameters.AddWithValue("@transferType", transferType);
            cmd.Parameters.AddWithValue("@transferAmt", transferAmt);
            cmd.Parameters.AddWithValue("@description", description);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if (num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addGoal

        // Adds a budget associated with an account
        // Returns true if successful
        // Throws exception if some values are not valid
        public Boolean addBudget(long acctNumber, int categoryID, DateTime startDate, DateTime endDate, double maxAmt, int favorite)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            if (maxAmt < 0)
            {
                throw new System.ArgumentException("Invalid range", "MaxAmt");
            } // end if

            string insertSQL = "INSERT INTO Budgets (AcctNumber, CategoryID,  StartDate, EndDate, CurrentAmt, MaxAmt, Favorite, Completed, Failed) VALUES ("
                + "@acctNumber, @categoryID, @startDate, @endDate, 0, @maxAmt, @favorite," + 0 + ", " + 0 + ")";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@maxAmt", maxAmt);
            cmd.Parameters.AddWithValue("@favorite", favorite);

            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if (num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addBudget

        // Adds a transaction
        // Returns an int > 0 if successful, 0 if not successful
        // Throws exception if acctNumber, amount, categoryID is below the minimum range
        public int addTransaction(long acctNumber, string transType, double amount, string description, int categoryID)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            // checks values for valid data
            if (amount < 0)
            {
                throw new System.ArgumentException("Invalid range", "Amount");
            } // end if

            // checks values for valid data
            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            string insertSQL = "INSERT INTO Transactions (AcctNumber, CategoryID, Deposit, Description, TransDate, TransType) VALUES ("
                + "@acctNumber, @categoryID, @amount, @description, @transDate, @transType)";

            SqlCommand cmd = new SqlCommand(insertSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@transDate", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@transType", transType);

            // holds rows added
            int added = 0;

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            if (transType == "CR") //if this is a deposit, much update goals
            {
                //This is commented out because the project lacks required components to 
                //ensure goal additions don't exceed amount deposited.
                //DepositAddFunds(acctNumber, amount);
            }

            return added;
        } //end addTransaction

        //This function is called by addTransaction function
        //When a deposit is made, this function will update goals 
        //depending on how they are meant to be progressed (percent or flatrate)
        //CURRENT ERROR: There is no ensuring that the user's total increase to goals
        //               doesn't exceed the deposit. This would need additions to the 
        //               project before use in production.
        private void DepositAddFunds(long accountNum , double amount)
        {
            DataTable goals = returnIncompleteGoals(accountNum).Tables[0];
            for (int i = 0; i < goals.Rows.Count; ++i) //iterate through each goal
            {
                DataRow DR = goals.Rows[i];
                string transferType = Convert.ToString(DR.Field<object>("TransferType"));
                bool percentDeposit = (transferType == "Percentage");
                bool flatRateDeposit = (transferType == "FlatRate");
                if (!percentDeposit && !flatRateDeposit) continue;//if not percent or flatrate, move on to next
                int goalID = Convert.ToInt32(DR.Field<object>("GoalID"));
                //amount to add is percent 
                
                //Series of if statements so additional plans are easily implimented in future
                if (percentDeposit)
                {
                    double percent = Convert.ToDouble(DR.Field<object>("TransferAmt")) / 100;//decimal
                    double toAdd = amount * percent;
                    updateGoalCurrentAmount(goalID, toAdd, accountNum);
                }
                if (flatRateDeposit)
                {
                    double toAdd = amount ;
                    updateGoalCurrentAmount(goalID, toAdd, accountNum);
                }
            }
        }

        // Logins a user if username and password are valid
        // Returns the user id or an error value
        // Throws excpetions if paramters are empty or null
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

        // Queries the database to see if username and password match
        // If username does not exist, or password is wrong returns error value
        private int loginChecked(string username, string password)
        {
            string selectSQL = "SELECT UserID, Password FROM Users WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@Username", username);

            SqlDataReader reader;
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

        // Retrieves user information
        // Returns a DataSet of the user information
        // Throws exception if userNumber is below the minimum range
        public DataSet returnUser(int userNumber)
        {
            // checks values for valid data
            if (userNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserNumber");
            } // end if

            string selectSQL = "SELECT FirstName, LastName, Email, GoalsNotifications, BudgetsNotifications, EmailNotifications, TextNotifications FROM Users WHERE UserID = @userNum";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@userNum", userNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnUser

        public DataSet returnAllUserIDs()
        {
            string selectSQL = "SELECT UserID FROM Users";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);

            DataSet temp = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        }

        // Retrieves all goals
        // Returns a DataSet of all the goals associated with the account
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnGoals(long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Goals WHERE AcctNumber = @acctNumber ORDER BY GoalID DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnGoals

        // Retrieves a speicified goal
        // Returns a DataSet of the goal associated with the ID
        // Throws exception if goalID is below the minimum range
        public DataSet returnOneGoal(int goalID)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            string selectSQL = "SELECT * FROM Goals WHERE GoalID = @goalID";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@goalID", goalID);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnOneGoal

        // Retrieves a speicified budget
        // Returns a DataSet of the budget associated with the ID
        // Throws exception if budgetID is below the minimum range
        public DataSet returnOneBudget(int budgetID)
        {
            // checks values for valid data
            if (budgetID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            string selectSQL = "SELECT * FROM Budgets WHERE BudgetID = @budgetID";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@budgetID", budgetID);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnOneBudget

        // Retrieves all incomplete goals
        // Returns a DataSet of all the incomplete goals associated with the account number
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnIncompleteGoals(long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Goals WHERE Completed = 0 AND AcctNumber = @acctNumber ORDER BY GoalID DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnIncompleteGoals

        // Retrieves all complete goals
        // Returns a DataSet of all the complete goals associated with the account number
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnCompleteGoals(long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Goals WHERE Completed = 1 AND AcctNumber = @acctNumber ORDER BY GoalID DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnCompleteGoals

        // Retrieves a count of completed goals for a given user
        // Returns an integer number of goals completed that are associated with the userID number
        // Throws exception if userID is below the minimum range
        public int returnCompleteGoalsCount(int userID)
        {
            // checks values for valid data
            if (userID < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserID");
            } // end if

            int goalsCount = 0;
            string selectSQL = "SELECT COUNT(Completed) AS Total FROM Goals JOIN Accounts ON Goals.AcctNumber = Accounts.AcctNumber WHERE Completed = 1 AND UserID = @userID";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@userID", userID);

            SqlDataReader rdr;
            connect.Open();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            goalsCount = Convert.ToInt32(rdr["Total"]);

            connect.Close();

            return goalsCount;
        } // end returnCompleteGoalsCount

        // Retrieves all budgets
        // Returns a DataSet of all budgets that are associated with the account number
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnBudgets(long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Budgets JOIN Categories ON Budgets.CategoryID = Categories.CategoryID WHERE AcctNumber = @acctNumber ORDER BY BudgetID DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnBudgets

        // Retrieves all budgets within a given date range
        // Returns a DataSet of all budgets that are associated with the account number
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnBoundedBudgets(long acctNumber, DateTime d1, DateTime d2)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Budgets JOIN Categories ON Budgets.CategoryID = Categories.CategoryID WHERE AcctNumber = @acctNumber AND StartDate >= @startDate";
            selectSQL += " AND EndDate <= @endDate ORDER BY BudgetID DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@startDate", d1);
            cmd.Parameters.AddWithValue("@endDate", d2);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnBoundedBudgets

        // Retrieves all categories
        // Returns a DataSet of all categories that are associated with the user
        // Throws exception if userID is below the minimum range
        public DataSet returnCategories(int userID)
        {
            // checks values for valid data
            if (userID < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserID");
            } // end if

            string selectSQL = "SELECT * FROM Categories WHERE (UserID = @userID OR UserID IS NULL)";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@userID", userID);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnCategories

        // Retrieves favorites based on the type
        // Returns a DataSet of all favorites that are associated with the account
        // Throws exception if acctNumber is below the minimum range, or favType is empty or null
        public DataSet returnFavorites(string favType, long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            if (favType == "" || favType == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "FavType");
            } // end if

            DataSet temp = new DataSet();

            if (favType == "Budget")
            {
                string selectSQL = "SELECT * FROM Budgets JOIN Categories ON Budgets.CategoryID = Categories.CategoryID WHERE AcctNumber = @acctNumber AND Favorite = " + 1;

                SqlCommand cmd = new SqlCommand(selectSQL, connect);
                cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                connect.Open();
                adapter.Fill(temp);
                connect.Close();
            } // end if

            if (favType == "Goal")
            {
                string selectSQL = "SELECT * FROM Goals WHERE AcctNumber = @acctNumber AND Favorite = " + 1;

                SqlCommand cmd = new SqlCommand(selectSQL, connect);
                cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                connect.Open();
                adapter.Fill(temp);
                connect.Close();
            } // end if
            return temp; 
        } // end returnFavorites

        // Retrieves accounts
        // Returns a DataSet of all accounts that are associated with the userID
        // Throws exception if userID is below the minimum range
        public DataSet returnAccounts(int userID)
        {
            // checks values for valid data
            if (userID < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserID");
            } // end if

            string selectSQL = "SELECT * FROM Accounts WHERE UserID = @userID";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@userID", userID);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } // end returnAccounts


        // Retrieves transactions
        // Returns a DataSet of all transactions that are associated with the account
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnTransactions(long acctNumber)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT TransactionID, Description, Deposit, Name, TransDate, TransType FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                + "WHERE AcctNumber = @acctNumber ORDER BY TransDate DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } //end returnTransactions 

        // Retrieves transactions
        // Returns a DataSet of all transactions that are associated with the account, and
        // Bounded by dates and a category (if category isn't needed, use 0) 
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnBoundTransactions(long acctNumber, DateTime startDate, DateTime endDate, int category, int sort)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            DataSet temp = new DataSet();
            string ordering = "ASC";

            if (sort < 0)
            {
                ordering = "DESC";
            } // end if

            if (category != 0)
            {
                string selectSQL = "SELECT * FROM Transactions WHERE AcctNumber = @acctNumber AND TransDate >= @startDate AND TransDate <= @endDate "
                    + "AND CategoryID = @category ORDER BY TransDate " + ordering;

                SqlCommand cmd = new SqlCommand(selectSQL, connect);
                cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@category", category);
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                connect.Open();
                adapter.Fill(temp);
                connect.Close();
            }
            else
            {
                string selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber AND TransDate >= @startDate AND TransDate <= @endDate ORDER BY TransDate " + ordering;

                SqlCommand cmd = new SqlCommand(selectSQL, connect);
                cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                             
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                connect.Open();
                adapter.Fill(temp);
                connect.Close();
            } // end if
            
            return temp;
        } //end returnBoundTransactions

        // Retrieves transactions filtered by a search term
        // Returns a DataSet of all transactions that are associated with the account, and
        // Bounded by dates and that have the search term
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnTransactionsSearch(long acctNumber, DateTime startDate, DateTime endDate, string desc)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                + "WHERE AcctNumber = @acctNumber AND TransDate >= @startDate AND TransDate <= @endDate "
                + "AND Description LIKE '%' + @description + '%' " + "ORDER BY TransDate DESC";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@description", desc);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } //end returnTransactionsSearch

        // Retrieves transactions sorted by categories
        // Returns a DataSet of all transactions that are associated with the account, and
        // Sorts them by the specified column 0 for Desc, 1 for Deposit, 2 for catName, 3 for TransDate, 4 for TransType
        // If order is true sorts in DSC if order is false sorts in ASC
        // Throws exception if acctNumber is below the minimum range
        public DataSet returnTransactionsCategorySort(long acctNumber, int sort, Boolean order)
        {
            // checks values for valid data
            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "";
            string ordering = "ASC";

            if (order)
            {
                ordering = "DESC";
            } // end if

            if (sort == 0)
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber ORDER BY Description " + ordering;
            }
            else if (sort == 1)
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber ORDER BY Deposit " + ordering;
            }
            else if (sort == 2)
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber ORDER BY Name " + ordering;
            }
            else if (sort == 3)
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber ORDER BY TransDate " + ordering;
            }
            else if (sort == 4)
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber ORDER BY TransType " + ordering;
            }
            else
            {
                selectSQL = "SELECT * FROM Transactions JOIN Categories ON Transactions.CategoryID = Categories.CategoryID "
                    + "WHERE AcctNumber = @acctNumber " + ordering;
            } // end if

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            DataSet temp = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            connect.Open();
            adapter.Fill(temp);
            connect.Close();

            return temp;
        } //end returnTransactionsSearch

        // Retrieves the total for transactions filtered by a categoryID
        // Returns a double of the sum of transactions that are associated with the account, and
        // Have the categoryID
        // Throws exception if acctNumber or categoryID is below the minimum range
        public double returnTransactionCategoryTotals(int categoryID, long acctNumber)
        {
            // checks values for valid data
            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT SUM(Deposit) as Total FROM Transactions WHERE CategoryID = @categoryID AND AcctNumber = @acctNumber";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            double total = 0.0;

            SqlDataReader rdr;

            connect.Open();
            rdr = cmd.ExecuteReader();
            rdr.Read();

            total = Double.Parse(rdr["Total"].ToString(), System.Globalization.NumberStyles.Currency);

            connect.Close();

            return total;
        } // end returnTransactionCategoryTotals

        // Retrieves the total for transactions filtered by a categoryID
        // Returns a double of the sum of all transactions that are associated with the account, and
        // Have the categoryID and are bound by the dates
        // Throws exception if acctNumber or categoryID is below the minimum range
        public double returnTransactionCategoryBoundTotals(int categoryID, long acctNumber, DateTime startDate, DateTime endDate)
        {
            // checks values for valid data
            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string selectSQL = "SELECT SUM(Deposit) as Total FROM Transactions WHERE CategoryID = @categoryID AND AcctNumber = @acctNumber"
                + " AND TransDate >= @startDate AND TransDate <= @endDate AND TransType = 'DR'";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            double total = 0.0;

            SqlDataReader rdr;

            connect.Open();
            rdr = cmd.ExecuteReader();
            rdr.Read();

            if (rdr["Total"].ToString() != "")
            {
                total = Double.Parse(rdr["Total"].ToString()); //, System.Globalization.NumberStyles.Currency);
            } // end if

            connect.Close();

            return total;
        } // end returnTransactionCategoryBoundTotals

        // Retrieves the categoryID of a category
        // Returns the categoryID
        // Throws exception if name is empty or null
        public int returnCategoryID(string name, int userID)
        {
            // checks values for valid data
            if (name == "" || name == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Name");
            } // end if

            int id = 0;
            string selectSQL = "SELECT CategoryID FROM Categories WHERE Name = @name AND (UserID = @userID OR UserID IS NULL)";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@userID", userID);

            SqlDataReader reader;

            connect.Open(); 
            reader = cmd.ExecuteReader();
            reader.Read();
            id = (int)reader["CategoryID"];
            connect.Close();

            return id;
        } // end returnCategoryID

        // Retrieves the category name
        // Returns the category name
        // Throws exception if categoryID is below the minimum range
        public string returnCategoryName(int categoryID)
        {
            // checks values for valid data
            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            string name;
            string selectSQL = "SELECT Name FROM Categories WHERE CategoryID = " + categoryID;

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);

            SqlDataReader reader;

            connect.Open();
            reader = cmd.ExecuteReader();
            reader.Read();
            name = reader["Name"].ToString();
            connect.Close();

            return name;
        } // end returnCategoryName

        // Updates the goalAmount
        // Throws exception if goalID, addAmt, or acctNumber is below the minimum range
        public void updateGoalAmt(int goalID, double addAmt, long acctNumber)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if (addAmt < 0)
            {
                throw new System.ArgumentException("Invalid range", "AddAmt");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            double currentAmt = 0.0;

            string selectSQL = "SELECT CurrentAmt FROM Goals WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand cmd = new SqlCommand(selectSQL, connect);
            cmd.Parameters.AddWithValue("@goalID", goalID);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            SqlDataReader rdr;
            connect.Open();
            rdr = cmd.ExecuteReader();
            rdr.Read();

            if (rdr["CurrentAmt"].ToString() != "")
            {
                currentAmt = Double.Parse(rdr["CurrentAmt"].ToString(), System.Globalization.NumberStyles.Currency);
            } //end if

            currentAmt += addAmt;
            rdr.Close();
            connect.Close();

            //update the goal with the new value
            string updateSQL = "UPDATE Goals SET CurrentAmt = @currentAmt WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@currentAmt", currentAmt);
            Cmd.Parameters.AddWithValue("@goalID", goalID);
            Cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return ;
        } // end updateGoalAmt

        // Updates the goal endDate
        // Throws exception if goalID or acctNumber is below the minimum range
        public void updateGoalEndDate(int goalID, DateTime date, long acctNumber)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string updateSQL = "UPDATE Goals SET EndDate = @EndDate WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@EndDate", date.ToString("yyyyMMdd"));
            Cmd.Parameters.AddWithValue("@goalID", goalID);
            Cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateGoalEndDate

        // Updates the goal totalAmount
        // Throws exception if goalID, newTotal, or acctNumber is below the minimum range
        public void updateGoalTotalAmt(int goalID, double newTotal, long acctNumber)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if (newTotal < 0)
            {
                throw new System.ArgumentException("Invalid range", "NewTotal");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string updateSQL = "UPDATE Goals SET TotalAmt = @newTotal WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@newTotal", newTotal);
            Cmd.Parameters.AddWithValue("@goalID", goalID);
            Cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateGoalTotalAmt

        // Updates the goal name
        // Throws exception if goalID or acctNumber is below the minimum range, or name is empty or null
        public void updateGoalName(int goalID, string name, long acctNumber)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if (name == "" || name == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "Name");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string updateSQL = "UPDATE Goals SET Name = @name WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@name", name);
            Cmd.Parameters.AddWithValue("@goalID", goalID);
            Cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateGoalName

        // Updates the goal currentAmt
        // Throws exception if goalID, newCurrent, or acctNumber is below the minimum range
        public void updateGoalCurrentAmount(int goalID, double newCurrent, long acctNumber)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if (newCurrent < 0)
            {
                throw new System.ArgumentException("Invalid range", "NewCurrent");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if

            string updateSQL = "UPDATE Goals SET CurrentAmt = @newCurrent WHERE GoalID = @goalID AND AcctNumber = @acctNumber";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@newCurrent", newCurrent);
            Cmd.Parameters.AddWithValue("@goalID", goalID);
            Cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateGoalCurrentAmount

        // Updates the goal favorite status
        // Throws exception if goalID is below the minimum range
        public void updateGoalFavorite(int goalID, bool favorite)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            string updateSQL = "UPDATE Goals SET Favorite = @favorite WHERE GoalID = @goalID";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@favorite", favorite);
            Cmd.Parameters.AddWithValue("@goalID", goalID);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateGoalFavorite

        // Updates the budget favorite status
        // Throws exception if budgetID is below the minimum range
        public void updateBudgetFavorite(int budgetID, bool favorite)
        {
            // checks values for valid data
            if (budgetID < 0)
            {
                throw new System.ArgumentException("Invalid range", "BudgetID");
            } // end if

            string updateSQL = "UPDATE Budgets SET Favorite = @favorite WHERE BudgetID = @budgetID";

            SqlCommand Cmd = new SqlCommand(updateSQL, connect);
            Cmd.Parameters.AddWithValue("@favorite", favorite);
            Cmd.Parameters.AddWithValue("@budgetID", budgetID);

            connect.Open();
            Cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateBudgetFavorite

        // Updates the transaction's category
        // Throws exception if transactionID is below the minimum range
        public void updateTransactionCategory(int transactionID, int newCategory, long acctNumber)
        {
            // checks values for valid data
            if (transactionID < 0)
            {
                throw new System.ArgumentException("Invalid range", "TransactionID");
            } // end if

            if (acctNumber < 0)
            {
                throw new System.ArgumentException("Invalid range", "AcctNumber");
            } // end if
            string updateSQL = "UPDATE Transactions SET CategoryID = @newCategory WHERE TransactionID = @transactionID AND AcctNumber = @acctNumber";

            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            cmd.Parameters.AddWithValue("@newCategory", newCategory);
            cmd.Parameters.AddWithValue("@transactionID", transactionID);
            cmd.Parameters.AddWithValue("@acctNumber", acctNumber);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateTransactionCategory

        // Updates the notification count for a goal item
        // Throws exception if goalID or count is below the minimum range
        public void updateNotificationGoals(int goalID, int count)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            if(count < 0)
            {
                throw new System.ArgumentException("Invalid range", "Count");
            } // end if

            string updateSQL = "UPDATE Goals SET NotificationCount = @num WHERE GoalID = @goalID";

            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            cmd.Parameters.AddWithValue("@num", count++);
            cmd.Parameters.AddWithValue("@goalID", goalID);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateNotificationGoals

        // Updates the notification count for a budget item
        // Throws exception if budgetID or count is below the minimum range
        public void updateNotificationBudgets(int budgetID, int count)
        {
            // checks values for valid data
            if (budgetID < 0)
            {
                throw new System.ArgumentException("Invalid range", "BudgetID");
            } // end if

            if (count < 0)
            {
                throw new System.ArgumentException("Invalid range", "Count");
            } // end if

            string updateSQL = "UPDATE Budgets SET NotificationCount = @num WHERE BudgetID = @budgetID";

            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            cmd.Parameters.AddWithValue("@num", count++);
            cmd.Parameters.AddWithValue("@budgetID", budgetID);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateNotificationBudgets

        // Updates the notifications flags for a user
        // Throws exception if userID is below the minimum range, or goal, budget, email, or text is not valid
        public void updateNotifications(int userID, int goal, int budget, int email, int text)
        {
            // checks values for valid data
            if (userID < 0)
            {
                throw new System.ArgumentException("Invalid range", "UserID");
            } // end if

            if (goal < 0 || goal > 1)
            {
                throw new System.ArgumentException("Invalid range", "Goal");
            } // end if

            if (budget < 0 || budget > 1)
            {
                throw new System.ArgumentException("Invalid range", "Budget");
            } // end if

            if (email < 0 || email > 1)
            {
                throw new System.ArgumentException("Invalid range", "Email");
            } // end if

            if (text < 0 || text > 1)
            {
                throw new System.ArgumentException("Invalid range", "Text");
            } // end if

            string updateSQL = "UPDATE Users SET GoalsNotifications = @goal, BudgetsNotifications = @budget, "
                + "EmailNotifications = @email, TextNotifications = @text WHERE UserID = @userID";

            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            cmd.Parameters.AddWithValue("@goal", goal);
            cmd.Parameters.AddWithValue("@budget", budget);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@userID", userID);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end updateNotifications

        //WRITTEN BY DAVID JONES
        //UPDATES PASSED PARAMETERS OF THE BUDGET WITH MATCHING ID 
        public Boolean updateBudget(int budgetID, long account, int categoryID, DateTime endDate, double maxAmt)
        {
            
            if (categoryID < 0)
            {
                throw new System.ArgumentException("Invalid range", "CategoryID");
            } // end if

            if (maxAmt < 0)
            {
                throw new System.ArgumentException("Invalid range", "MaxAmt");
            } // end if

            string updateSQL = "UPDATE Budgets SET CategoryID = @categoryID, EndDate = @endDate, MaxAmt = @maxAmt WHERE BudgetID = @budgetID";
            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@maxAmt", maxAmt);
            cmd.Parameters.AddWithValue("@budgetID", budgetID);
            connect.Open();
            int num = cmd.ExecuteNonQuery();
            connect.Close();

            if (num == 0)
            {
                return false;
            } // end if

            return true;
        } // end addBudget

        // Updates the notification count all goals and budgets to 0
        public void clearNotifications()
        {
            string updateSQL = "UPDATE Budgets SET NotificationCount = 0"; 

            SqlCommand cmd = new SqlCommand(updateSQL, connect);
            
            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            updateSQL = "UPDATE Goals SET NotificationCount = 0";

            cmd = new SqlCommand(updateSQL, connect);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } // end clearNotifications

        // Deletes a specified goal
        // Throws exception if goalID is below the minimum range
        public void deleteGoal(int goalID)
        {
            // checks values for valid data
            if (goalID < 0)
            {
                throw new System.ArgumentException("Invalid range", "GoalID");
            } // end if

            string deleteSQL = "DELETE FROM Goals WHERE GoalID = @goalID";

            SqlCommand cmd = new SqlCommand(deleteSQL, connect);
            cmd.Parameters.AddWithValue("@goalID", goalID);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } //end deleteGoal

        // Deletes a specified budget
        // Throws exception if budgetID is below the minimum range
        public void deleteBudget(int budgetID)
        {
            // checks values for valid data
            if (budgetID < 0)
            {
                throw new System.ArgumentException("Invalid range", "BudgetID");
            } // end if

            string deleteSQL = "DELETE FROM Budgets WHERE BudgetID = @budgetID";

            SqlCommand cmd = new SqlCommand(deleteSQL, connect);
            cmd.Parameters.AddWithValue("@budgetID", budgetID);

            connect.Open();
            cmd.ExecuteNonQuery();
            connect.Close();

            return;
        } //end deleteBudget
    } // end class
} // end namespace