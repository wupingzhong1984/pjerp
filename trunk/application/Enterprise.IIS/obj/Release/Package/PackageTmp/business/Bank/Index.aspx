<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Bank.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>银行账户</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="银行账户" Position="Center" ShowHeader="False"
                    BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowBorder="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>

                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" Size="Medium" OnClick="btnSearch_Click">
                                </f:Button>

                                <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                                    Icon="PageWhiteEdit" Size="Medium">
                                </f:Button>

                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="作废"
                                    Icon="Delete" Hidden="True" Size="Medium">
                                </f:Button>

                                <f:Button ID="btnOff" OnClick="btnOff_Click" Size="Medium" runat="server" Text="禁用" Icon="StopRed" Hidden="True">
                                </f:Button>
                                <f:Button ID="btnStart" OnClick="btnStart_Click" Size="Medium" runat="server" Text="启用" Icon="StarGrey" Hidden="True">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtFName" runat="server" Label="银行">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFComment" runat="server" Label="卡号">
                                                        </f:TextBox>
                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px">
                                    <Items>
                                        <f:Grid ID="Grid1" Title="银行账户" PageSize="20" ShowBorder="false" ShowHeader="false"
                                            AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                            DataKeyNames="FCode,FParentCode" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                            OnRowCommand="Grid1_RowCommand" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField Width="180px" DataField="FName" HeaderText="开户银行" SortField="FName" />
                                                <f:BoundField Width="100px" DataField="FUserCode" HeaderText="自定义编码" SortField="FUserCode" />
                                                <f:BoundField Width="220px" DataField="FComment" HeaderText="账号" SortField="FComment" NullDisplayText="保密" />
                                                <f:BoundField Width="100px" DataField="FAbstract" HeaderText="业务性质" SortField="FAbstract" TextAlign="Right" />
                                                <f:CheckBoxField MinWidth="100px" RenderAsStaticField="True" DataField="FFlag"
                                                    TextAlign="Center" HeaderText="是否启用" SortField="FFlag" />
                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="20" Value="20" />
                                                    <f:ListItem Text="30" Value="30" />
                                                    <f:ListItem Text="50" Value="50" />
                                                </f:DropDownList>
                                            </PageItems>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Hidden="True"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
            IFrameUrl="about:blank" Height="300px" Width="450px" OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">

        function closeActiveTab() {
            parent.removeActiveTab();
        };

    </script>
</body>
</html>
