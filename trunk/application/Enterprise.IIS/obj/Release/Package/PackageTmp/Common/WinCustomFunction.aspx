<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinCustomFunction.aspx.cs" Inherits="Enterprise.IIS.Common.WinCustomFunction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>常用功能</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alink a, a:hover, a:visited {
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="False"
                    Width="200px" Position="Center" Layout="Fit"
                    runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" Size="Medium"
                                    OnClick="btnSubmit_Click" ConfirmText="确定要保存吗？">
                                </f:Button>
                                <f:Button ID="btnResult" Text="重新定义常用功能" runat="server" Icon="PageRefresh" Size="Medium"
                                    OnClick="btnResult_Click" ConfirmText="确定要重新定义常用功能吗？">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Tree ID="leftMenuTree" ShowHeader="False"
                            ShowBorder="True" Icon="House" Title="常用功能" EnableAjax="True" runat="server"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
    </form>
</body>
</html>
