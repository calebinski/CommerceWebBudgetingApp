using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataConnect;
using Tester;
using System.Web.Services;
using System.Data;
using System.Net.Mail;
//using Microsoft.Win32.TaskScheduler; 

public partial class Main : System.Web.UI.Page
{

    static int userID;
    
    //emtpy but needed to handle listview event
    public void GoalsList_ItemEditing(Object sender, ListViewEditEventArgs e)
    {
        //ListViewItem goal = GoalsList.Items[e.NewEditIndex];
        //g = goal;
    }

    public void GoalsList_DeleteGoal(object sender, ListViewDeleteEventArgs e)
    {
        
        ListViewItem goal = GoalsList.Items[e.ItemIndex]; //obtain copy of relevant goal
        int id = Convert.ToInt32(e.Keys["GoalID"]);
        DataAbstract DA = new DataAbstract();
        DA.deleteGoal(id);
        Response.Redirect("~/Goals.aspx"); //reload to show changes
    }

    //handles ListViewUpdateEvent
    //pulls data bound in the Edit Template and calls abstract layer to change database
    //Date formats with slashes or dashes work. $ icons in number field replaces with blank spaces
    public void GoalsList_ItemUpdating(Object sender, ListViewUpdateEventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        long accountNum = Convert.ToInt64(e.NewValues["AcctNumber"]); //same account as before update
        string name = Convert.ToString(e.NewValues["Name"]);
        double amt = Convert.ToDouble(e.NewValues["TotalAmt"].ToString().Replace('$' , ' '));
        string des = Convert.ToString(e.NewValues["Description"]);
        DateTime newDate = Convert.ToDateTime(e.NewValues["EndDate"].ToString().Replace('-', '/'));
        int  gID = Convert.ToInt32(e.Keys["GoalID"]);

        DA.updateGoalEndDate(gID, newDate, accountNum);
        DA.updateGoalName(gID, name, accountNum);
        DA.updateGoalTotalAmt(gID, amt, accountNum);

        Response.Redirect("~/Goals.aspx");
    }

