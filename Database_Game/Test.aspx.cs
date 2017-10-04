using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Tester
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataAbstract data = new DataAbstract();

            try
            {
                //data.addUser("Grey", "Ice", "GreyI", "grey");
                //data.addUser("Blue", "Grade", "BellG", "blue");

                //data.login("GreyI", "gray");

                //data.addGoal(1, 1, 20, DateTime.Today.AddMonths(1));

                //data.addCategory("Car");

                //DataSet work = data.returnGoals(1);
            }
            catch(Exception ex)
            {
                ex.ToString();
            } // end try/catch
        }
    }
}