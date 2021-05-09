<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Address.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
        <f:Panel ID="Panel1" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                            OnClick="btnSubmit_Click" Size="Medium">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px" BodyPadding="3px" LabelAlign="Right"
                    runat="server">
                    <Rows>
                        <f:FormRow ID="FormRow1">
                            <Items>
                                <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Required="True" LabelAlign="Right" />
                                <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                    ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="Right"
                                    Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2">
                            <Items>
                                <f:TextBox ID="txtFLinkman" runat="server" Label="联系人" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="联系人" AutoPostBack="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFAddress" runat="server" Label="邮寄地址" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="邮寄地址" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3">
                            <Items>
                                <f:TextBox ID="txtFPhome" runat="server" Label="电话" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="电话" AutoPostBack="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFMoile" runat="server" Label="手机" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="手机" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4">
                            <Items>
                                 <f:TextBox ID="txtFZip" runat="server" Label="邮编" MinLength="1" Required="True"
                                    ShowRedStar="True" EmptyText="邮编" AutoPostBack="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFCity" runat="server" Label="寄件城市" MinLength="1" Required="True"
                                    ShowRedStar="True" EmptyText="寄件城市" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow9">
                            <Items>
                                <f:TextArea Label="备注" ID="txtFMemo" runat="server" EmptyText="备注" />
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="800px" OnClose="Window1_Close">
        </f:Window>
    </form>
    
</body>
</html>