    //inputs: num = the current amount saved, denom = goal worked towards
    //output: string to be used to set width of bar (setting width of style)
    //        maximum output is "width: 100%"
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
        realVal = realVal - (realVal % 1); //rounds down to number between 0 and 100
        int result = Convert.ToInt32(realVal); 
        return result;
    }

   //emtpy but needed to handle listview event
    public void GoalsList_ItemCancel(object sender, ListViewCancelEventArgs e)
    {
        return;
    }

    //Before page loads, the logged in user data is pulled.
    //If none are logged in, user 1 is logged in (for easier development and demonstration)
    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        if (Convert.ToInt32(Session["userID"]) != 0)
        {
            userID = Convert.ToInt32(Session["userID"]);
            DataSet accountData = DA.returnAccounts(userID); //gets all accounts
            System.Data.DataTable accountsTable = accountData.Tables[0]; //table holding all account entries for the user
            object s = accountsTable.Rows[0].Field<object>("AcctNumber"); //sets the default account to the first of the user's accounts. LIKELY CHANGE LATER.
            if (Convert.ToInt64(Session["account"]) == 0)Session["account"] = Convert.ToInt64(s);                        //saves the first account as the default account during the session
            userID = Convert.ToInt16(Session["userID"]);                    //saves the Session userID to the variable on this page 
        }
        else
        {
            Session["userID"] = 1;  //temporary solution for demo 3/19/2017
            Session["account"] = 211111110;
        }

        DataSet DS = new DataSet();

        //Sets the source for the listview 
        //Session's tab key decides if active or inactive tabs are displayed
        if (Convert.ToString(Session["tab"]) == "Inactive")
        {
            Session.Add("tab", "Inactive");
            DS = DA.returnCompleteGoals(Convert.ToInt64(Session["account"]));
            GoalsList.DataSource = DS;
            GoalsList.DataBind();

            //sets addfunds box and button to hidden
            TextBox addbox = new TextBox();
            Button addbutn = new Button();
            for (int i = 0; i < GoalsList.Items.Count(); i++)
            {
                addbox = (TextBox)GoalsList.Items[i].FindControl("AddAmt");
                addbutn = (Button)GoalsList.Items[i].FindControl("AddFundsSubmit");
                if (addbutn != null)
                {
                    if (addbox != null)
                    {
                        addbox.Visible = false;
                        addbutn.Visible = false;
                    }
                }
            }
            

        }
        else
        {
            Session.Add("tab", "Active");
            DS = DA.returnIncompleteGoals(Convert.ToInt64(Session["account"]));
            GoalsList.DataSource = DS;
            GoalsList.DataBind();
            //sets addfunds box and button to visible
            TextBox addbox = new TextBox();
            Button addbutn = new Button();

            for (int i = 0; i < GoalsList.Items.Count(); i++)
            {
                addbox = (TextBox)GoalsList.Items[i].FindControl("AddAmt");
                addbutn = (Button)GoalsList.Items[i].FindControl("AddFundsSubmit");
                if (addbutn != null)
                {
                    if (addbox != null)
                    {
                        addbox.Visible = true;
                        addbutn.Visible = true;
                    }
                }
            }

        }

        
    }

    //toggles page to display active goals tab
    public void setActive(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["tab"]) != "Active")
        {
            Session.Add("tab", "Active");
            Response.Redirect("~/Goals.aspx");
            
        }
    }

    //toggles page to display inactive goals tab
    public void setInactive(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["tab"]) != "Inactive")
        {
            Session.Add("tab", "Inactive");
            Response.Redirect("~/Goals.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {       
        
    }

    //handles the toggling of favorite goals
    //called when the favorite icon is clicked by the user
    public void toggleFavorite(object sender, CommandEventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        DataRow DR = DA.returnOneGoal(Convert.ToInt32(e.CommandArgument)).Tables[0].Rows[0];
        bool pastFavor = Convert.ToBoolean(DR.Field<object>("Favorite"));
        DA.updateGoalFavorite(Convert.ToInt32(e.CommandArgument), !pastFavor);
    }

    //handles the presentation of correct favorite/nonfavorite icons
    //changes the src of image to be correct based on database
    public void imageChoose(object sender, EventArgs e)
    {
        Image x = (Image)sender;
        int id = Convert.ToInt32(x.Attributes["Text"]);
        DataAbstract DA = new DataAbstract();
        DataRow DR = DA.returnOneGoal(id).Tables[0].Rows[0];
        bool fav = Convert.ToBoolean(DR.Field<object>("favorite"));
        if (fav) x.Attributes["src"] = "images/faveTrue.png";
        else x.Attributes["src"] = "images/faveFalse.png";
        sender = x;
    }

    //called by button clicked by user
    //uses bound data along with abstract layer calls to update database
    public void addFundsClick(object sender, CommandEventArgs e)
    {
        
        int id = Convert.ToInt32(e.CommandArgument);
        System.Diagnostics.Debug.WriteLine(id);
        DataAbstract DA = new DataAbstract();
        DataRow DR = DA.returnOneGoal(id).Tables[0].Rows[0];
        double prev = Convert.ToDouble(DR.Field<object>("CurrentAmt"));

        Button addBtn = sender as Button; //finding listview item, finding textbox value
        ListViewItem lvi = addBtn.NamingContainer as ListViewItem; //gets listViewItem containing the clicked button
        TextBox tb = lvi.FindControl("AddAmt") as TextBox;
        string tbs = tb.Text;

        if (tbs != "")      
        {
            double toAdd = Convert.ToDouble(tbs);
            double newAmount = prev + toAdd;
            DA.updateGoalCurrentAmount(id, newAmount, Convert.ToInt64(Session["account"]));
            Response.Redirect("~/Goals.aspx");
        }
        
    }

    //called by button clicked in addGoal modal
    //gets data bound in ListViewItem Edit View
    //uses that data with abstract layer to update database
    public void addGoalClick(Object sender,
                           EventArgs e)
    {        
        DataAbstract DA = new DataAbstract();
        string name = GoalIDUserInput.Text;
        long accountNum = Convert.ToInt64(Session["account"]); //uses the session to recieve the account
        double amount = Convert.ToDouble(GoalAmtUserInput.Text);     //holds amount of the goal
        string desc = GoalDescUserInput.Text;                       //optional description of the goal
        int transRadioSelection = PaymentOptions.SelectedIndex;

        string transactionType = PaymentOptions.SelectedItem.Attributes["ID"];
        double toAdd = 0; //the percentage or dollar amount to be added each time
        string dateString = GoalDateUserInput.Text.Replace('-', '/').Trim();
        dateString = dateString.Replace(' ', '/');
        DateTime endDate = Convert.ToDateTime(GoalDateUserInput.Text);
        if (transactionType == "Percentage")
        {
            toAdd = Convert.ToDouble(PercentageInput.Text);
        }

        if (transactionType == "FlatRate")
        {
            toAdd = Convert.ToDouble(FlatRateInput.Text);
        }

        //acctNumber name totalAmount, datetime, transType, TransAmt
        DA.addGoal(accountNum, name, amount, endDate, transactionType, toAdd, desc);

        //Reset form data after button click. Form data was resent upon refresh without this.
        Session["ViewState"] = null;
        Response.Redirect("Goals.aspx");
    }

    //clears session values and returns to login screen 
    public void logoutClick(Object sender, EventArgs e)
    {
        Session["ViewState"] = null;
        Session["userID"] = null;
        Session["account"] = null;
        Response.Redirect("Login.aspx");
    }
}
