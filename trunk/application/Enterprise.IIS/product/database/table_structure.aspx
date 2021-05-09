<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="table_structure.aspx.cs" Inherits="Enterprise.IIS.product.database.table_structure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据结构</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                  Width="200px" Position="Left" Layout="Fit"
                runat="server">
                <Items>
                    <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Icon="Table" Title="表" runat="server"
                        EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit"
                BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Items>
                    <f:Grid ID="Grid1" Title="数据结构" ShowBorder="False" ShowHeader="False"
                         runat="server" EnableCheckBoxSelect="False"
                        DataKeyNames="id" IsDatabasePaging="true"
                        
                        EmptyText="查询无结果" EnableHeaderMenu="True">
                        <Columns>
                            <f:BoundField Width="200px" DataField="column" HeaderText="列名" />      
                            <f:BoundField Width="200px" DataField="remark" HeaderText="说明"/>
                            <f:BoundField Width="100px" DataField="datatype" HeaderText="数据类型" />
                            <f:BoundField Width="100px" DataField="length" HeaderText="长度" />
                            <f:BoundField Width="100px" DataField="identity" HeaderText="标识"/>
                            <f:BoundField Width="100px" DataField="key" HeaderText="主键"/>
                            <f:BoundField Width="100px" DataField="isnullable" HeaderText="允许空"/>
                            <f:BoundField Width="100px" DataField="default" HeaderText="默认值"/>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
</body>
</html>