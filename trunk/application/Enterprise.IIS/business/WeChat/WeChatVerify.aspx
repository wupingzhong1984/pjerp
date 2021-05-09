<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeChatVerify.aspx.cs" Inherits="Enterprise.IIS.business.WeChat.WeChatVerify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hdfImage"/>
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False"
                Width="200px" Position="Left" Layout="Fit" runat="server" EnableCollapse="true" >
                <Items>
                    <f:Tree ID="trMenu" Width="50px" OnNodeCommand="trMenu_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Title="企业微信通讯录" runat="server" AutoScroll="True" >
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Title="企业微信通讯录" Position="Center" ShowBorder="False"
                BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                <Items>
                    <f:SimpleForm ID="SimpleFormDept" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                        runat="server" ShowBorder="false" ShowHeader="False"
                        AutoScroll="true" LabelAlign="Right">
                        
                    </f:SimpleForm>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
</body>
</html>
