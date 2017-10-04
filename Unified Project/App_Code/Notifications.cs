using System;
using System.Net.Mail;
using System.Web.UI;
using System.Data;
namespace Tester
{
    public class WidespreadNotifier
    {
        private string getGoalText(int accountNum)
        {
            string result = ""; //accrues text to be returned throughout function
            DataAbstract DA = new DataAbstract();
            DataTable goals = DA.returnIncompleteGoals(accountNum).Tables[0];
            result += "Your current Goal Updates \n"; 
            for (int j = 0; j < goals.Rows.Count; ++j) //iterate through all goals of account
            {
                DataRow g = goals.Rows[j];
                int notificationCount = Convert.ToInt32(g.Field<object>("NotificationCount"));
                int goalID = Convert.ToInt32(g.Field<object>("GoalID"));
                string goalName = Convert.ToString(g.Field<object>("Name"));
                double currentAmt = Convert.ToDouble(g.Field<object>("CurrentAmt"));
                double totalAmt = Convert.ToDouble(g.Field<object>("TotalAmt"));
                double percent = currentAmt / totalAmt;

                Notifications N = new Notifications();
                int notificationCountUpdate = N.Check("goals", percent * 100);

                if (notificationCountUpdate > notificationCount)
                {
                    DA.updateNotificationGoals(goalID, notificationCountUpdate);
                    string percentString = Convert.ToString(percent);
                    percentString = percentString.Remove(percentString.IndexOf('.') + 2);
                    string goalString = goalName + " has reached " + (percent * 100).ToString("N0") + "%\n";
                    result += goalString;
                }
            }//end of specific goal

            return result;
        }
        static public void notifyOne(int userID)
        {
            Boolean notifyGoals ;
            Boolean notifyBudgets;
            
            DataAbstract DA = new DataAbstract();
            notifyBudgets = Convert.ToBoolean(DA.returnUser(userID).Tables[0].Rows[0].Field<object>("GoalsNotifications"));
            notifyGoals = Convert.ToBoolean(DA.returnUser(userID).Tables[0].Rows[0].Field<object>("BudgetsNotifications"));
            DataTable accounts = DA.returnAccounts(userID).Tables[0];
            string emailMsg = "Report on your financial status \n";
            for (int i = 0; i < accounts.Rows.Count; ++i) //iterate through all accounts of the user
            {       
                long accountNum = Convert.ToInt64(accounts.Rows[i].Field<object>("AcctNumber"));


                //Section for proceessing Goal portion of email
                if (notifyGoals)
                {
                    //emailMsg += getGoalText(accountNum);
                }//end of goals section
                //Section for processing Budget section of email
                if (notifyBudgets)
                {
                    DataTable budgets = DA.returnBudgets(accountNum).Tables[0];
                    emailMsg += "\n\nYour current Budget Updates \n";
                    for (int j = 0; j < budgets.Rows.Count; ++j)
                    {
                        DataRow b = budgets.Rows[j];
                        int notificationCount = Convert.ToInt32(b.Field<object>("NotificationCount"));
                        int budgetID = Convert.ToInt32(b.Field<object>("BudgetID"));

                        string categoryName = DA.returnCategoryName(Convert.ToInt32(b.Field<object>("CategoryID")));
                        double currentAmt = Convert.ToDouble(b.Field<object>("CurrentAmt"));
                        double maxAmt = Convert.ToDouble(b.Field<object>("MaxAmt"));
                        double percent = currentAmt / maxAmt;

                        Notifications N = new Notifications();
                        int notificationCountUpdate = N.Check("budget", percent * 100);

                        if (notificationCountUpdate > notificationCount)
                        {
                            DA.updateNotificationBudgets(budgetID, notificationCountUpdate);
                            string budgetString = categoryName + " has reached " + (percent * 100).ToString("N0") + "%\n";
                            emailMsg += budgetString;
                        }
                    }//end of budgets 
                }
                string[] m = emailMsg.Trim().Split();
                Notifications Note = new Notifications();
                Note.sendEmail(emailMsg, accountNum.ToString());
            }//end of specific account
        }

        static public void notifyAll()
        {
            DataAbstract DA = new DataAbstract();

            DataTable userIDs = DA.returnAllUserIDs().Tables[0]; 
            for (int rows = 0; rows < userIDs.Rows.Count; ++rows )
            {
                int user = Convert.ToInt32(userIDs.Rows[rows].Field<object>("UserID"));
                notifyOne(user);
            }//end of userID 
        }
    }
    public class Notifications : System.Web.UI.Page
    {   
        public Notifications()
        {
       
        }

        //Input: "Goal" or "Budget" depending on which is being checked
        //Output: Number representing the number at which the database 
        //           counter should be if notification has already been sent
        public int Check(string PassedType, double PassedPercent)
        {
            string Type;
            double Percent;
            Type = PassedType;
            Percent = PassedPercent;
            if (Percent >= 100)
            {
                return 101;
            }
            else if (Percent < 100 && Percent >= 95 && Type == "budget")
            {
                //Budget has an additional threshold for notification, hence this block
                return 4;
            }

            else if (Percent < 95 && Percent >= 90)
            {
                return 3;
            }

            else if (Percent < 90 && Percent >= 75)
            {
                return 2;
            }

            else if (Percent < 75 && Percent >= 50)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        public void sendEmail(string msg, string account)
        {
            string[] m = msg.Split(' ');
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("dmjr43@mail.umkc.edu");
            mail.From = new MailAddress("team3waffles@gmail.com", "", System.Text.Encoding.UTF8);
            mail.Subject = "Account " + account + " Report";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = msg;
            mail.BodyEncoding = System.Text.Encoding.ASCII;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("team3waffles@gmail.com", "wafflesaregood");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(mail);
                Page.RegisterStartupScript("UserMsg", "<script>alert('Successfully Send...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
            
        }
    }
}