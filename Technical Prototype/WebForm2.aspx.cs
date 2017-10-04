using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataConnect;
using Tester;

namespace TestWebApp
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void onClick(object sender, EventArgs e)
        {
            if (!Calendar1.SelectedDate.Equals("1/1/0001"))
            {
                string goal = Text1.Value;
                string amount = Text2.Value;

                double amountDouble = Convert.ToDouble(amount);
                string category = DropDownList1.SelectedValue;

                DataAbstract DA = new DataAbstract();

                bool entered = DA.addGoal((int)Session["userID"], 1, amountDouble, Calendar1.SelectedDate);

                if (entered)
                {
                    string confirm = "";
                    confirm += "Success: Goal entered into database \n";
                    confirm += "Goal: " + goal + "\n Amount: " + amount ;

                    TextArea1.Value = confirm;
                }

                else TextArea1.Value = "No Goal Entered into Database!";                
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}