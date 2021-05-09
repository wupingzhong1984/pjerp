<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Device.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品资料信息</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="提交表单" runat="server" Size="Medium" Hidden="True" Icon="SystemSaveNew"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
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
                                <f:Label ID="lblFName" runat="server" Label="所属分类">
                                </f:Label>
                                <f:TextBox ID="txtFCode" runat="server" Label="编码" EmptyText="编码" Required="True"
                                    ShowRedStar="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFName" runat="server" Label="品名" MinLength="2" Required="True"
                                    ShowRedStar="True" EmptyText="品名">
                                </f:TextBox>
                                <f:TextBox ID="txtFSpec" runat="server" Label="规格" EmptyText="规格">
                                </f:TextBox>
                                <f:DropDownList runat="server" ID="ddlUnit" EnableEdit="true" Label="单位">
                                </f:DropDownList>
                                <f:NumberBox ID="txtFQty" runat="server" EmptyText="数量" Label="数量"
                                    DecimalPrecision="2" NoNegative="True" Required="True" ShowRedStar="True">
                                </f:NumberBox>
                                <f:TextBox runat="server" Label="客户代码" ID="txtFCCode" Readonly="True" Enabled="False" />
                                <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="left"
                                                            Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                        </f:TriggerBox>
                                <f:TriggerBox ID="txtFAddress" EnablePostBack="True"
                                    ShowLabel="true" Label="位置"
                                    Readonly="false" TriggerIcon="Search" runat="server">
                                </f:TriggerBox>
                                <f:DatePicker ID="txtFPurchaseDate" runat="server" Required="false" Label="购买日期" EmptyText="日期"
                                    ShowRedStar="false">
                                </f:DatePicker>
                                <f:DatePicker ID="txtFInstallDate" runat="server" Required="false" Label="安装日期" EmptyText="日期"
                                     ShowRedStar="false">
                                </f:DatePicker>
                                <f:DatePicker ID="DatePicker1" runat="server" Required="false" Label="安装日期" EmptyText="日期"
                                    ShowRedStar="false">
                                </f:DatePicker>
                                <f:NumberBox ID="txtFInspectionCycle" runat="server" EmptyText="检验周期" Label="检验周期"
                                    DecimalPrecision="2" NoNegative="True" Required="True" ShowRedStar="True">
                                </f:NumberBox>
                                <f:DropDownList runat="server" ID="ddlFInstallByName" EnableEdit="true" Label="安装人">
                                </f:DropDownList>
                                <f:DropDownList runat="server" ID="ddlStatus" EnableEdit="true" Label="设备装态">
                                </f:DropDownList>
                                <f:TextBox ID="txtFUser" runat="server" Label="使用者" EmptyText="使用者">
                                </f:TextBox>
                                <f:TextBox ID="txtFParms" runat="server" Label="其它参数" EmptyText="其它参数">
                                </f:TextBox>
                                <f:TextArea ID="txtFMemo" runat="server" Label="备注" EmptyText="备注">
                                </f:TextArea>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Panel>
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
