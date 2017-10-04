using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tester;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


public partial class Budget : System.Web.UI.Page
{
    static int userID;
    DataSet BudgetDS;
    DateTime d1 = new DateTime(2017, 1, 1);
    DateTime d2 = new DateTime(2017, 6, 1);
    //DateTime now = DateTime.Now;
    //DateTime later = DateTime.Now.AddMonths(1);
    


    protected void Page_Load(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        if (Convert.ToInt32(Session["userID"]) != 0)
        {
            userID = Convert.ToInt32(Session["userID"]);
            DataSet accountData = DA.returnAccounts(userID); //gets all accounts
            System.Data.DataTable accountsTable = accountData.Tables[0]; //table holding all account entries for the user
            object s = accountsTable.Rows[0].Field<object>("AcctNumber"); //sets the default account to the first of the user's accounts. LIKELY CHANGE LATER.
            if (Convert.ToInt64(Session["account"]) == 0) Session["account"] = Convert.ToInt64(s);                        //saves the first account as the default account during the session
            userID = Convert.ToInt16(Session["userID"]);                    //saves the Session userID to the variable on this page 
        }
        else
        {
            Session["userID"] = 1;  //temporary solution for demo 3/19/2017
            Session["account"] = 211111110;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        if (Convert.ToInt32(Session["userID"]) != 0)
        {
            userID = Convert.ToInt32(Session["userID"]);
            DataSet accountData = DA.returnAccounts(userID); //gets all accounts
            System.Data.DataTable accountsTable = accountData.Tables[0]; //table holding all account entries for the user
            object s = accountsTable.Rows[0].Field<object>("AcctNumber"); //sets the default account to the first of the user's accounts. LIKELY CHANGE LATER.
            if (Convert.ToInt64(Session["account"]) == 0) Session["account"] = Convert.ToInt64(s);                        //saves the first account as the default account during the session
            userID = Convert.ToInt16(Session["userID"]);                    //saves the Session userID to the variable on this page 
        }
        else
        {
            Session["userID"] = 1;  //temporary solution for demo 3/19/2017
            Session["account"] = 211111110;
        }

        BudgetDS = DA.returnBoundedBudgets(Convert.ToInt64(Session["account"]), d1, d2);

        //Sets the source for the listview 

        BudgetList.DataSource = BudgetDS;
        BudgetList.DataBind();
        //LoadSubjects();
        DataTable accounts = DA.returnAccounts(userID).Tables[0];
        DropDownList DDL = AccountList;

        DDL.Items.Insert(0, new ListItem("Select Account", "0"));
        DDL.DataSource = accounts;
        DDL.DataTextField = "AcctNumber";
        DDL.DataValueField = "AcctNumber";

        DDL.DataBind();

        DataTable categories = DA.returnCategories(userID).Tables[0];
        DropDownList DDL2 = CategorySelect;

        DDL2.Items.Insert(0, new ListItem("Select Category", "0"));
        DDL2.DataSource = categories;
        DDL2.DataTextField = "Name";
        DDL2.DataValueField = "CategoryID";

        DDL2.DataBind();
        
    }

    public void ListViewEdit(object sender, ListViewEditEventArgs e)
    {
        //
    }

    
    

    public void ListViewUpdate(object sender, ListViewUpdateEventArgs e)
    {

        ListViewItem item = ((ListView)sender).Items[Convert.ToInt32(e.ItemIndex)];
        DropDownList Accounts = (DropDownList)item.FindControl("EditAccountList");
        DropDownList Categories = (DropDownList)item.FindControl("EditCategorySelect");
        
        DataAbstract DA = new DataAbstract();
        
        int id = Convert.ToInt32(e.Keys["BudgetID"]);
        int catID = Convert.ToInt32(Categories.SelectedValue);
        long account = Convert.ToInt64(Accounts.SelectedValue);
        double max = Convert.ToDouble(e.NewValues["MaxAmt"]);
        string dateString = Convert.ToString(e.NewValues["EndDate"]);
        DateTime endDate = Convert.ToDateTime(dateString);


        DA.updateBudget(id, Convert.ToInt64(Session["Account"]),catID, endDate, max);
        Response.Redirect("~/Budget.aspx");
    }

    public void BudgetList_DeleteGoal(object sender, ListViewDeleteEventArgs e)
    {
        
        ListViewItem goal = BudgetList.Items[e.ItemIndex];

        int id = Convert.ToInt32(e.Keys["BudgetID"]);
        DataAbstract DA = new DataAbstract();
        DA.deleteBudget(id);
        Response.Redirect("~/Budget.aspx");
    }

    public void getCategories(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        DataTable categories = DA.returnCategories(userID).Tables[0];
        DropDownList DDL = CategorySelect;
        DDL = (DropDownList)sender;
        DDL.Items.Insert(0, new ListItem("Select Category", "0"));
        DDL.DataSource = categories;
        DDL.DataTextField = "Name";
        DDL.DataValueField = "CategoryID";
       
        //DDL.DataBind();
        DDL.ID = "EditCategorySelect";



    }

    public void getAccounts(object sender, EventArgs e)
    {
        
        DataAbstract DA = new DataAbstract();
        DataTable accounts = DA.returnAccounts(userID).Tables[0];
        DropDownList DDL = AccountList;
        DDL = (DropDownList)sender;
        DDL.Items.Insert(0, new ListItem("Select Account", "0"));
        DDL.DataSource = accounts;
        DDL.DataTextField = "AcctNumber";
        DDL.DataValueField = "AcctNumber";
        
        //DDL.DataBind();
        
        DDL.ID = "EditAccountList";

        //sender = DDL;

    }

    private void LoadSubjects()
    {
        DataAbstract DA = new DataAbstract();
        DataTable categories = DA.returnCategories(userID).Tables[0];
        DataTable accounts = DA.returnAccounts(userID).Tables[0];

        //Set sources for the dropdowns and associate the text and value 
        //   based on the datatable fields

        CategorySelect.DataSource = categories;
        CategorySelect.DataTextField = "Name";
        CategorySelect.DataValueField = "CategoryID";
        CategorySelect.DataBind();
        
                
        AccountList.DataSource = accounts;
        AccountList.DataTextField = "AcctNumber";
        AccountList.DataValueField = "AcctNumber";
        AccountList.DataBind();
          
        CategorySelect.Items.Insert(0, new ListItem("Select Category", "0"));
        AccountList.Items.Insert(0, new ListItem("Select Account", "0"));

    }

    public string widthString(double num, double denom)
    {
        double width = (num / denom) * 100;
        if (width > 100) width = 100;
        string s = "width: " + width.ToString() + "%";
        return s;
    }

    //Take two doubles and get their percent as an integer, always rounded down
     public int getPercent(double numerator, double denominator)
    {
        double realVal = numerator / denominator;
        realVal *= 100;
        realVal = realVal - (realVal % 1);
        int result = Convert.ToInt32(realVal);
        return result;
    }

    //MIGHT NEVER GET USED.
    public void checkOther(Object sender,
                          EventArgs e)
    {
        DropDownList DDL = (DropDownList)sender;
        if (DDL.SelectedItem.Text == "other")
        {
            //DO STUFF IF OTHER
        }
        else
        {
            //DO STUFF IF NOT OTHER 
        }
    }

    public void addBudgetClick(Object sender,
                          EventArgs e)
    {
        DataAbstract DA = new DataAbstract();

        int categoryID = Convert.ToInt32(CategorySelect.SelectedValue); //REAL ONE WILL USE INDEX OF DROP-DOWN? 

        long accountNum = Convert.ToInt64(AccountList.SelectedValue) ; //uses the session to recieve the account
        double amount = Convert.ToDouble(BudgetAmtField.Text);     //holds amount of the goal
     
        //CHECK LINE BELOW FOR PROPER VALUES 
        DateTime endDate = Convert.ToDateTime(EndDate.Text.Replace('-','/'));
        DateTime startDate = Convert.ToDateTime(StartDate.Text.Replace('-', '/'));
        //ADD CODE TO GET CHECKBOX VALUE 
        int favorited = 0;
        //CheckBox CB = new CheckBox();  //REPLACE THIS WITH CHECKBOX VALUE!!
        //if (CB.Checked) favorited = 1;
        DA.addBudget(accountNum, categoryID, startDate, endDate, amount, favorited);
        
        //Reset form data after button click. Form data was resent upon refresh without this.
        Session["ViewState"] = null;
        Response.Redirect("Budget.aspx");
    }
    
    

    public void imageChoose(object sender, EventArgs e)
    {
        Image x = (Image)sender;
        int id = Convert.ToInt32(x.Attributes["Text"]);
        DataAbstract DA = new DataAbstract();
        DataRow DR = DA.returnOneBudget(id).Tables[0].Rows[0];
        bool fav = Convert.ToBoolean(DR.Field<object>("favorite"));
        if (fav) x.Attributes["src"] = "images/faveTrue.png";
        else x.Attributes["src"] = "images/faveFalse.png";
        sender = x;
    }

    public void toggleFavorite(object sender, CommandEventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        DataRow DR = DA.returnOneBudget(Convert.ToInt32(e.CommandArgument)).Tables[0].Rows[0];
        bool pastFavor = Convert.ToBoolean(DR.Field<object>("Favorite"));
        DA.updateBudgetFavorite(Convert.ToInt32(e.CommandArgument), !pastFavor);
    }


    [WebMethod]		
     public static string[,] getbudgetData()   		
     {		
 		
         DataAbstract DA = new DataAbstract();
         

         DataSet accountData = DA.returnAccounts(userID);		
         System.Data.DataTable accountsTable = accountData.Tables[0]; //table holding all account entries for the user		
         int accountCount = accountsTable.Rows.Count; //the total number of accounts for the user		
 		
         int totalBudgetCount = 0;		
 		
         //grab each account under the userID, grab the goals for each account, sum number of all goals		
         for (int num = 0; num<accountCount; ++num)		
         {		
             System.Data.DataRow accountRow = accountsTable.Rows[num];		
             object s = accountRow.Field<object>("AcctNumber");		
             long accountNum = Convert.ToInt64(s);		
             totalBudgetCount += DA.returnBudgets(accountNum).Tables[0].Rows.Count;		
         }		
 		
 		
         string[,] result = new string[totalBudgetCount, 9]; //will hold data for all goals in all accounts		
 		
         int total = 0;		
 		
         //populate the 2D array		
 		
         for (int i = 0; i<accountCount; ++i) //variable i iterates through accounts		
         {		
             System.Data.DataRow accountRow = accountsTable.Rows[i];		
             long accountNum = Convert.ToInt64(accountRow.Field<object>("AcctNumber"));		
             DataSet D = DA.returnBudgets(accountNum);		
 		
 		
             System.Data.DataTable T = D.Tables[0];         //Gets the actual Goals Table, filtered appropriately 		
             int rowCount = T.Rows.Count;                    //Number of entries		
 		
             for (int j = 0; j<rowCount; ++j) //variable j iterates through budgets of an account		
             {		
                 //RAW DATABASE OBJECTS		
                 object BudgetIDData, CategoryIDData, StartDateData, EndDateData, MaxAmtData, CompletedData, FailedData, CurrentAmt;		
                 //BUDGET INFOS TO BECOME STRINGS		
                 string BudgetID, CategoryID, AcctNumber, StartDate, EndDate, MaxAmt, Completed, Failed, Current;		
                 System.Data.DataRow DR = T.Rows[j];         //gets the current row we're copying 		
 		
                 //BudgetID		
                 BudgetIDData = DR.Field<object>("BudgetID");		
                 BudgetID = Convert.ToString(BudgetIDData);		
                 //CategoryID		
                 CategoryIDData = DR.Field<object>("CategoryID");		
                 CategoryID = DA.returnCategoryName(Convert.ToInt32(CategoryIDData));		
                 //AcctNumber		
                 AcctNumber = Convert.ToString(accountNum);		
                 //StartDate		
                 StartDateData = DR.Field<object>("StartDate");		
                 StartDate = Convert.ToDateTime(StartDateData).ToString("M dd, yyyy");		
                 //EndDate		
                 EndDateData = DR.Field<object>("EndDate");		
                 EndDate = Convert.ToDateTime(EndDateData).ToString("M dd, yyyy");		
                 //MaxAmy		
                 MaxAmtData = DR.Field<object>("MaxAmt");		
                 MaxAmt = Convert.ToInt32(MaxAmtData).ToString("C2");		
                 //Completed?		
                 CompletedData = DR.Field<object>("Completed");		
                 Completed = Convert.ToString(CompletedData);		
                 //Failed? 		
                 FailedData = DR.Field<object>("Failed");		
                 Failed = Convert.ToString(FailedData);		
 		
                 CurrentAmt = DR.Field<object>("CurrentAmt");		
                 Current = Convert.ToString(CurrentAmt);		
 		
                 result[total, 0] = BudgetID;		
                 result[total, 1] = CategoryID;		
                 result[total, 2] = AcctNumber;		
                 result[total, 3] = StartDate;		
                 result[total, 4] = EndDate;		
                 result[total, 5] = MaxAmt;		
                 result[total, 6] = Completed;  // "false" or "true"		
                 result[total, 7] = Failed;    // "false" or "true"		
                 result[total, 8] = Current;	
                 ++total;		
             }//end goal		
 		
         }		
         return result;		
     }
    public void logoutClick(Object sender, EventArgs e)
    {
        Session["ViewState"] = null;
        Session["userID"] = null;
        Session["account"] = null;
        Response.Redirect("Login.aspx");
    }

}

