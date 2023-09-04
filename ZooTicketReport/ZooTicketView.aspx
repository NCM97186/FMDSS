<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZooTicketView.aspx.cs" Inherits="FMDSS.ZooTicketReport.ZooTicketView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript">
        function closeWindow() {
            setTimeout(function () {
                window.close();
            }, 40000);
        }

        window.onload = closeWindow();
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').bind('copy paste cut', function (e) {
                e.preventDefault(); //disable cut,copy,paste
                //alert('cut,copy & paste options are disabled !!');
            });
        });
</script>


</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div>

            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="709px"
                SizeToReportContent="True"
                InteractiveDeviceInfos="(Collection)"
                ShowBackButton="False" ShowCredentialPrompts="False"
                ShowDocumentMapButton="False" ShowExportControls="False" ShowFindControls="False"
                ShowPageNavigationControls="False" ShowParameterPrompts="False" ShowPrintButton="False"
                ShowPromptAreaButton="False" ShowRefreshButton="False" ShowToolBar="False" ShowWaitControlCancelLink="False">
                <LocalReport ReportEmbeddedResource="ZooTicketReport.Report1.rdlc">

                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ZooHeaderFooter" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="ZooTicketDataSetTableAdapters.Sp_Zoo_SelecTicketDetailTableAdapter"></asp:ObjectDataSource>

        </div>
    </form>
</body>
</html>
