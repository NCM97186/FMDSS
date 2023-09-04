﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="FMDSS.MIS_Reports.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>     
             
    <link href="https://jqueryui.com/jquery-wp-content/themes/jquery/css/base.css?v=1" rel="stylesheet">
     <link href="https://jqueryui.com/jquery-wp-content/themes/jqueryui.com/style.css" rel="stylesheet">
    <link href="~/css/dashboard/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="~/css/dashboard/dataTables.bootstrap.css" rel="stylesheet" />
    

    <script src="../js/dashboard/bootstrap-datepicker.js"></script>
    <script src="../Scripts/assets/jquery-blockui/jquery.blockUI.js"></script>
    <script src="http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" ></script>
    <script src="http://code.jquery.com/jquery-1.10.2.js" ></script>
    <script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js" ></script>

    <script>
        $(document).ready(function () {
            $('#TextBox_from').datepicker();
            $('#TextBox_to').datepicker();
        });
        

    </script>





</head>
<body>
    <form id="reportForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>

            <table style=" width:80% ">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>Duration</td>
                    <td>
                        <asp:DropDownList ID="DDL_Duration" runat="server">
                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                            <asp:ListItem Text="Quarterly" Value="Quarterly"></asp:ListItem>
                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            <asp:ListItem Text="Date Wise" Value="DateWise"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>

                <tr>
                    <td>Date</td>
                    <td>
                        <asp:TextBox ID="TextBox_from" runat="server" ClientIDMode="Static"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="TextBox_to" runat="server" ClientIDMode="Static" ></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>CIRCLE </td>
                    <td>
                        <asp:DropDownList ID="DDL_CIRCLE_CODE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_CIRCLE_CODE_OnSelectedIndexChanged" >
                            
                        </asp:DropDownList></td>
                </tr>

                   <tr>
                    <td>DIVISON</td>
                    <td>
                        <asp:DropDownList ID="DDL_DIVISON_CODE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_DIVISON_CODE_OnSelectedIndexChanged" >
                            
                        </asp:DropDownList></td>
                </tr>

                <tr>
                    <td>RANGE</td>
                    <td>
                        <asp:DropDownList ID="DDL_RANGE" runat="server">
                           
                        </asp:DropDownList></td>
                </tr>

                <tr>
                    <td>Service Category</td>
                    <td>
                        <asp:DropDownList ID="DDL_ServiceCategory" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DDL_ServiceCategory_SelectedIndexChanged">
                           
                        </asp:DropDownList></td>
                </tr>

                <tr>
                    <td>Service Name</td>
                    <td>
                        <asp:DropDownList ID="DDL_ServiceName" runat="server">
                           
                        </asp:DropDownList></td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="Button_submit" runat="server" Text="Submit" Height="28px" OnClick="Button_submit_Click" Width="102px" /></td>
                    <asp:Label ID="Label_error" runat="server" ForeColor="Red" Text=""></asp:Label>
                </tr>

            </table>


            <rsweb:ReportViewer ID="rdView" runat="server" Height="481px" Width="1096px" >


            </rsweb:ReportViewer>

        </div>
    </form>
</body>
</html>
