<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_orgnization_list.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.select_orgnization_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="选择部门" ShowBorder="false" ShowHeader="False" Position="Center"
                Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnEdit" OnClick="btnEdit_Click" Size="Medium" runat="server" Text="确认提交"
                                Icon="PageSave">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Icon="House" Title="组织机构" runat="server" EnableMultiSelect="True"
                        EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True" OnNodeCheck="trDept_NodeCheck">
                    </f:Tree>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
</body>
</html>
