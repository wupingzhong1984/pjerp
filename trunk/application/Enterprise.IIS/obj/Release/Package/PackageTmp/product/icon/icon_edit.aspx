<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="icon_edit.aspx.cs" Inherits="Enterprise.IIS.product.icon.icon_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" Hidden="True" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1" Size="Medium"
                        OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <%--<f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                        <Items>
                            <f:TextBox ID="txticon_src" runat="server" Label="图标路径" MinLength="1" Required="True"
                                ShowRedStar="True" EmptyText="图标路径">
                            </f:TextBox>
                        </Items>
                    </f:SimpleForm>--%>
                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                        runat="server">
                        <Rows>
                            <f:FormRow ID="FormRow1">
                                <Items>
                                    <f:TextBox ID="txticon_src" runat="server" Label="图标路径" MinLength="1" Required="True"
                                        ShowRedStar="True" EmptyText="图标路径">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow2">
                                <Items>
                                    <f:Image ID="imgPhoto" Width="24px" Height="24px" Label="图标" ToolTipTitle="图标"
                                        CssClass="photo" runat="server">
                                    </f:Image>
                                    <f:FileUpload runat="server" ID="filePhoto" ShowRedStar="false" ButtonText="上传图标"
                                        ButtonOnly="true" Required="false" ShowLabel="False" Label="上传图标" AutoPostBack="true"
                                        OnFileSelected="filePhoto_FileSelected">
                                    </f:FileUpload>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:Form>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
