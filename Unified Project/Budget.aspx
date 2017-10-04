<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Budget.aspx.cs" Inherits="Budget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
		<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
		<meta name="viewport" content="width=device-width, initial-scale=1"/>
		<title>Commerce App - Budget</title>
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
                        <h4>Your Budgets</h4>
                    </div>
					<div class="textRight col-xs-4">
					    <asp:Button ID="logOutButton" onClick="logoutClick"  CssClass="aspButton" style="background-color: #e1e1e1; color: #006847;" runat="server" Text="Logout" />
					</div>
				</div>
                <!-- Page Content -->
                <div id="pageContent" class="row">
                    <div id="mobileNavTop" class="col-xs-12">
                        <div id="summary_m" class="navBtn"><h3>Summary</h3></div>
                        <div id="budget_m" class="navBtn"><h3>Budget</h3></div>
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
                                <h4><b>Create and manage your budgets.</b></h4>
                            </div>
                            <div class="col-xs-2 textRight">
                                <!-- Requires additional work. -->
                                <!-- Requires validation; user cannot specifiy amount of type double for flat rate. -->
                                <div id="addBudgetBtn" class="darkBtn textCenter"><h5>Add Budget</h5></div>
                            </div>
                        </div>
                        <div id="histogram" class="col-xs-12" style:"display: block; margin: 0 auto;"></div> <br />
                        <asp:ListView ID="BudgetList" DataKeyNames="BudgetID" OnItemEditing="ListViewEdit"  OnItemUpdating="ListViewUpdate" runat="server" OnItemDeleting="BudgetList_DeleteGoal">
                        <LayoutTemplate>
                            <div id="itemPlaceholder" runat="server"></div>
                            <asp:DataPager runat="server" ID="BudgetListDataPager" PageSize="3">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" />
                                    <asp:NumericPagerField NumericButtonCssClass="dataPagerStyle" />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
							<!-- Numerical values associated with dollar amounts need to be truncated. -->
							<div class="col-xs-12 itemHeader" runat="server">
								<div class="col-xs-1">
									<!-- Toggle Favorite Button -->
                                    <asp:ImageButton ID="FaveToggle" ToolTip="Set as Favorite" CssClass="faveBtn" OnPreRender="imageChoose"  runat="server" Text='<%#Eval("BudgetID") %>' Width="35" Height="35"  OnCommand="toggleFavorite"  CommandArgument='<%#Eval("BudgetID") %>'/>
								</div>
								<div class="col-xs-5 textLeft">
									<asp:Label ID="CategoryName" CssClass="aspLabel" runat="server" Text='<%#Eval("Name") %>' />
								</div>
								<div class="col-xs-6 textRight">
									<p>Spend less than <asp:Label ID="MaxbudgetAmt" runat="server" Text='<%#Eval("MaxAmt", "{0:c}") %>' />
									between <asp:Label ID="StartDate" CssClass="aspLabel" runat="server" Text='<%#Eval("StartDate", "{0:MMM dd, yyyy}") %>' /> and
                                        <asp:Label ID="EndDate" CssClass="aspLabel" runat="server" Text='<%#Eval("EndDate", "{0:MMM dd, yyyy}") %>' />
									</p>
								</div>
							</div>
							<div class="col-xs-12 itemContent" runat="server">
								<div class="col-xs-12">
									<div class="progress">
										<div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow='<%#:(getPercent(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("MAxAmt")))).ToString() %>' aria-valuemin="0" aria-valuemax="100" style='<%#:widthString(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("MaxAmt")))%>'>
										</div>	
                                    </div>
                                    You've spent <asp:Label ID="CurrentAmt" runat="server" Text='<%#Eval("CurrentAmt", "{0:c}") %>' />. (<%#:(getPercent(Convert.ToDouble(Eval("CurrentAmt")) , Convert.ToDouble(Eval("MaxAmt")))).ToString() %>% of Limit.)
								</div>
                                <div class="col-xs-12">
                                    <div class="col-xs-10"></div>
                                    <div class="col-xs-2">
                                        <asp:Button ID="EditBudget" CssClass="darkBtn textCenter" CommandName="Edit" runat="server" Text="Edit Budget"/>
                                    </div>
                            </div>
							</div>
						</ItemTemplate>
                        <EditItemTemplate >
                            <div class="col-xs-12 itemHeader" runat="server" style="color: #000;">
                                <div class="col-xs-3">
                                    <asp:Label ID="EditCategory" CssClass="aspLabelWhite" runat="server"><p>Choose a category:</p></asp:Label>
                                    <asp:DropDownList ID="EditCategorySelect"   OnLoad="getCategories"  runat="server" AppendDataBoundItems="true"  >
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label ID="EditAccount" CssClass="aspLabelWhite" runat="server"><p>Choose an account:</p></asp:Label>
                                    <asp:DropDownList ID="EditAccountList" OnLoad="getAccounts" runat="server" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label ID="EditBudgetAmt" CssClass="aspLabelWhite" runat="server"><p>Amount:</p></asp:Label>
                                    <asp:TextBox ID="EditBudgetAmtField" Text='<%#Bind("MaxAmt") %>' runat="server"></asp:TextBox>
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label ID="EditEndDateLabel" CssClass="aspLabelWhite" runat="server"><p>End Date:</p></asp:Label>
                                    <asp:TextBox ID="EditEndDate" Text='<%#Bind("EndDate") %>' runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 itemContent" runat="server">
                                <div class="col-xs-6"><asp:Button ID="DeleteBudget" CssClass="darkBtn textCenter" runat="server"  OnClientClick="if (!confirm('Are you sure you want to delete this goal?')) return false;"  Text="Delete"  CommandName="Delete" /></div>
                                <div class="col-xs-6"><asp:Button ID="SubmitEdit" CssClass="darkBtn textCenter" runat="server"    Text="Save Changes"  CommandName="Update" /></div>
                                
                            </div>
                        </EditItemTemplate>
                    </asp:ListView>
                    <div class="col-xs-12 textRight">
                        <div class="col-xs-10"></div>
						<div id="desktopMode" class="col-xs-2">Desktop Mode</div>
					</div>
                    </div>
                    <div id="mobileNavBottom" class="col-xs-12">
                        <div id="goals_m" class="navBtn"><h3>Goals</h3></div>
                        <div id="wallet_m" class="navBtn"><h3>Wallet</h3></div>
                        <div id="profile_m" class="navBtn"><h3>Profile</h3></div>
                        <div id="help_m" class="navBtn"><h3>Help</h3></div>
                    </div>
                </div>
                
		</div>
        <!-- Add Budget Modal -->
        <div id="addBudget" class="modal" onclick="">
            <!-- Modal content -->
            <div class="row modal-content">
                <!-- Info for further validation: https://msdn.microsoft.com/en-us/library/ff650303.aspx -->
                <div class="col-xs-12">
                    <div class="col-xs-6 textLeft">
                        <h3>Add a Budget Item</h3><br />
                    </div>
                    <div class="col-xs-6 textRight">
                        <span class="close">&times;</span>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-6">
                        <asp:Label ID="Categroy" runat="server"><p>Choose a category:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:DropDownList ID="CategorySelect" runat="server" AppendDataBoundItems="false"  >
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-6">
                        <asp:Label ID="Account" runat="server"><p>Choose an account:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:DropDownList ID="AccountList" runat="server" AppendDataBoundItems="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 hidden">
                    <div class="col-xs-6">
                        <asp:Label ID="CustCat"  runat="server"><p>Name your custom category:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:TextBox ID="CustomCategory" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-6">
                        <asp:Label ID="BudgetAmt" runat="server"><p>Amount:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:TextBox ID="BudgetAmtField" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-6">
                        <asp:Label ID="StartDateLabel" runat="server"><p>Start Date:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:TextBox ID="StartDate" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="col-xs-6">
                        <asp:Label ID="EndDateLabel" runat="server"><p>End Date:</p></asp:Label>
                    </div>
                    <div class="col-xs-6 textRight">
                        <asp:TextBox ID="EndDate" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 textCenter" style="color: #FFF;">
                    <!-- Need event handler in code behind. -->
                    <asp:Button ID="SubmitButton" CssClass="darkBtn textCenter" OnClick="addBudgetClick" CommandArgument="" runat="server" Text="Add Budget"></asp:Button>
                </div>
            </div>
        </div>
    </form>
    <!-- Bootstrap Dependencies -->
	<script src="bootstrap/js/jquery-3.1.1.min.js"></script>
	<script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- Javascript -->
    <script src="plotly/plotly-latest.min.js"></script>
	<script src="js/budget.js"></script>
</body>
</html>
