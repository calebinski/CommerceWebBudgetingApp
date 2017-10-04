<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Goals.aspx.cs" Inherits="Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
	<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
	<meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Commerce Budgeting Tool</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link href="css/styles.css" rel="stylesheet"/>
	<link href="css/fonts.css" rel="stylesheet"/>
</head>
<body>
    <div class="container">
        <!-- Navigation -->
			<div class="col-sm-2">
				<div class="row">
                    <a href="Play.aspx">
					    <div class="navButton">
						    <p>Play</p>
					    </div>
                    </a>
				</div>
				<div class="row">
					<a href="Wallet.aspx">
					    <div class="navButton">
						<p>Wallet</p>
					    </div>
                    </a>
				</div>
				<div class="row">
					<a href="Goals.aspx">
					    <div class="navButton" style="background-color: #6ab244;">
						<p>Goals</p>
					    </div>
                    </a>
				</div>
				<div class="row">
					<a href="Budget.aspx">
					    <div class="navButton">
						<p>Budget</p>
					    </div>
                    </a>
				</div>
				<div class="row">
					<a href="Help.aspx">
					    <div class="navButton">
						<p>Help</p>
					    </div>
                    </a>
				</div>
			</div>
    <!-- Content -->
        <div class="col-sm-10">
            <div id="content">
                
                <script src ="js/populateGoals.js">


                </script>
                
                <!-- Trigger/Open The AddGoal Modal -->
                <div id="addGoalBtn">
                    <h4>+ Add a Goal!</h4> <!-- Clicking this should bring up a popup where a user may add a new goal. -->
                </div>
                <!-- The Modal -->
                <div id="addGoal" class="modal">
                    <!-- Modal content -->
                    <div class="modal-content">
                        <span class="close">&times;</span>
                            <form id="form1" runat="server">
                                <asp:Label id="GoalID" runat="server">Name your goal: </asp:Label>
                                <asp:TextBox id="GoalIDUserInput" runat="server"></asp:TextBox><br />
                                <asp:Label id="GoalAmt" runat="server">Goal amount: </asp:Label>
                                <asp:TextBox id="GoalAmtUserInput" runat="server"></asp:TextBox><br />
                                <asp:Button id="SubmitButton" runat="server" Text="Add Goal!"></asp:Button>
                            </form>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
	<!--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>-->
	<script src="bootstrap/js/jquery-3.1.1.min.js"></script>
	<!-- Include all compiled plugins (below), or include individual files as needed -->
	<script src="bootstrap/js/bootstrap.min.js"></script>
    <script src="js/scripts.js"></script>
</body>
</html>
