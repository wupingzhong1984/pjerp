<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Safe.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>安全隐患</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region2" Title="安全隐患" Position="Center"
                ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnSearch" runat="server" Text="查询" Size="Medium" Icon="Zoom" OnClick="btnSearch_Click">
                            </f:Button>
                            <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑" Size="Medium"
                                Icon="PageWhiteEdit">
                            </f:Button>                
                            <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel" Size="Medium"
                                Hidden="True" EnableAjax="false" DisableControlBeforePostBack="false">
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
                                                     <f:DropDownList ID="ddlTypeNum" runat="server" Label="类型" LabelAlign="Right">                                                   
                                                </f:DropDownList>                                                
                                               <f:DatePicker ID="dpReformDate" runat="server" Label="整改日期" LabelAlign="Right"></f:DatePicker>  
                                                
                                                    <f:Label runat="server" Hidden="True"/>
                                                  <f:Label runat="server" Hidden="True"/>
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
                                        runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                        IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                        AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField MinWidth="80px" DataField="FOrderNo" HeaderText="编号" SortField="FOrderNo" />                                           
                                            <f:BoundField MinWidth="100px" DataField="FCheckDate" HeaderText="检查日期" SortField="FCheckDate" DataFormatString="{0:yyyy-MM-dd}"/>
                                            <f:BoundField MinWidth="100px" DataField="FReformDate" HeaderText="整改日期" SortField="FReformDate" DataFormatString="{0:yyyy-MM-dd}"/>
                                            <f:BoundField MinWidth="200px" DataField="FContext" HeaderText="隐患内容" SortField="FContext" />
                                            <f:BoundField MinWidth="200px" DataField="FReason" HeaderText="原因分析" SortField="FReason" />                                            
                                            <f:BoundField MinWidth="200px" DataField="FMarkContext" HeaderText="整改措施" SortField="FMarkContext" />                                            
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
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" 
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true"  Hidden="True"
        Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="486px" Width="850px"
        OnClose="Window1_Close">
    </f:Window>
    </form>
</body>
</html>
