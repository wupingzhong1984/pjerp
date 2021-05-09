<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tablesize.aspx.cs" Inherits="Enterprise.IIS.product.database.tablesize" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据统计</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="数据统计" runat="server" ShowBorder="false" Layout="Fit" ShowHeader="False">
       <%-- <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="新增" Icon="ADD" Hidden="True">
                    </f:Button>
                    <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="授权" Icon="PageWhiteEdit">
                    </f:Button>
                    <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" runat="server" Text="删除" Icon="Delete" Hidden="True">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>--%>
        <Items>
            <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                <Regions>
                    <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center"
                        Layout="Fit" runat="server" BodyPadding="3px">
                        <Items>
                            <f:Grid ID="Grid1" runat="server"
                                EnableCheckBoxSelect="false" ShowBorder="false" ShowHeader="false"
                                DataKeyNames="id" IsDatabasePaging="true"
                                EmptyText="查询无结果" EnableHeaderMenu="True">
                                <Columns>
                                    <f:BoundField Width="180px" DataField="name" HeaderText="表名" />
                                     <f:BoundField Width="160px" DataField="tdescription" HeaderText="表说明"/> 
                                    <f:BoundField Width="200px" DataField="rows" HeaderText="记录数"/> 
                                    <%--<f:BoundField Width="200px" DataField="reserved" HeaderText="描述"/>--%> 
                                    <f:BoundField Width="200px" DataField="data" HeaderText="使用大小"/> 
                                    <f:BoundField Width="200px" DataField="index_size" HeaderText="索引所使使用大小"/> 
                                    <f:BoundField Width="160px" DataField="unused" HeaderText="未用的空间量"/> 

                                </Columns>       
                            </f:Grid>
                        </Items>
                    </f:Region>
                </Regions>
            </f:RegionPanel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
