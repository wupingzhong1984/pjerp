<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Monthly.Index" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>财务月结</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="3px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Top" ToolbarAlign="Left">
                    <Items>
                        <f:Button ID="btnSubmit" Text="月结" runat="server" Hidden="False" Icon="SystemSaveNew" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                        </f:Button>
                        <f:Button ID="btnMonthR" Text="反月结" runat="server" Hidden="False" Icon="RecordRed" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnMonthR_Click">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>

            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                    runat="server">
                    <Rows>
                        <f:FormRow ID="FormRow1">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                <f:DatePicker ID="dpkFMonth" runat="server" Label="月结月份" DateFormatString="yyyy-MM" LabelAlign="Right">
                                </f:DatePicker>
                                <f:Label runat="server" Hidden="True" />
                                <f:Label runat="server" Hidden="True" />
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        };

    </script>
</body>
</html>
