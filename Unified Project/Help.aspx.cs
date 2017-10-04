using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tester;
using System.Data;
public partial class Help : System.Web.UI.Page
{
    int userID;
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();
        DA.clearNotifications();
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
            Session["userID"] = 1;   //temporary solution for demo 3/19/2017
            Session["account"] = 211111110;
        }
    }
    public void logoutClick(Object sender, EventArgs e)
    {
        Session["ViewState"] = null;
        Session["userID"] = null;
        Session["account"] = null;
        Response.Redirect("Login.aspx");
    }
}
