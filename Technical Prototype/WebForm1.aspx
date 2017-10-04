<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TestWebApp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div style="width: auto; margin-right: auto; margin-left: auto;">
		<div style="margin-left: auto; margin-right: auto; padding: 10px; text-align: center;">
				Username:
				<input type="text" id ="username"  name="username" runat="server" />
				<br/><br/>
				Password:
				<input type="password" id="password" name="password" runat="server"/>
				<br/><br/>
				<asp:button type="submit" value="Submit" runat="server" OnClick="onClick" Text="Submit"/>
		</div>
	</div>
        
    </form>
</body>
</html>
