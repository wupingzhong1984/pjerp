<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinEmployee.aspx.cs" Inherits="Enterprise.IIS.Common.WinEmployee" %>

<%@ Import Namespace="Enterprise.Data" %>
<%@ Import Namespace="Enterprise.Framework.Enum" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>员工管理</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowHeader="false" Split="true"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                            ShowBorder="false" Icon="House" Title="组织机构" runat="server"
                            EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="员工管理" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                                </f:Button>
                                <f:Button ID="btnSearch" runat="server" Size="Medium" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                                </f:Button>
                                <f:Button ID="btnConfirm" runat="server" Text="确定"
                                    OnClick="btnConfirm_Click" Icon="Accept" Hidden="False" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="false"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        
                                                         <f:TextBox ID="txtFSpell" runat="server" Label="助记码">
                                                        </f:TextBox>


                                                        <f:TextBox ID="txtaccount_name" runat="server" Label="姓名">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtcode_cn" runat="server" Label="助记码">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="id,job_number,name" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowClick">
                                            <Columns>
                                                <f:BoundField MinWidth="120px" DataField="job_number" HeaderText="工号" SortField="job_number" />
                                                <f:BoundField MinWidth="80px" DataField="name" HeaderText="姓名" SortField="name" />
                                                <f:BoundField MinWidth="40px" DataField="professional" HeaderText="岗位" SortField="professional" />
                                                <f:BoundField MinWidth="80px" DataField="code_cn" HeaderText="助记码" SortField="code_cn" />
                                                <f:BoundField MinWidth="40px" DataField="sex" HeaderText="性别" SortField="sex" />
                                                <f:BoundField MinWidth="100px" DataField="base_orgnization.org_name" HeaderText="所属机构" SortField="orgnization_id" />
                                                <f:BoundField MinWidth="120px" DataField="job_date" HeaderText="入职时间" SortField="job_date" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="130px" DataField="email" HeaderText="电子邮箱" SortField="email" />
                                                <f:BoundField MinWidth="80px" DataField="office_phone" HeaderText="手机号码" SortField="office_phone" />
                                                <f:BoundField MinWidth="80px" DataField="qq" HeaderText="QQ" SortField="qq" />
                                                
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
    </form>
</body>
</html>
