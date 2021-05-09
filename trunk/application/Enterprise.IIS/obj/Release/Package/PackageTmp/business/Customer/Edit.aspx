<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Customer.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="TabStrip1" runat="server" />
        <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
        <f:TabStrip ID="TabStrip1" Width="850px" Height="350px" BodyPadding="0px" TabPosition="Left"
            AutoPostBack="true" OnTabIndexChanged="TabStrip1_TabIndexChanged"
            ShowBorder="false" ActiveTabIndex="0" runat="server">
            <Tabs>
                <f:Tab Title="基本信息" BodyPadding="0px"
                    Layout="Fit" runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                    OnClick="btnSubmit_Click" Size="Medium">
                                </f:Button>

                                <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Panel ID="Panel1" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True" BodyPadding="0px">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                                    runat="server" LabelAlign="Right" BodyPadding="5px">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlDistric" Label="所属区域"
                                                    EnableEdit="True" DataTextField="FKey" DataValueField="FValue" LabelAlign="Right">
                                                </f:DropDownList>
                                                <f:TextBox ID="txtFCode" runat="server" Label="客户代码" EmptyText="客户代码" Required="True"
                                                    ShowRedStar="True" LabelAlign="Right">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:TextBox ID="txtFName" runat="server" Label="客户名称" MinLength="1" Required="True"
                                                    ShowRedStar="True" EmptyText="客户名称" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                                <f:TextBox ID="txtFAddress" runat="server" Label="地址" MinLength="1" Required="False"
                                                    ShowRedStar="False" EmptyText="地址" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow3">
                                            <Items>
                                                <f:TextBox ID="txtFLinkman" runat="server" Label="联系人" MinLength="1" Required="False"
                                                    ShowRedStar="False" EmptyText="联系人" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                                <f:DatePicker ID="txtFDate" runat="server" Label="建档日期" EmptyText="建档日期" Required="False"
                                                    ShowRedStar="False" LabelAlign="Right">
                                                </f:DatePicker>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow4">
                                            <Items>
                                                <f:TextBox ID="txtFPhome" runat="server" Label="电话" MinLength="1" Required="False"
                                                    ShowRedStar="False" EmptyText="电话" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                                <f:TextBox ID="txtFMoile" runat="server" Label="手机" MinLength="1" Required="False"
                                                    ShowRedStar="False" EmptyText="手机" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow5">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlFIsPrint" Label="是否发票"
                                                    AutoPostBack="False" DataTextField="key" Required="True"
                                                    DataValueField="value" ShowRedStar="True" LabelAlign="Right">
                                                </f:DropDownList>
                                                <f:NumberBox Label="信用天数" ID="txtFTipsDay" runat="server" LabelAlign="Right" EmptyText="信用天数" Text="0" Required="True" ShowRedStar="True" />
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow6">
                                            <Items>
                                                <f:NumberBox Label="运输服务费" LabelAlign="Right" ID="txtFFreight" runat="server" EmptyText="运输服务费" Text="0" Required="False" ShowRedStar="False" />
                                                <f:DropDownList runat="server" ID="ddlFPaymentMethod" LabelAlign="Right" Label="结算方式" Required="True" ShowRedStar="True" DataTextField="FKey" DataValueField="FValue">
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow8">
                                            <Items>
                                                <f:NumberBox Label="信用额度" LabelAlign="Right" ID="txtFCredit" runat="server" EmptyText="信用额度" Text="0" Required="False" ShowRedStar="False" />
                                                <f:DropDownList runat="server" ID="ddlFSalesman" LabelAlign="Right" Label="业务员" Required="False" ShowRedStar="False" DataTextField="name" DataValueField="name">
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow9">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlFRebateFlag" LabelAlign="Right" Label="是否回扣" Required="False" ShowRedStar="False" DataTextField="name" DataValueField="name">
                                                    <f:ListItem Text="是" Value="是" Selected="true" />
                                                    <f:ListItem Text="否" Value="否" />
                                                </f:DropDownList>
                                                <f:DropDownList runat="server" ID="ddlFPushFlag" LabelAlign="Right" Label="打印单价" Required="False" ShowRedStar="False" DataTextField="name" DataValueField="name">
                                                    <f:ListItem Text="是" Value="是" Selected="true" />
                                                    <f:ListItem Text="否" Value="否" />
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow10">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlFMonthly" Label="财务月结" LabelAlign="Right" AutoPostBack="true">
                                                    <f:ListItem Text="自然月" Value="自然月" />
                                                    <f:ListItem Text="非自然月" Value="非自然月" />
                                                </f:DropDownList>
                                                <f:NumberBox runat="server" ID="txtFMonthlyDay" Label="月结日期" LabelAlign="Right" Text="25" />
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow12">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlFGroupNoFlag" LabelAlign="Right" Label="结算主号标识"
                                                    Required="True" ShowRedStar="False" DataTextField="name" DataValueField="name">
                                                    <f:ListItem Text="是" Value="是" Selected="true" />
                                                    <f:ListItem Text="否" Value="否" />
                                                </f:DropDownList>
                                                <f:TextBox ID="txtFGroupNo" runat="server" Label="结算主号" MinLength="1" Required="True"
                                                    ShowRedStar="True" EmptyText="结算主号" AutoPostBack="True" LabelAlign="Right">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow13">
                                            <Items>
                                                <f:DropDownList ID="ddlFProvince" OnSelectedIndexChanged="ddlFProvince_SelectedIndexChanged" AutoPostBack="true" runat="server" Label="省" >
                                                </f:DropDownList>
                                                <f:DropDownList ID="ddlFCity" OnSelectedIndexChanged="ddlFCity_SelectedIndexChanged" AutoPostBack="true" runat="server" Label="市" >
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow14">
                                            <Items>
                                                <f:DropDownList ID="ddlFCounty" runat="server" Label="县" >
                                                </f:DropDownList>
                                                <f:TextBox ID="txtFZipCode" runat="server" Label="邮政编码" Required="False"
                                                    ShowRedStar="False" EmptyText="邮政编码" LabelAlign="Right">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow15">
                                            <Items>
                                                <f:TextBox ID="txtFCoordinate" runat="server" Label="地图坐标" 
                                                    EmptyText="地图坐标" LabelAlign="Right">
                                                </f:TextBox>
                                                <f:Label Hidden="true" ID="Label" runat="server"></f:Label>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow16">
                                            <Items>
                                                <f:TextArea Label="备注" ID="txtFMemo" LabelAlign="Right" runat="server" EmptyText="备注" />
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Tab>
                <f:Tab Title="部门" BodyPadding="0px"
                    Layout="Fit" runat="server">
                    <Items>
                        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region1" Title="设置部门" Position="Center"
                                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                                    <Toolbars>
                                        <f:Toolbar ID="Toolbar2" runat="server">
                                            <Items>
                                                <f:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="新增" Icon="Add" Hidden="False" Size="Medium">
                                                </f:Button>
                                                <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="False" Text="编辑"
                                                    Icon="PageWhiteEdit" Size="Medium">
                                                </f:Button>
                                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="删除"
                                                    Icon="Delete" Hidden="False" Size="Medium">
                                                </f:Button>
                                                <f:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" Icon="PageSave" Hidden="False" Size="Medium">
                                                </f:Button>
                                                <f:Button ID="btnClose2" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                                                </f:Button>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Items>
                                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                                            <Regions>
                                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                                    Position="Top" Layout="Fit"
                                                    BodyPadding="0px" runat="server" Height="68px" EnableCollapse="True">
                                                    <Items>
                                                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                                            <Rows>
                                                                <f:FormRow ID="FormRow111">
                                                                    <Items>
                                                                        <f:TextBox ID="tbxFName" runat="server" Label="子级名称">
                                                                        </f:TextBox>
                                                                        <f:TextBox ID="tbxFFLinkman" runat="server" Label="联系人">
                                                                        </f:TextBox>
                                                                        <f:TextBox ID="tbxFPhone" runat="server" Label="电话 ">
                                                                        </f:TextBox>
                                                                    </Items>
                                                                </f:FormRow>
                                                                <f:FormRow ID="FormRow112">
                                                                    <Items>
                                                                        <f:TextBox ID="tbxFAddress" runat="server" Label="地址">
                                                                        </f:TextBox>
                                                                        <f:TextBox ID="tbxFMemo" runat="server" Label="备注">
                                                                        </f:TextBox>
                                                                        <f:TextBox ID="tbxFId" runat="server" Label="内码" Hidden="true">
                                                                        </f:TextBox>
                                                                    </Items>
                                                                </f:FormRow>
                                                            </Rows>
                                                        </f:Form>
                                                    </Items>
                                                </f:Region>
                                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                                    Layout="Fit" runat="server" BodyPadding="0px" AutoScroll="True" MinHeight="220px">
                                                    <Items>
                                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId"
                                                            IsDatabasePaging="true" SortDirection="DESC"
                                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="False" KeepCurrentSelection="True"
                                                            EnableRowClickEvent="true">
                                                            <Columns>
                                                                <f:BoundField MinWidth="260px" DataField="FName" HeaderText="部门名称" SortField="FName" TextAlign="Left" runat="server" />
                                                                <f:BoundField MinWidth="120px" DataField="FFLinkman" HeaderText="联系人" SortField="FFLinkman" />
                                                                <f:BoundField MinWidth="80px" DataField="FPhone" HeaderText="电话" SortField="FPhone" />
                                                                <f:BoundField MinWidth="260px" DataField="FAddress" HeaderText="地址" SortField="FAddress" />
                                                                <f:BoundField MinWidth="320px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Region>
                                            </Regions>
                                        </f:RegionPanel>
                                    </Items>
                                </f:Region>

                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Tab>


            </Tabs>
        </f:TabStrip>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>
