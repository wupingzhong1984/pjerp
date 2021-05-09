<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="Enterprise.IIS.product.menu.menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单配置</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hdfImage"/>
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False"
                Width="200px" Position="Left" Layout="Fit" runat="server" EnableCollapse="true" >
                <Items>
                    <f:Tree ID="trMenu" Width="50px" OnNodeCommand="trMenu_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Title="系统菜单" runat="server" AutoScroll="True" >
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Title="菜单配置" Position="Center" ShowBorder="False"
                BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnAdd" Text="新增" Size="Medium" OnClick="btnAdd_Click" Icon="Add" CssClass="inline"
                                runat="server" Hidden="True"/>
                            <f:Button ID="btnDelete" Text="删除" Size="Medium" OnClick="btnDelete_Click" Icon="Delete" runat="server" Hidden="True"/>
                            <f:Button ID="btnSubmit" ValidateForms="SimpleFormDept" Icon="DatabaseAdd" OnClick="btnSubmit_Click" Hidden="True"
                                CssClass="inline" Text="提交表单" Size="Medium" runat="server">
                            </f:Button>
                            <f:Button ID="btnResut" Text="重置表单" Size="Medium" EnablePostBack="false" Icon="reload" runat="server" Hidden="True">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:SimpleForm ID="SimpleFormDept" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                        runat="server" ShowBorder="false" ShowHeader="False"
                        AutoScroll="true" LabelAlign="Right">
                        <Items>
                            <f:GroupPanel runat="server" Title="基本信息" ID="GPanelMenuInfo" EnableCollapse="True">
                                <Items>
                                    <f:TextBox ID="txtmenu_name" ShowRedStar="true" runat="server" Label="菜单名称" Required="true">
                                    </f:TextBox>
                                    <f:TextArea ID="txtmenu_url" runat="server" Label="页面地址" ShowRedStar="true" Required="true">
                                    </f:TextArea>
                                    <f:NumberBox ID="txtorg_sort" runat="server" Label="排序" Required="True" ShowRedStar="True">
                                    </f:NumberBox>
                                </Items>
                            </f:GroupPanel>
                            <f:GroupPanel runat="server" Title="显示控制" ID="GPanelMenuType" EnableCollapse="True">
                                <Items>
                                    <f:RadioButtonList ID="rbtnListmenu_is_view" runat="server" Label="菜单显示" ShowRedStar="true"
                                        Required="true">
                                        <f:RadioItem Text="是" Value="1" Selected="true" />
                                        <f:RadioItem Text="否" Value="2" />
                                    </f:RadioButtonList>
                                    <f:RadioButtonList ID="rbtnListmenu_is_frame_view" runat="server" Label="打开方式" ShowRedStar="true"
                                        Required="true">
                                        <f:RadioItem Text="在框架内" Value="1" Selected="true" />
                                        <f:RadioItem Text="在框架外" Value="2" />
                                    </f:RadioButtonList>
                                </Items>
                            </f:GroupPanel>
                            <f:GroupPanel runat="server" Title="图标设置" ID="GPanelICON" EnableCollapse="True">
                                <Items>
                                    <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="False"
                                        ShowHeader="False" LabelAlign="Right">
                                        <Items>
                                            <f:Panel ID="PanelICON" ShowHeader="false" Title="ICON"
                                                 EnableAjax="true" ShowBorder="false" runat="server">
                                                <Items>
                                                    <f:RadioButtonList ID="p_rbtnICON" AutoPostBack="true" OnSelectedIndexChanged="p_rbtnICON_SelectedIndexChanged"
                                                        runat="server" TableConfigColumns="20" ColumnNumber="20" DataTextField="id" DataValueField="icon_src">
                                                    </f:RadioButtonList>
                                                    <f:Image ID="Image1" runat="server" ImageUrl="" Label="图标样式">
                                                    </f:Image>
                                                    <f:FileUpload runat="server" ID="filePhoto" ShowRedStar="false" ButtonText="上传图标"
                                        ButtonIcon="Image" Required="false" ShowLabel="True" Label="上传图标" AutoPostBack="true" ButtonOnly="True"
                                        OnFileSelected="filePhoto_FileSelected">
                                    </f:FileUpload>
                                                </Items>
                                            </f:Panel>
                                        </Items>
                                    </f:SimpleForm>
                                </Items>
                            </f:GroupPanel>
                            <f:GroupPanel runat="server" Title="页面功能" ID="GPanelActions" EnableCollapse="True">
                                <Items>
                                    <f:SimpleForm ID="SimpleForm2" runat="server" ShowBorder="False"
                                        ShowHeader="False" LabelAlign="Right">
                                        <Items>
                                            <f:Panel ID="Panel3" ShowHeader="false" Title="ICON"
                                                 EnableAjax="true" ShowBorder="false" runat="server">
                                                <Items>
                                                    <f:CheckBoxList ID="cboxListActions" runat="server" TableConfigColumns="9" ColumnNumber="11"
                                                        DataTextField="action_name" DataValueField="action_en">
                                                    </f:CheckBoxList>
                                                </Items>
                                            </f:Panel>
                                        </Items>
                                    </f:SimpleForm>
                                </Items>
                            </f:GroupPanel>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
</body>
</html>
