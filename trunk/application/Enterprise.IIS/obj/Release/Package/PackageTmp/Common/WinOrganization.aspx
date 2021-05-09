<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinOrganization.aspx.cs" Inherits="Enterprise.IIS.Common.WinOrganization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择调拨部门</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-grid-tpl .others input
        {
            vertical-align: middle;
        }
        .x-grid-tpl .others label
        {
            margin-left: 5px;
            margin-right: 15px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="选择调拨部门" ShowBorder="false" ShowHeader="False" Position="Center"
                Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                            </f:Button>
                            <f:Button ID="btnSubmit" Icon="PageSave" Size="Medium" OnClick="btnSava_Click"
                                Text="提交表单" runat="server">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="False" ShowHeader="false" Position="Center"
                                Layout="Fit" runat="server" BodyPadding="3px">
                                <Items>
                                    <f:Tree ID="trDept" Width="60px" ShowHeader="False" ShowBorder="false" Icon="House" OnNodeCommand="Tree1_NodeCommand"
                                        EnableMultiSelect="False" Title="组织机构" runat="server" AutoScroll="True">
                                    </f:Tree>
                                </Items>
                            </f:Region>
                        </Regions>
                    </f:RegionPanel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <br />
    </form>
</body>
</html>
