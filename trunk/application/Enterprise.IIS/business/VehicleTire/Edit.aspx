<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.VehicleTire.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>轮胎登记</title>
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

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow4_ddlDeliveryMethod-labelEl {
            color: red;
        }

        #RegionPanel1_Region1_header {
            background-color: #dfeaf2;
        }

        #component-1021 {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_Toolbar1_headerText-inputEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow0_ddlFK-labelEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_ddlSK-labelEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_txtFAccount-labelEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow2_lblFSK-inputEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow2_lblFEnd-inputEl {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="轮胎登记" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Center" Layout="Fit"
                                    BodyPadding="0px" runat="server" Height="96px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" BodyPadding="5px" ShowBorder="false" ShowHeader="False" LabelWidth="80px" LabelAlign="Right"
                                            runat="server">
                                            <Toolbars>

                                                <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                                                    <Items>
                                                        <f:Label runat="server" ID="headerText" Text="添加轮胎登记" />
                                                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" EmptyText="单据号" Readonly="True" Enabled="False" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" EmptyText="制单人" Readonly="True" LabelAlign="Right" Enabled="False">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="日期" EmptyText="日期"
                                                            ShowRedStar="false" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFireNum1" runat="server" Label="旧胎号" EmptyText="旧胎号"  LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtFireNum2" runat="server" Label="新胎号" EmptyText="新胎号"  LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:NumberBox runat="server" ID="txtFMileage" Label="公里数" Required="True" LabelAlign="Right" />
                                                        <f:TextBox ID="txtFSpec" runat="server" Label="型号" EmptyText="型号"  LabelAlign="Right" >
                                                        </f:TextBox>
                                                        <f:NumberBox runat="server" ID="txtFPrice" Label="单价" Required="True" LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>
                                                
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" LabelAlign="Right" />
                                                        <f:TextBox ID="txtFireNum3" runat="server" Label="目前轮胎号" EmptyText="目前轮胎号"  LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                
                                                 <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:TextBox runat="server" ID="txtFMemo" Label="摘要" LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>

                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>
