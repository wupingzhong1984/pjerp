<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SynergicData.aspx.cs" Inherits="Enterprise.IIS.SynergicData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #Panel1_Toolbar1_headerText-inputEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />

        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px" AutoScroll="True" Width="850px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Top" ToolbarAlign="Left">
                    <Items>
                        <f:Label runat="server" ID="headerText" Text="数据接口"/>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                            runat="server" AutoScroll="True" RegionPosition="Top">
                            <Rows>
                                <f:FormRow ID="FormRow1">
                                    <Items>
                                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                            <font color="red">基础档案</font>
                                            <hr />
                                        </f:ContentPanel>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow2" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnDepartment" OnClick="btnDepartment_OnClick" Icon="Note" Text="部门档案" Size="Large" runat="server" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnEmployee" OnClick="btnEmployee_OnClick" Icon="Note" Text="人事档案" Size="Large" runat="server" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnSupplierType" OnClick="btnSupplierType_OnClick" Icon="Note" Text="供应商分类" Size="Large" runat="server" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnSupplier" OnClick="btnSupplier_OnClick" Icon="Note" Text="供应商" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                    </Items>
                                </f:FormRow>

                                <f:FormRow ID="FormRow3" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnCustomerType" OnClick="btnCustomerType_OnClick" Icon="Note" Text="客户分类" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnCustomer" OnClick="btnCustomer_OnClick" Icon="Note" Text="客户档案" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnItemType" OnClick="btnItemType_OnClick" Icon="Note" Text="存货分类" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnItem" OnClick="btnItem_OnClick" Icon="Note" Text="存货档案" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>

                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnUnit" OnClick="btnUnit_OnClick" Icon="Note" Text="计量单位" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnWarehouse" OnClick="btnWarehouse_OnClick" Icon="Note" Text="仓库档案" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnReceive" OnClick="btnReceive_OnClick" Icon="Note" Text="收发类型" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnPurchase" OnClick="btnPurchase_OnClick" Icon="Note" Text="采购类型" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnSales" OnClick="btnSales_OnClick" Icon="Note" Text="销售类型" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnCurrency" OnClick="btnCurrency_OnClick" Icon="Note" Text="币种" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Label runat="server"/>
                                        <f:Label runat="server"/>
                                    </Items>
                                </f:FormRow>
                                
                                <f:FormRow ID="FormRow6" BoxConfigPadding="1px">
                                    <Items>
                                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                            <font color="red">业务单据</font>
                                            <hr />
                                        </f:ContentPanel>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow7" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnSalesOrder" OnClick="btnSalesOrder_OnClick" Icon="Note" Text="销售订单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnSalesNote" OnClick="btnSalesNote_OnClick" Icon="Note" Text="销售出库单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnSalesReturn" OnClick="btnSalesReturn_OnClick" Icon="Note" Text="销售退货单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Label runat="server" Hidden="True"/>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow8" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnPurchaseOrder" OnClick="btnPurchaseOrder_OnClick" Icon="Note" Text="采购订单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnPurchaseNote" OnClick="btnPurchaseNote_OnClick" Icon="Note" Text="采购入库单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnbtnPurchaseReturn" OnClick="btnbtnPurchaseReturn_OnClick" Icon="Note" Text="采购退货单" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Label runat="server" Hidden="True"/>
                                    </Items>
                                </f:FormRow>
                                
                                <f:FormRow ID="FormRow9" BoxConfigPadding="1px">
                                    <Items>
                                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                            <font color="red">业务查询</font>
                                            <hr />
                                        </f:ContentPanel>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow10" BoxConfigPadding="1px">
                                    <Items>
                                        <f:Button ID="btnAR" OnClick="btnAR_OnClick" Icon="Note" Text="查看应收款" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnAP" OnClick="btnAP_OnClick" Icon="Note" Text="查看应付款" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnARInvoice" OnClick="btnARInvoice_Click" Icon="Note" Text="查看销售发票" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        <f:Button ID="btnAPInvoice" Icon="Note" Text="查看采购发票" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>
                                        
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow11" BoxConfigPadding="1px" Hidden="true">
                                    <Items>
                                        <f:TextBox ID="txtFCustomer"  runat="server" Label="客户编码（T6）"></f:TextBox>
                                        <f:Button ID="btnFDataCustomer" OnClick="btnFDataCustomer_Click" Icon="Note" Text="同步机构" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>

                                        <f:TextBox ID="txtFSupplier"  runat="server" Label="供应商编码（T6）"></f:TextBox>
                                        <f:Button ID="btnDataFSupplier" OnClick="btnDataFSupplier_Click" Icon="Note" Text="同步机构" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>

                                    </Items>
                                </f:FormRow>

                                    <f:FormRow ID="FormRow12" BoxConfigPadding="1px">
                                    <Items>
                                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                            <font color="red">业务变更</font>
                                            <hr />
                                        </f:ContentPanel>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow13" BoxConfigPadding="1px">
                                    <Items>
                                        <f:TextBox ID="txtFItemCode"  runat="server" Label="存货代码(T6)"></f:TextBox>
                                        <f:TextBox ID="txtFItemName"  runat="server" Label="存货名称(T6)"></f:TextBox>
                                        <f:Button ID="btnItemName" OnClick="btnItemName_Click" Icon="Note" Text="同步更新" runat="server" Size="Large" BoxConfigPadding="2px"></f:Button>

                                        <f:Label runat="server" Hidden="true"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>

    <script src="jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        };

    </script>
</body>
</html>
