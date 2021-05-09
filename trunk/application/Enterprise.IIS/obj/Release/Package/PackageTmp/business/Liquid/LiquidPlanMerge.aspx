<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiquidPlanMerge.aspx.cs" Inherits="Enterprise.IIS.business.Liquid.LiquidPlanMerge" %>

<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="0px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="False" Icon="SystemSaveNew" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="基本信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="100px"
                                    runat="server">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:Label ID="txtKeyId" runat="server" LabelAlign="Right" Label="单据号" />
                                                <f:DatePicker ID="txtFDate"  Required="True" ShowRedStar="True" runat="server" LabelAlign="Right" Label="日期" />
                                                <f:DropDownList ID="tbxFItemName" runat="server" EnableEdit="True"
                                                    LabelAlign="Right" Label="商品名称" Required="True" ShowRedStar="True">
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:DropDownList ID="tbxFBill" runat="server" LabelAlign="Right" Label="业务类型">
                                                    <f:ListItem Text="进销" Value="进销" Selected="True" />
                                                    <f:ListItem Text="自销" Value="自销" />
                                                    <f:ListItem Text="代运" Value="代运" />
                                                </f:DropDownList>

                                                <f:DropDownList  Required="True" ShowRedStar="True" ID="tbxFVehicleNum" runat="server" LabelAlign="Right" Label="车牌号" AutoPostBack="true" OnSelectedIndexChanged="tbxFVehicleNum_SelectedIndexChanged">
                                                </f:DropDownList>

                                                <f:DropDownList ID="tbxFDriver" EnableEdit="True" EnableMultiSelect="true"
                                                    runat="server" LabelAlign="Right" Label="司机">
                                                </f:DropDownList>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow3">
                                            <Items>
                                                <f:DropDownList ID="tbxFSupercargo" runat="server" LabelAlign="Right" Label="押运员">
                                                </f:DropDownList>

                                                <f:NumberBox ID="tbxFMargin"  Required="True" ShowRedStar="True" runat="server" LabelAlign="Right" Label="装车前余量" Text="0">
                                                </f:NumberBox>

                                                <f:TextBox ID="txtFBeginAddress" LabelAlign="Right" Label="起点" runat="server" />
                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow70">
                                            <Items>

                                                <f:TextBox ID="txtFEndAddress" LabelAlign="Right" Label="终点" runat="server" />

                                                <f:NumberBox ID="txtFBeginMileage" runat="server" LabelAlign="Right" Label="起始里程数" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFEndMileage" runat="server" LabelAlign="Right" Label="结束里程数" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow71">
                                            <Items>

                                                <f:NumberBox ID="txtFQty" runat="server" LabelAlign="Right" Label="加油量" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFPrice" runat="server" LabelAlign="Right" Label="单价" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFAmount" runat="server" LabelAlign="Right" Label="金额" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow72">
                                            <Items>
                                                <f:NumberBox ID="txtFOtherAmount" runat="server" LabelAlign="Right" Label="其它费用" Text="0">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtFMileage" runat="server" LabelAlign="Right" Label="总公里数" Text="0">
                                                </f:NumberBox>
                                                <f:Label runat="server" Hidden="True" />
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow4">
                                            <Items>

                                                <f:TextArea ID="txtFMemo" LabelAlign="Right" Label="备注"
                                                    EmptyText="备注" runat="server">
                                                </f:TextArea>

                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab2" Title="采购订单" BodyPadding="1px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                    runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId"
                                    IsDatabasePaging="False" EnableMultiSelect="False"
                                    AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                    <Columns>
                                        <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                        <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="配送日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                        <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                        <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                        <f:BoundField MinWidth="60px" DataField="FQty" HeaderText="计划量" SortField="FQty" TextAlign="right" />
                                        <f:BoundField MinWidth="60px" DataField="FPrice" HeaderText="单价" SortField="FPrice" TextAlign="right" />
                                        <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="right" />
                                        
                                        <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                        <f:BoundField MinWidth="120px" DataField="FFreight" HeaderText="运输服务费" SortField="FFreight" TextAlign="Right" />
                                        <f:BoundField MinWidth="60px" DataField="FDeliveryMethod" HeaderText="配送方式" SortField="FDeliveryMethod" />
                                        <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FShipper" HeaderText="发货人" SortField="FShipper" TextAlign="Center" />
                                        <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" TextAlign="Center" />
                                        
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Tab>

                        <f:Tab ID="Tab3" Title="销售订单" BodyPadding="1px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Grid ID="Grid2" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                    runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId"
                                    IsDatabasePaging="False" EnableMultiSelect="True"
                                    AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                    <Columns>
                                        <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                        <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="配送日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                        <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                        <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                        <f:BoundField MinWidth="60px" DataField="FQty" HeaderText="计划量" SortField="FQty" TextAlign="right" />
                                        <f:BoundField MinWidth="60px" DataField="FPrice" HeaderText="单价" SortField="FPrice" TextAlign="right" />
                                        <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="right" />
                                        <f:BoundField MinWidth="120px" DataField="FFreight" HeaderText="运输服务费" SortField="FFreight" TextAlign="Right" />
                                        <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                        <f:BoundField MinWidth="60px" DataField="FDeliveryMethod" HeaderText="配送方式" SortField="FDeliveryMethod" />
                                        <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" TextAlign="Center" />
                                        <f:BoundField MinWidth="120px" DataField="FShipper" HeaderText="发货人" SortField="FShipper" TextAlign="Center" />
                                        <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" TextAlign="Center" />
                                        
                                    </Columns>
                                </f:Grid>
                            </Items>
                            </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>

        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择供货商列表"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window2_Close">
        </f:Window>

        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择客户列表"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window2_Close">
        </f:Window>

    </form>

    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script type="text/javascript" src="../../jqueryui/js/jquery-1.8.3.min.js"> </script>

    <script type="text/javascript" src="../../res/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0"> </script>
    <script type="text/javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var inputselector = '.f-grid-tpl input';

        F.ready(function () {


        });


    </script>
</body>
</html>
