<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Enterprise.IIS.product.enterpriseinfo.index" %>

<%--<%@ Register TagPrefix="fjx" Namespace="com.flajaxian" Assembly="Enterprise.FileUploader" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="基本参数" runat="server" ShowBorder="false" Layout="Fit" ShowHeader="False">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="inline" ValidateForms="SimpleForm1"
                        runat="server" Text="提交表单" Icon="SystemSaveNew" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnReset" Text="重置表单" EnablePostBack="false" runat="server" Icon="reload" Size="Medium">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SimpleForm ID="SimpleForm1" Width="700px" runat="server" LabelWidth="150px" Title="基本参数"
                BodyPadding="50px" ShowBorder="True" ShowHeader="False">
                <Items>
                    <f:TextBox ID="txtcom_name" Label="公司名称" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtcom_chairman" Label="董事长" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtcom_tel" Label="电话" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtName" ShowRedStar="true" Required="true" Label="系统全称" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtAbbreviation" ShowRedStar="true" Required="true" Label="系统简称" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtKeywords" ShowRedStar="true" Required="true" Label="系统关键字" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtversion" ShowRedStar="true" Required="true" Label="版本号" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtDescription" Label="系统功能描述" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtICP" Label="备案号" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtCookieDomain" Label="Cookie作用域" runat="server">
                    </f:TextBox>
                    <f:TextBox ID="txtSiteID" Label="系统授权号" runat="server">
                    </f:TextBox>
                    <f:RadioButtonList ID="rblAllowReg" AutoPostBack="true" Label="允许会员注册" ColumnNumber="3"
                        runat="server">
                        <f:RadioItem Text="是" Value="1" />
                        <f:RadioItem Text="否" Value="0" />
                    </f:RadioButtonList>
                    <f:RadioButtonList ID="rblCheckReg" AutoPostBack="true" Label="需要邮件激活" ColumnNumber="3"
                        runat="server">
                        <f:RadioItem Text="是" Value="1" />
                        <f:RadioItem Text="否" Value="0" />
                    </f:RadioButtonList>
                    <f:Image runat="server" ID="imgLogo" Label="单位Logo" ImageUrl="~/images/LH201311131001.jpg">
                    </f:Image>
                   <%-- <f:ContentPanel ID="ContentPanel1" ShowBorder="False" BodyPadding="0px" ShowHeader="False"
                        AutoScroll="true" runat="server" Expanded="True">
                        <fjx:FileUploader ID="FileUploader1" runat="server" FileIsPosted="true" OnFileReceived="FileUploader1_FileReceived"
                            IsSingleFileMode="True" InitiateUploadOnSelect="True">
                            <Adapters>
                                <fjx:FileSaverAdapter Runat="server" />
                            </Adapters>
                        </fjx:FileUploader>
                    </f:ContentPanel>--%>
                    <f:Label runat="server" Text="上传完logo请重新加载该画面" Label="注意事项">
                    </f:Label>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
