<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Help.aspx.cs" Inherits="Help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
	<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
	<meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Commerce App - Help</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link href="css/styles.css" rel="stylesheet"/>
	<link href="css/fonts.css" rel="stylesheet"/>
</head>
<body>
        <form id="form1" runat="server">
                <div id="container" class="container">
				<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
				<!-- header -->
				<div id="header" class="col-xs-12">
				    <div class ="textLeft col-xs-4">
					    <h4>Commerce</h4>
					</div>
                    <div class="textCenter col-xs-4">
                        <h4>Help</h4>
                    </div>
					<div class="textRight col-xs-4">
					    <asp:Button ID="logOutButton" onClick="logoutClick" CssClass="aspButton" style="background-color: #e1e1e1; color: #006847;" runat="server" Text="Logout" />
					</div>
				</div>
                <!-- Page Content -->
                <div id="pageContent" class="row">
                    <div id="mobileNavTop" class="col-xs-12">
                        <div id="summary_m" class="navBtn"><h3>Summary</h3></div>
                        <div id="budget_m" class="navBtn"><h3>Budget</h3></div>
                        <div id="goals_m" class="navBtn"><h3>Goals</h3></div>
                        <div id="wallet_m" class="navBtn"><h3>Wallet</h3></div>
                        <div id="profile_m" class="navBtn"><h3>Profile</h3></div>
                        <div id="help_m" class="navBtn"><h3>Help</h3></div>
                    </div>
                    <div id="navigation" class="col-xs-2">
                        <div id="summary" class="navBtn"><h3>Summary</h3></div>
                        <div id="budget" class="navBtn"><h3>Budget</h3></div>
                        <div id="goals" class="navBtn"><h3>Goals</h3></div>
                        <div id="wallet" class="navBtn"><h3>Wallet</h3></div>
                        <div id="profile" class="navBtn"><h3>Profile</h3></div>
                        <div id="help" class="navBtn"><h3>Help</h3></div>
                    </div>
                    <div id="content" class="col-xs-10">
                        <div id="contentHeader" class="col-xs-12">
                            <div class="col-xs-10 textLeft">
                                <h1><b>Help Section</b></h1>

                                <h2><b>Transactions</b></h2>
                                <!--how to search for a transaction-->
                                <div id="searchTransactionsTopic" class="helpQ">
                                    <h3><b>Searching for a specific transaction.</b></h3>
                                </div>
                                <div id="searchTransactionSteps" style="display: none;">
                                    <h4>1. On the Wallet, page, select the text box labeled "Search".</h4>
                                    <h4>2. Input the description of the transaction you are looking for.</h4>
                                    <h4>3. Select the button labeled "Search" located to the right of the text box.</h4>
                                </div>

                                <!-- Edit a Transaction -->
                                <div id="editTransactionTopic" class="helpQ">
                                    <h3><b>Editing a transaction.</b></h3>
                                </div>
                                <div id="editTransactionSteps" style="display: none;">
                                    <h4>1. On the Wallet page, find the transaction you wish to edit.</h4>
                                    <h4>2. Select the hyperlink labeled “Edit” in the row of the transaction you wish to edit
                                                (this hyperlink can be found to the right of the category of the transaction).</h4>
                                    <h4>3. Input information you wish to change.</h4>
                                    <h4>4. Select the button labeled “Done” in the lower right hand corner of the pop-up.</h4>
                                </div>

                                <h2><b>Budgets</b></h2>
                                <!-- Adding a Budget -->
                                <div id="addBudgetTopic" class="helpQ">
                                    <h3><b>Creating a budget.</b></h3>
                                </div>
                                <div id="addBudgetSteps" style="display: none;">
                                    <h4>1. On the Budget page, select the button labeled “Add Budget” located in the top
                                                right hand corner of the screen underneath the top bar.</h4>
                                    <h4>2. Input information requested.</h4>
                                    <h4>3. Once you are satisfied with the information you input for the budget, select the
                                                button labeled “Add Budget” located at the bottom of the pop-up window.</h4>
                                </div>

                                <!-- Editing a Budget -->
                                <div id="editBudgetTopic" class="helpQ">
                                    <h3><b>Editing a budget.</b></h3>
                                </div>
                                <div id="editBudgetSteps" style="display: none;">
                                    <h4>1. On the Budget page, find the budget you wish to edit.</h4>
                                    <h4>2. Select the button labeled “Edit Budget” located in the lower right hand corner of
                                                the budget box you wish to edit.</h4>
                                    <h4>3. Input the information you wish to change about the budget.</h4>
                                    <h4>4. Select the button labeled “Done” in the lower part of the pop-up window.</h4>
                                </div>

                                <!-- Rmoving a Budget -->
                                <div id="removeBudgetTopic" class="helpQ">
                                    <h3><b>Removing a budget.</b></h3>
                                </div>
                                <div id="removeBudgetSteps" style="display: none;">
                                    <h4>1. On the budget page, find the budget you wish to remove.</h4>
                                    <h4>2. Select the button labeled “Edit Budget” located in the lower right hand corner of
                                                the budget box you wish to remove.</h4>
                                    <h4>3. Select the button labeled “Remove Budget”.</h4>
                                </div>

                                <!--Marking something as a favorite -->
                                <div id="favoriteTopic" class="helpQ">
                                    <h3><b>Favoriting a goal or budget.</b></h3>
                                </div>
                                <div id="favoriteSteps" style="display: none;">
                                    <h4>1. Look for the budget or goal you wish to favorite.</h4>
                                    <h4>2. In the top left-hand corner of the budget/goal box, select the round button.</h4>
                                    <h4>3. A star will appear in the middle of the button showing that the budget or goal has been "Favorited".</h4>
                                </div>

                                <h2><b>Goals</b></h2>
                                <!-- Adding a Goal -->
                                <div id="addGoalTopic" class="helpQ">
                                    <h3><b>Creating a goal.</b></h3>
                                </div>
                                <div id="addGoalSteps" style="display: none;">
                                    <h4>1. On the Goals page, select the button labeled “Add a Goal” in the upper right hand
                                                portion of the screen.</h4>
                                    <h4>2. Input the information requested.</h4>
                                    <h4>3.Once you are satisfied with the information you have given, select the “Add Goal”
                                                button located in the lower left-hand section of the pop-up menu.</h4>
                                </div>

                                <!-- Manually add funds to a goal -->
                                <div id="addGoalFundsTopic" class="helpQ">
                                    <h3><b>Manually adding funds to a goal.</b></h3>
                                </div>
                                <div id="addGoalFundsSteps" style="display: none;">
                                    <h4>1. On the Goals page, find the goal you wish to add funds to.</h4>
                                    <h4>2. In the text box located in the lower right hand section of the goal you wish to add
                                                funds to, input the dollar amount you wish to add to the goal.</h4>
                                    <h4>3. Select the Add funds button that is located directly to the right of the text box.</h4>
                                </div>

                                <!-- Edit a goal -->
                                <div id="editGoalTopic" class="helpQ">
                                    <h3><b>Editing a goal.</b></h3>
                                </div>
                                <div id="editGoalSteps" style="display: none;">
                                    <h4>1. On the Goals page, find the goal you wish to edit.</h4>
                                    <h4>2. In the lower right hand corner of the goal you wish to edit, select the “Edit Goal”
                                                button.</h4>
                                    <h4>3. Once you are satisfied with what you have changed, select the button in the
                                                lower right hand corner of the goal box labeled “Save Changes”.</h4>
                                </div>

                                <!-- delete a goal -->
                                <div id="deleteGoalTopic" class="helpQ">
                                    <h3><b>Deleting a goal.</b></h3>
                                </div>
                                <div id="deleteGoalSteps" style="display: none;">
                                    <h4>1. On the Goals page, find the goal you wish to delete.</h4>
                                    <h4>2. In the lower right hand corner of the goal you wish to edit, select the “Edit Goal”
                                                button.</h4>
                                    <h4>3. Select the “Delete Goal” button located in the lower left hand corner of the goal
                                                box.</h4>
                                    <h4>If you wish to continue to delete the goal select the “OK” button, if you do not
                                                wish to delete the goal select the “Cancel” button.</h4>
                                </div>

                                <!-- finding active/inactive goals -->
                                <div id="findActiveInactiveGoalTopic" class="helpQ">
                                    <h3><b>Finding active and inactive goals.</b></h3>
                                </div>
                                <div id="findActiveInactiveGoalSteps" style="display: none;">
                                    <h4>1. On the Goals page, select the “Inactive Goals” tab located under the text line,
                                                “Create and manage financial goals.”.</h4>
                                    <h4>2. In the lower right hand corner of the goal you wish to edit, select the “Edit Goal”
                                                button.</h4>
                                    <h4>3. On the Goals page, select the “Active Goals” tab located under the text line,
                                                “Create and manage financial goals.”.</h4>
                                </div>

                                <h2><b>User Profile</b></h2>
                                <!-- view user profile -->
                                <div id="viewUserProfileTopic" class="helpQ">
                                    <h3><b>Viewing your user profile.</b></h3>
                                </div>
                                <div id="viewUserProfileSteps" style="display: none;">
                                    <h4>1. On the Profile page, select the tab labeled “Profile” located under the text line,
                                                “View your profile and achievements.”.</h4>
                                </div>

                                <!-- view achievements -->
                                <div id="viewAchievementsTopic" class="helpQ">
                                    <h3><b>Viewing your achievements.</b></h3>
                                </div>
                                <div id="viewAchievementsSteps" style="display: none;">
                                    <h4>1. On the Profile page, select the tab labeled “Achievements” located under the text
                                            line, “View your profile and achievements.”.</h4>
                                </div>

                                <!-- notifications -->
                                <div id="notificationsTopic" class="helpQ">
                                    <h3><b>Finding notification settings.</b></h3>
                                </div>
                                <div id="notificationsSteps" style="display: none;">
                                    <h4>1. On the Profile page, select the tab labeled “Settings” located under the text line,
                                                “View your profile and achievements.”.</h4>
                                    <h4>2. If you wish to receive notifications about goals, check the box to the right of the
                                                text that says “Receive Goal Notifications”.</h4>
                                    <h4>3. If you wish to receive notifications about budgets, check the box to the right of the
                                                text that says “Receive Budget Notifications”.</h4>
                                    <h4>4. If you wish to receive notifications via email, check the box to the right of the text
                                                that says “Receive Notifications via Email”.</h4>
                                    <h4>5. If you wish to receive notifications via text, check the box to the right of the text
                                                that says “Receive Notifications via Text”.</h4>
                                    <h4>6. If you have checked or unchecked any of the above boxes, be sure to select the
                                                button labeled “Update” located below these checkboxes before you navigate away from this page.</h4>
                                </div>

                            </div>
                            <div class="col-xs-2 textRight">
                                
                            </div>
                        </div>               
                    <div class="col-xs-12 textRight">
                        <div class="col-xs-10"></div>
						<div id="desktopMode" class="col-xs-2">Desktop Mode</div>
					</div>
                    </div>
                    <div id="mobileNavBottom" class="col-xs-12">
                    </div>
                </div>
        </div>
		</form>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
	<!--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>-->
	<script src="bootstrap/js/jquery-3.1.1.min.js"></script>
	<!-- Include all compiled plugins (below), or include individual files as needed -->
	<script src="bootstrap/js/bootstrap.min.js"></script>
    <script src="js/help.js"></script>
</body>
</html>