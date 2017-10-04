<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Goals.aspx.cs" Inherits="Main" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<meta charset="utf-8"/>
		<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
		<meta name="viewport" content="width=device-width, initial-scale=1"/>
		<title>Commerce App - Goals</title>
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
						<h4>Your Goals</h4>
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
								<h4><b>Create and manage financial goals.</b></h4>
							</div>
							<div class="col-xs-2 textRight">
								<!-- Requires additional work. -->
								<!-- Requires validation; user cannot specifiy amount of type double for flat rate. -->
								<div id="addGoalBtn" class="darkBtn textCenter"><h5>Add a Goal</h5></div>
							</div>
						</div>
						<div class="col-xs-12 tabWrapper">
                            <asp:Button ID="ActiveGoals" OnClick="setActive" CssClass="col-xs-2 tab" runat="server" Text="Active Goals"/>
                            <asp:Button ID="InactiveGoals" OnClick="setInactive" CssClass="col-xs-2 tab" runat="server" Text="Inactive Goals"/>
							<div class="col-xs-2"></div>
							<div class="col-xs-2"></div>
							<div class="col-xs-2"></div>
						</div>
                        <div id="activeGoals">
					<asp:ListView ID="GoalsList" DataKeyNames="GoalID" OnItemDeleting="GoalsList_DeleteGoal" OnItemEditing="GoalsList_ItemEditing"  OnItemCanceling="GoalsList_ItemCancel"  OnItemUpdating="GoalsList_ItemUpdating" runat="server">
						<LayoutTemplate>
							<div id="itemPlaceholder" runat="server"></div>
							<asp:DataPager runat="server" ID="GoalsListDataPager" PageSize="3">
								<Fields>
									<asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" />
                                    <asp:NumericPagerField NumericButtonCssClass="dataPagerStyle" />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" />
								</Fields>
							</asp:DataPager>
						</LayoutTemplate>
						<ItemTemplate>
							<div class="col-xs-12 itemHeader" runat="server">
								<div class="col-xs-1">
									<asp:ImageButton ID="FaveToggle" ToolTip="Set as Favorite" CssClass="faveBtn" OnPreRender="imageChoose"  runat="server" Text='<%#Eval("GoalID") %>' Width="35" Height="35"  OnCommand="toggleFavorite"  CommandArgument='<%#Eval("GoalID") %>'/>
								</div>
								<div class="col-xs-3 textLeft">
									<asp:Label ID="GoalName" CssClass="aspLabel" runat="server" Text='<%#Eval("Name") %>' />
								</div>
								<div class="col-xs-4 textCenter">

								</div>
								<div class="col-xs-4 textRight">
									<p>Save <asp:Label ID="GoalTotalAmount" runat="server" Text='<%#Eval("TotalAmt", "{0:c}") %>' />
									by <asp:Label ID="GoalDate" CssClass="aspLabel" runat="server" Text='<%#Eval("EndDate", "{0:MMM dd, yyyy}") %>' /></p>
								</div>
							</div>
							<div class="col-xs-12 itemContent" runat="server">
								<div class="col-xs-12 textCenter">
									<div class="progress">
										<div  class="progress-bar progress-bar-info" role="progressbar"  aria-valuenow='<%#:getPercent(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("TotalAmt"))).ToString() %>' aria-valuemin="0" aria-valuemax="100" style='<%#:widthString(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("TotalAmt")))%>'>
											
										</div>
									</div>
                                    You've saved <asp:Label ID="CurrentAmt" runat="server" Text='<%#Eval("CurrentAmt", "{0:c}") %>' />. (<%#:(getPercent(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("TotalAmt")))).ToString() %>% Complete.)
								</div>
								<div class="col-xs-12 textLeft" style="margin-bottom: 1em; padding-left: 1em; padding-right: 1em;">
									<asp:Label ID="UserDescription" CssClass="aspLabel" runat="server" Text='<%#Eval("Description") %>' />
								</div>
								<div class="col-xs-12">
									<div class="col-xs-9 textLeft">
										<!-- Fields to add funds -->
										<asp:TextBox ID="AddAmt" runat="server" Text=""></asp:TextBox>
										<asp:Button ID="AddFundsSubmit" CssClass="darkBtn textCenter" runat="server" Text="Add Funds" OnCommand="addFundsClick" CommandArgument='<%#Eval("GoalID") %>'/>
									</div>
									<div class="col-xs-3">
										<asp:Button ID="EditGoal" CssClass="darkBtn textCenter" runat="server" Text="Edit Goal" CommandName="Edit" />
										
									</div>
								</div>
							</div>
						</ItemTemplate>
						<EditItemTemplate >
							<div class="col-xs-12 itemHeader" runat="server" style="color: #000;">
								<div class="col-xs-1"></div>
								<div class="col-xs-3 textLeft">
									<asp:Label ID="EditName" CssClass="aspLabelWhite" runat="server" Text="Goal Name: "></asp:Label>
									<asp:TextBox ID="EditGoalName" runat="server" Text='<%#Bind("Name") %>' />
								</div>
								<div class="col-xs-4 textCenter">
									<asp:Label ID="EditAmt" CssClass="aspLabelWhite" runat="server" Text="Goal Amount: "></asp:Label><br />
									<asp:TextBox ID="EditGoalTotalAmount" runat="server" Text='<%#Bind("TotalAmt", "{0:c}") %>' /><br />
								</div>
								<div class="col-xs-4 textRight">
									<asp:Label ID="EditDate" CssClass="aspLabelWhite" runat="server" Text="Goal Date (mm/dd/yyyy): "></asp:Label>
									<asp:TextBox ID="EditGoalDate" runat="server" Text='<%#Bind("EndDate", "{0:d}") %>' />
								</div>
							</div>
							<div class="col-xs-12 itemContent" runat="server">
								<div class="col-xs-12 textLeft">
									<asp:Label ID="EditDesc" runat="server" Text="Description: "></asp:Label>
									<asp:TextBox ID="EditUserDescription" runat="server" Text='<%#Bind("Description") %>' />
								</div>
								<div class="col-xs-12">
                                    <div class="col-xs-3">
										<asp:Button ID="DeleteGoal" CssClass="darkBtn textCenter" runat="server" OnClientClick="if (!confirm('Are you sure you want to delete this goal?')) return false;" CommandName="Delete"  Text="Delete Goal" />
									</div>
                                    <div class="col-xs-6">

									</div>
									<div class="col-xs-3">
										<asp:Button ID="SubmitEdit" CssClass="darkBtn textCenter" runat="server"  Text="Save Changes"  CommandName="Update" CommandArgument='<%#:Bind("AcctNumber") %>' />
									</div>
								</div>
							</div>
						</EditItemTemplate>
					</asp:ListView>
                            </div>
                        <div id="inactiveGoals">

                        </div>
					<div class="col-xs-12 textRight">
                        <div class="col-xs-10"></div>
						<div id="desktopMode" class="col-xs-2">Desktop Mode</div>
					</div>
					</div>
					<div id="mobileNavBottom" class="col-xs-12">
						<div id="wallet_m" class="navBtn"><h3>Wallet</h3></div>
						<div id="profile_m" class="navBtn"><h3>Profile</h3></div>
						<div id="help_m" class="navBtn"><h3>Help</h3></div>
					</div>
				</div>
				
		</div>
		<!-- Add Goal Modal -->
		<div id="addGoal" class="modal" onclick="">
			<!-- Modal content -->
			<div class="row modal-content">
				
				<div class="col-xs-12">
					<div class="col-xs-6 textLeft">
						<h3>Add a Goal</h3><br />
					</div>
					<div class="col-xs-6 textRight">
						<span class="close">&times;</span>
					</div>
				</div>
				<div class="col-xs-12">
					<div class="col-xs-7 textLeft">
						<asp:Label ID="GoalID" CssClass="aspLabel" runat="server">Create a name for your goal. </asp:Label><br /><br />
					</div>
					<div class="col-xs-5">
						<asp:TextBox ID="GoalIDUserInput" runat="server"></asp:TextBox><br />
						<!-- Requires an input for the field. -->
						
						<!-- Info for further validation: https://msdn.microsoft.com/en-us/library/ff650303.aspx -->
					</div>
				</div>
				<div class="col-xs-12">
					<div class="col-xs-7 textLeft">
						<asp:Label ID="GoalAmt" CssClass="aspLabel" runat="server">How much would you like to save? </asp:Label><br /><br />
					</div>
					<div class="col-xs-5">
						<asp:TextBox ID="GoalAmtUserInput" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="col-xs-12">
					<div class="col-xs-7 textLeft">
						<asp:Label ID="GoalDate" CssClass="aspLabel" runat="server">By what date should this goal be reached? (mm/dd/yyyy)</asp:Label><br /><br />
					</div>
					<div class="col-xs-5">
						<asp:TextBox ID="GoalDateUserInput" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="col-xs-12">
					<div class="col-xs-7 textLeft">
						<asp:Label ID="GoalDesc" CssClass="aspLabel" runat="server">Describe this goal. (Optional)</asp:Label><br /><br />
					</div>
					<div class="col-xs-5">
						<asp:TextBox ID="GoalDescUserInput" CssClass="aspLargeTextBox" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="col-xs-12">
					<div class="col-xs-7 textLeft">
						<asp:Label ID="PaymentOption" CssClass="aspLabel" runat="server">How would you like to save money?</asp:Label><br /><br />
						<div id="flatRateIn">
							<asp:Label ID="FRLabel" CssClass="aspLabel" runat="server">Amount:</asp:Label>
							<asp:TextBox ID="FlatRateInput" runat="server"></asp:TextBox>
						</div>
						<div id="percentIn">
							<asp:Label ID="PCTLabel" CssClass="aspLabel" runat="server">Percentage:</asp:Label>
							<asp:TextBox ID="PercentageInput" runat="server"></asp:TextBox>
						</div>
					</div>
					<div id="paymentOptions" class="col-xs-5">
						<asp:RadioButtonList ID="PaymentOptions" runat="server">
							<asp:ListItem CssClass="radioBtn" ID="FlatRate" ClientIDMode="Static" runat="server">A specified amount each deposit.</asp:ListItem>
							<asp:ListItem CssClass="radioBtn" ID="Percentage" ClientIDMode="Static" runat="server">A percentage of each deposit.</asp:ListItem>
							<asp:ListItem CssClass="radioBtn" ID="FreeDeposit" runat="server">Manually add funds as desired.</asp:ListItem>
						</asp:RadioButtonList>
						
					</div>
				</div>
				<div class="col-xs-12" style="color: #FFF;">
					<asp:Button ID="SubmitButton" CssClass="darkBtn textCenter" OnCommand="addGoalClick" CommandArgument="" runat="server" Text="Add Goal"></asp:Button>
				</div>
			</div>
		</div>
		</form>
		<!-- Bootstrap Dependencies -->
		<script src="bootstrap/js/jquery-3.1.1.min.js"></script>
		<script src="bootstrap/js/bootstrap.min.js"></script>
		<!-- Javascript -->
		<script src="js/goals.js"></script>
	</body>
</html>