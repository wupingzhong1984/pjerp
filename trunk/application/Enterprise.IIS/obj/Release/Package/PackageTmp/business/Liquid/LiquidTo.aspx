<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiquidTo.aspx.cs" Inherits="Enterprise.IIS.business.Liquid.LiquidTo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>导气</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Size="Medium" Hidden="False" Icon="SystemSaveNew"
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
                            
                            <f:TextBox ID="txtKeyId" runat="server" Label="单据号" Readonly="True" LabelAlign="Right">
                            </f:TextBox>

                            <f:NumberBox ID="txtFMarginEnd" runat="server" Label="余量" Readonly="True" LabelAlign="Right"
                                DecimalPrecision="2" >
                            </f:NumberBox>
                            
                            <f:DropDownList runat="server" ID="ddlBill" EnableEdit="true" 
                                Label="导气业务" LabelAlign="Right"  AutoPostBack="true" OnSelectedIndexChanged="ddlBill_SelectedIndexChanged">
                                <f:ListItem Text="导车" Value="导车" />
                                <f:ListItem Text="导储罐" Value="导储罐" />
                                <f:ListItem Text="放气" Value="放气" />
                            </f:DropDownList>
                            
                            <f:DropDownList ID="tbxFVehicleNum"  runat="server" Label="车牌号" LabelAlign="Right" Hidden="False">
                            </f:DropDownList>
                            
                            <f:DropDownList ID="tbxFItemName"  runat="server"  Label="导入产品" Required="True" ShowRedStar="True"  LabelAlign="Right" EnableEdit="False">
                            </f:DropDownList>

                            <f:NumberBox ID="txtFQty" runat="server" Label="本次导气量" 
                                DecimalPrecision="2" Required="True" ShowRedStar="True" LabelAlign="Right">
                            </f:NumberBox>

                            <f:TextArea ID="txtFMemo" runat="server" Label="备注" EmptyText="备注" LabelAlign="Right">
                            </f:TextArea>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
