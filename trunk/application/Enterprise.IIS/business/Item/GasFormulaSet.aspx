<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasFormulaSet.aspx.cs" Inherits="Enterprise.IIS.business.Item.GasFormulaSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品配方设置</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl04-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl05-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl06-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl07-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl08-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl14-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_Toolbar1_headerText-inputEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="产品配方配置" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Label runat="server" ID="headerText" Text="产品配方配置" />
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAdd" runat="server" Text="添加配方" Icon="Add" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Hidden="False" Text="保存配方"
                                    Icon="PageSave" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnPrint_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="False" DisableControlBeforePostBack="False" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="45px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFCode" runat="server" Label="商品代码">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFSpell" runat="server" Label="助记码">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center" RegionSplit="False"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True" Split="False">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FCode" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="False"
                                            EnableRowSelectEvent="true" OnRowSelect="Grid1_RowSelect" KeepCurrentSelection="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="商品代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="商品名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FSpell" HeaderText="助记码" SortField="FSpell" />
                                                <f:BoundField MinWidth="120px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="120px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:CheckBoxField MinWidth="100px" RenderAsStaticField="true" DataField="FFlag"
                                                    TextAlign="Center" HeaderText="是否启用" SortField="FFlag" />
                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="100" Value="100" />
                                                    <f:ListItem Text="200" Value="200" Selected="True" />
                                                    <f:ListItem Text="300" Value="300" />
                                                </f:DropDownList>
                                            </PageItems>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>

                <f:Region ID="Region2" ShowHeader="False" Split="False" Title="装车明细" Icon="ApplicationViewDetail"
                    Width="260px" Position="Bottom" Height="160px" Layout="Fit" runat="server" EnableCollapse="True">
                    <Items>
                        <f:Grid ID="Grid2" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId"
                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC"
                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" OnRowCommand="Grid2_RowCommand"
                            AllowCellEditing="true" ClicksToEdit="1" EnableAfterEditEvent="true" OnAfterEdit="Grid2_AfterEdit">
                            <Columns>
                                <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                                    ColumnID="colDelete" Width="45px" CommandName="Delete" Hidden="False" SortField="mc">
                                </f:LinkButtonField>

                                <f:RenderField MinWidth="100px" ColumnID="FItemCode" DataField="FItemCode" FieldType="String"
                                    HeaderText="原料编码">
                                    <Editor>
                                        <f:TriggerBox ID="tbxFItemCode" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                        </f:TriggerBox>
                                    </Editor>
                                </f:RenderField>

                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="原料名称" SortField="FItemName" runat="server" />
                                <f:RenderField MinWidth="100px" ColumnID="FQty" DataField="FQty" FieldType="Double" TextAlign="Right"
                                    HeaderText="数量">
                                    <Editor>
                                        <f:TextBox ID="tbxFQty" Required="true" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" runat="server" />
                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" runat="server" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="True" EnableClose="True"
            Icon="ApplicationViewDetail" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="680px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>

        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="Popup Window 2" CloseAction="HidePostBack"
            EnableIFrame="true" Height="300px" Width="360px">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        var gridClientID = '<%= Grid1.ClientID %>';
        var inputselector = '.f-grid-tpl input';

        F.ready(function () {

        });

        function reloadGrid(keys) {
            __doPostBack(null, 'reloadGrid:' + keys);
        };

        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>
