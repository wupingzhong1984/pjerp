<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs"
    Inherits="Enterprise.IIS.product.account.changepassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" Text="关闭" runat="server" Icon="SystemClose" OnClick="btnClose_OnClick" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1" Size="Medium"
                        OnClick="btnSave_OnClick">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <f:SimpleForm ID="SimpleForm1" runat="server" LabelWidth="100px" BodyPadding="5px"
                        ShowBorder="false" ShowHeader="false" Width="350px">
                        <Items>
                            <f:TextBox ID="tbxOldPassword" TextMode="Password" runat="server" Label="当前密码" Required="true"
                                ShowRedStar="true"  LabelAlign="Right">
                            </f:TextBox>
                            <f:TextBox ID="tbxNewPassword" TextMode="Password" runat="server" Label="新密码" Required="true"
                                ShowRedStar="true"  LabelAlign="Right">
                            </f:TextBox>
                            <f:TextBox ID="tbxConfirmNewPassword" TextMode="Password" runat="server" Label="确认新密码"
                                Required="true" ShowRedStar="true" CompareControl="tbxNewPassword"  LabelAlign="Right">
                            </f:TextBox>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>