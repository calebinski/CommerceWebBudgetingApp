using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tester;
using DataConnect;
using System.Diagnostics;

namespace TestWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            DataAbstract DA = new DataAbstract() ;

           // DA.addUser("David", "Jones", "David", "David");
        }

        protected void onClick(object sender, EventArgs e)
        {
            string n = username.Value;
            string p = password.Value;
            DataAbstract DA = new DataAbstract();

            int authenticated = DA.login(n, p);


            if (authenticated > -1)
            {

                Session.Add("userID", authenticated);

                Response.Redirect("~/WebForm2.aspx");


            }
            else username.Value = "FAIL";



            Response.Redirect("~/Home/Index");




        }
    }
}