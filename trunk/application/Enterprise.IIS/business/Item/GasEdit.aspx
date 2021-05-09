<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasEdit.aspx.cs" Inherits="Enterprise.IIS.business.Item.GasEdit" %>


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
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
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
                    <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" LabelAlign="Right"
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                        <Items>
                            <f:Label ID="lblFName" runat="server" Label="所属分类">
                            </f:Label>
                            <f:TextBox ID="txtFCode" runat="server" Label="商品代码" EmptyText="商品代码" Required="True"
                                ShowRedStar="True">
                            </f:TextBox>
                            <f:TextBox ID="txtFName" runat="server" Label="商品名称" MinLength="2" Required="True"
                                ShowRedStar="True" EmptyText="商品名称">
                            </f:TextBox>
                            <f:TextBox ID="txtFGroupNum" runat="server" Label="产品组号" EmptyText="产品组号">
                            </f:TextBox>
                            <f:TextBox ID="txtFRack" runat="server" Label="库位货架" EmptyText="库位货架">
                                </f:TextBox>
                            <f:TextBox ID="txtFSpec" runat="server" Label="规格" EmptyText="规格">
                            </f:TextBox>
                            <f:TextBox ID="txtcinvdefine1" runat="server" Label="阀门" EmptyText="阀门">
                            </f:TextBox>
                            <f:TextBox ID="txtFQty" runat="server" Label="公斤" Text="0" EmptyText="公斤">
                            </f:TextBox>
                            <f:DropDownList runat="server" ID="ddlUnit" EnableEdit="true" Label="单位">
                            </f:DropDownList>
                            <f:NumberBox ID="txtFPurchasePrice" runat="server" EmptyText="采购价" Label="采购价" 
                                DecimalPrecision="2" NoNegative="True" Required="True" ShowRedStar="True" Text="0">
                            </f:NumberBox>
                            <f:NumberBox ID="txtFSalesPrice" runat="server" EmptyText="销售价" Label="销售价" 
                                DecimalPrecision="2" NoNegative="True" Required="True" ShowRedStar="True" Text="0">
                            </f:NumberBox>
                            <f:NumberBox ID="txtFPieceWork1" runat="server" EmptyText="劳务单价" Label="劳务单价" 
                                DecimalPrecision="2" NoNegative="True" Text="0">
                            </f:NumberBox>
                            <f:DropDownList runat="server" ID="ddlBottleNum" EnableEdit="true" Label="包装物">
                            </f:DropDownList>
                            <f:DropDownList runat="server" ID="ddlFIsLiquid" EnableEdit="False" Label="商品性质">
                                 <f:ListItem Text="瓶装气" Value="瓶装气" Selected="true" />
                                 <f:ListItem Text="槽车液体" Value="槽车液体" />
                                 <f:ListItem Text="排管车" Value="排管车" />
                            </f:DropDownList>
                            <f:TextArea ID="txtFMemo" runat="server" Label="备注" EmptyText="备注">
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
