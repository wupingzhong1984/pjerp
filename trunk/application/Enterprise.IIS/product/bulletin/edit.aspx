<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .mright
        {
            margin-right: 5px;
        }
        .datecontainer .x-form-field-trigger-wrap
        {
            margin-right: 5px;
        }
    </style>
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
                    <f:Button ID="btnSubmit" Text="提交表单" Hidden="True" runat="server" Icon="SystemSaveNew"
                        ValidateForms="SimpleForm1" OnClick="btnSubmit_Click" Size="Medium">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" LabelAlign="Right"
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                        <Items>
                            <f:TextBox ID="txttitle" runat="server" Label="标题" MinLength="1" Required="True"
                                ShowRedStar="True" EmptyText="名称" LabelAlign="Right">
                            </f:TextBox>
                            <f:RadioButtonList ID="rbtnPrecedence" Label="优先状态" runat="server" Required="True"
                                ShowRedStar="True" LabelAlign="Right">
                                <f:RadioItem Text="一般" Value="一般" Selected="true" />
                                <f:RadioItem Text="紧急" Value="紧急" />
                                <f:RadioItem Text="重要" Value="重要" />
                            </f:RadioButtonList>
                            <f:DatePicker ID="dpvalidity_s" runat="server" Required="True" Label="有效开始日期" EmptyText="请选择开始日期"
                                ShowRedStar="True" LabelAlign="Right">
                            </f:DatePicker>
                            <f:DatePicker ID="dpvalidity_e" runat="server" Required="True" Label="有效结束日期" EmptyText="请选择结束日期"
                                ShowRedStar="True" LabelAlign="Right">
                            </f:DatePicker>
                            <f:TriggerBox ID="tbOrgnization" EnableEdit="false" Text="" EnablePostBack="True"
                                TriggerIcon="None" Label="公告部门" runat="server">
                            </f:TriggerBox>
                            <f:TriggerBox ID="tbRole" EnableEdit="false" Text="" EnablePostBack="false" TriggerIcon="None"
                                Label="公告角色" runat="server" LabelAlign="Right">
                            </f:TriggerBox>
                            <f:TriggerBox ID="tbAccount" EnableEdit="false" Text="" EnablePostBack="false" TriggerIcon="None"
                                Label="公告个人" runat="server" LabelAlign="Right">
                            </f:TriggerBox>
                            <f:FileUpload runat="server" ID="FileUpload1" ButtonText="浏览" EmptyText="" Required="false"
                                ShowLabel="True" ShowEmptyLabel="True" Label="上传文件" AutoPostBack="true" LabelAlign="Right">
                            </f:FileUpload>
                            <f:TextArea ID="txtm_content" runat="server" Label="公告内容" EmptyText="公告内容" Required="True"
                                ShowRedStar="True" MinLength="10" MaxLength="1200" Height="242px" LabelAlign="Right">
                            </f:TextArea>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    <f:HiddenField ID="HFOrgnization" runat="server" />
    <f:HiddenField ID="HFRole" runat="server" />
    <f:HiddenField ID="HFAccount" runat="server" />
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="True"
        Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Title="选择部门"  Height="450px" Width="360px">
    </f:Window>
    <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="True"
        Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Title="选择角色"  Height="450px" Width="800px">
    </f:Window>
    <f:Window ID="Window3" runat="server" WindowPosition="Center" IsModal="true"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="True"
        Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Title="选择员工"  Height="450px" Width="800px">
    </f:Window>
    </form>
</body>
</html>
