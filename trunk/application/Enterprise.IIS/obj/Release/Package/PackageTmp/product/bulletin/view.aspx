<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.view" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>公告详情</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .mright {
            margin-right: 5px;
        }

        .datecontainer .x-form-field-trigger-wrap {
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Top" ToolbarAlign="Left">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" OnClientClick="closeActiveTab();" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
                            AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                            <Items>
                                <f:Label ID="lbltitle" runat="server" Label="标题">
                                </f:Label>
                                <f:Label ID="lblvalidity_s" runat="server" Label="有效开始日期">
                                </f:Label>
                                <f:Label ID="lblvalidity_e" runat="server" Label="有效结束日期">
                                </f:Label>
                                <f:Label ID="lblm_content" runat="server" Label="公告内容">
                                </f:Label>
                                <f:Label ID="lblfile" Label="附件" EncodeText="false" runat="server"></f:Label>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
