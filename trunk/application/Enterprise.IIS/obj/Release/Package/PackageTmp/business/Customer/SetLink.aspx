<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetLink.aspx.cs" Inherits="Enterprise.IIS.business.Customer.SetLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
        <f:Panel ID="Panel1" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True">
            <Items>
                <f:Grid ID="Grid1" PageSize="20"
                    ShowBorder="False"
                    ShowHeader="False"
                    AllowPaging="False"
                    EnableAfterEditEvent="True"
                    OnAfterEdit="Grid1_AfterEdit"
                    runat="server"
                    EnableCheckBoxSelect="True"
                    DataKeyNames="FCusCode,FId"
                    EnableSummary="True"
                    SummaryPosition="Bottom"
                    OnRowCommand="Grid1_RowCommand"
                    AllowSorting="False"
                    EmptyText="查询无结果"
                    EnableHeaderMenu="True"
                    AllowCellEditing="true"
                    ClicksToEdit="1">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium" EnablePostBack="False">
                                </f:Button>

                                <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                    OnClick="btnSubmit_Click" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                            ColumnID="colDelete" Width="55px" CommandName="Delete" Hidden="False" SortField="mc">
                        </f:LinkButtonField>

                        <f:RenderField MinWidth="130px" ColumnID="FType" DataField="FType" FieldType="String"
                            HeaderText="地址类别">
                            <Editor>
                                <f:DropDownList runat="server" ID="tbxFType">
                                    <f:ListItem Text="交货邮寄" Value="交货邮寄" />
                                    <f:ListItem Text="邮寄" Value="邮寄" />
                                    <f:ListItem Text="交货" Value="交货" />
                                </f:DropDownList>
                            </Editor>
                        </f:RenderField>

                        <f:RenderField MinWidth="600px" ColumnID="FAddress" DataField="FAddress" TextAlign="Left" FieldType="String"
                            HeaderText="地址">
                            <Editor>
                                <f:TextBox ID="tbxFAddress" Required="true" runat="server">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>

                        <f:RenderField MinWidth="100px" ColumnID="FLinkman" DataField="FLinkman" TextAlign="Left" FieldType="String"
                            HeaderText="联系人" Hidden="true">
                            <Editor>
                                <f:TextBox ID="tbxFLinkman" Required="true" runat="server">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>

                        <f:RenderField MinWidth="100px" ColumnID="FPhome" DataField="FPhome" TextAlign="Left" FieldType="String"
                            HeaderText="电话" Hidden="true">
                            <Editor>
                                <f:TextBox ID="tbxFPhome" Required="true" runat="server">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>

                        <f:RenderField MinWidth="100px" ColumnID="FMoile" DataField="FMoile" FieldType="String" TextAlign="Left"
                            HeaderText="手机" Hidden="true">
                            <Editor>
                                <f:TextBox ID="tbxFMoile" Required="true" runat="server">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>

                        <f:RenderField MinWidth="260px" ColumnID="FMemo" DataField="FMemo"
                            HeaderText="备注说明" Hidden="true">
                            <Editor>
                                <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
