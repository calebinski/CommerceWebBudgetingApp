using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataConnect;
using Tester;
using System.Web.Services;

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAbstract DA = new DataAbstract();

        DA.addUser("Candice", "Lastnamery", "Candice", "Candice"); 
         
    }

    [WebMethod]
    public static int hundredDollarBillsYall(string name, string address)
    {
       var result = 100;

        return result;
    }

}