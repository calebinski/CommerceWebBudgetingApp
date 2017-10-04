<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="TestWebApp.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <input id="Text1" type="text" runat="server" /><div>
    
        <input id="Text2" type="text" runat="server" /><br />
            
    
    </div>



        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>select</asp:ListItem>
            <asp:ListItem>Save up money</asp:ListItem>
            <asp:ListItem>Spending Limit</asp:ListItem>
            <asp:ListItem>Spend less in category</asp:ListItem>
        </asp:DropDownList>

        <br /><br />
        <asp:DropDownList ID="Spending" runat="server">
            <asp:ListItem>select</asp:ListItem>
            <asp:ListItem>Clothes</asp:ListItem>
            <asp:ListItem>Entertainment</asp:ListItem>
            <asp:ListItem>Dining</asp:ListItem>
        </asp:DropDownList>
        <p>
				<asp:button type="submit" value="Submit" runat="server" OnClick="onClick" Text="Submit" ID="button1"/>
		</p>
        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
        <textarea id="TextArea1" cols="20" name="S1" rows="7" runat="server"></textarea><br />

        </form>
</body>
</html>
