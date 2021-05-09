<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Divisor.aspx.cs" Inherits="Enterprise.IIS.business.Factor.Divisor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>压缩因子</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .x-grid-row-summary .x-grid-cell {
            background-color: #dfeaf2;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_tbxFCustomer-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_ddlFBill-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow4_ddlDeliveryMethod-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow4_ddlFDistributionPoint-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_header {
            background-color: #dfeaf2;
        }
        #component-1021 {
            background-color: #dfeaf2;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hfdCode"/>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="False" ShowHeader="False" Title="引入钢瓶档案" Position="Center"
                    Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnClose" Text="关闭"
                                    runat="server" Icon="SystemClose" Size="Medium" OnClick="btnClose_OnClick">
                                </f:Button>
                                <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                    OnClick="btnSubmit_Click" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="3px" runat="server">
                                    <Items>
                                        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                                            runat="server" ShowBorder="false" ShowHeader="False"
                                            AutoScroll="true">
                                            <Items>
                                                <f:GroupPanel runat="server" Title="模版下载" ID="GPanelRoleInfo" EnableCollapse="True">
                                                    <Items>
                                                        <f:Label ID="Label1" runat="server" Label="模板说明" Text="模板说明">
                                                        </f:Label>
                                                        <f:ContentPanel ID="ContentPanel1" ShowBorder="false" BodyPadding="10px" ShowHeader="false"
                                                            AutoScroll="true" CssClass="intro" runat="server">
                                                            <a href="~/business/template/压缩因子.xls" runat="server">下载模版</a>
                                                        </f:ContentPanel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:GroupPanel runat="server" Title="功能授权" ID="GPanelActions" EnableCollapse="True">
                                                    <Items>
                                                        <f:Panel ID="PanelActions" ShowHeader="false" Title="ICON"
                                                            ShowBorder="false" runat="server">
                                                            <Items>
                                                                <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Label="客户代码" 
                                                            Readonly="false" TriggerIcon="Search" runat="server"  AutoPostBack="True">
                                                        </f:TriggerBox>
                                                                <f:FileUpload runat="server" ID="fileUpload1" ShowRedStar="false" ButtonText="上传数据"
                                                                    ButtonOnly="true" Required="false" ShowLabel="true" Label="上传数据" AutoPostBack="true"
                                                                    OnFileSelected="fileUpload1_FileSelected">
                                                                </f:FileUpload>
                                                            </Items>
                                                        </f:Panel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:Grid ID="Grid1" Title="引入压缩因子" ShowBorder="false" ShowHeader="False"
                                                    runat="server" EnableCheckBoxSelect="True" DataKeyNames="id" AllowPaging="False"
                                                    IsDatabasePaging="true" AllowSorting="False" EmptyText="查询无结果"
                                                    EnableHeaderMenu="True">
                                                    <Columns>
                                                        <f:BoundField Width="100px" DataField="压力" HeaderText="压力" SortField="压力" />
                                                        <f:BoundField Width="100px" DataField="温度" HeaderText="温度" SortField="温度" />
                                                        <f:BoundField Width="100px" DataField="因子系数" HeaderText="因子系数" SortField="因子系数" />
                                                    </Columns>
                                                </f:Grid>
                                            </Items>
                                        </f:SimpleForm>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>

         <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="800px" OnClose="Window1_Close">
        </f:Window>
    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
