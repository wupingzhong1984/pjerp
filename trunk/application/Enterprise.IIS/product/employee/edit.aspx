<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Enterprise.IIS.product.employee.edit" %>

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
        BodyPadding="0px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="True" Icon="SystemSaveNew"  Size="Medium"
                        ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                ActiveTabIndex="0" runat="server">
                <Tabs>
                    <f:Tab ID="Tab1" Title="基本信息" BodyPadding="10px" Layout="Fit"
                        runat="server">
                        <Items>
                            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                runat="server">
                                <Rows>
                                    <f:FormRow ID="FormRow1">
                                        <Items>
                                            <%--<f:Label ID="lblorgnization_name" LabelAlign="Right" runat="server" Label="组织机构">
                                            </f:Label>--%>
                                            
                                            <f:DropDownList runat="server" ID="ddlOrgnization" Label="组织机构" EnableEdit="False" LabelAlign="Right">
                                            </f:DropDownList>
                                            <f:TextBox ID="txtname" LabelAlign="Right" runat="server" Label="姓名" MinLength="2" Required="True" ShowRedStar="True"
                                                EmptyText="姓名">
                                            </f:TextBox>
                                            <f:TextBox ID="txtjob_number" LabelAlign="Right" runat="server" Label="工号" MinLength="3" EmptyText="工号" Readonly="True"  Enabled="False">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow2">
                                        <Items>
                                            <f:DropDownList ID="ddlaccount_sex" LabelAlign="Right" runat="server" Label="性别" Required="true" ShowRedStar="True">
                                                <f:ListItem Text="男" Value="男"></f:ListItem>
                                                <f:ListItem Text="女" Value="女"></f:ListItem>
                                            </f:DropDownList>
                                            <f:DatePicker ID="dpbirthday" LabelAlign="Right" runat="server" Required="false" Label="出生日期" EmptyText="出生日期"
                                                ShowRedStar="false" DateFormatString="yyyy-MM-dd">
                                            </f:DatePicker>
                                            <f:NumberBox ID="txtage" LabelAlign="Right" runat="server" Label="年龄" EmptyText="年龄" MaxLength="150">
                                            </f:NumberBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow3">
                                        <Items>
                                            <f:TextBox ID="txtprofession" LabelAlign="Right" runat="server" Label="毕业院校" EmptyText="毕业院校">
                                            </f:TextBox>
                                            <f:DropDownList ID="ddleducation" LabelAlign="Right" runat="server" Label="文化程度" DataTextField="key" DataValueField="value">
                                            </f:DropDownList>
                                            <f:TextBox ID="txtmajor" runat="server" LabelAlign="Right" Label="专业" EmptyText="专业">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow4">
                                        <Items>
                                            <f:DatePicker ID="dpjob_date" LabelAlign="Right" runat="server" Required="false" Label="入职日期" EmptyText="入职日期"
                                                ShowRedStar="false">
                                            </f:DatePicker>
                                            <f:DropDownList ID="ddlFLogistics" runat="server" Label="物流公司" >
                                            </f:DropDownList>

                                            <f:DropDownList ID="ddlFDistributionPoint" LabelAlign="Right" runat="server" Label="作业区" DataTextField="key" DataValueField="value" EnableMultiSelect="True">
                                            </f:DropDownList>
                                        </Items>
                                    </f:FormRow>

                                    <f:FormRow ID="FormRow5">
                                        <Items>
                                            <f:TextBox ID="txtFNo" LabelAlign="Right" runat="server" Label="资格证号" EmptyText="资格证号">
                                            </f:TextBox>
                                            <f:Label runat="server" Hidden="true"></f:Label>
                                            <f:Label runat="server" Hidden="true"></f:Label>

                                        </Items>
                                    </f:FormRow>

                                    <f:FormRow ID="FormRow6">
                                        <Items>
                                            <f:TextArea ID="txtgraduate_institutions" LabelAlign="Right" runat="server" Label="描述" EmptyText="描述" Height="160px">
                                            </f:TextArea>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab2" Title="工作相关" BodyPadding="10px" Layout="Fit"
                        runat="server">
                        <Items>
                            <f:Form ID="SimpleForm2" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                runat="server">
                                <Rows>
                                    <f:FormRow ID="FormRow7">
                                        <Items>
                                            <f:DropDownList ID="ddlprofessional" LabelAlign="Right" runat="server" Label="所属岗位" DataTextField="key" DataValueField="value" EnableMultiSelect="True">
                                            </f:DropDownList>
                                            <f:DropDownList ID="ddljob_nature" LabelAlign="Right" runat="server" Label="用工性质" DataTextField="key" DataValueField="value">
                                            </f:DropDownList>
                                            <f:TextBox ID="txtidentity_card" LabelAlign="Right" runat="server" Label="身份证号码" MinLength="3" EmptyText="身份证号码">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow8">
                                        <Items>
                                            <f:DropDownList ID="ddlbank_class" LabelAlign="Right" runat="server" Label="银行名称" DataTextField="key" DataValueField="value">
                                            </f:DropDownList>
                                            <f:TextBox ID="txtsalary_bank" LabelAlign="Right" runat="server" Label="工资卡号" MinLength="3" EmptyText="工资卡号">
                                            </f:TextBox>
                                            <f:TextBox ID="txtemail" LabelAlign="Right" runat="server" Label="电子邮箱" MinLength="3" EmptyText="电子邮箱">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow9">
                                        <Items>
                                            <f:TextBox ID="txtqq" runat="server" LabelAlign="Right" Label="QQ" MinLength="3" EmptyText="QQ">
                                            </f:TextBox>
                                            <f:TextBox ID="txtoffice_phone" LabelAlign="Right" runat="server" Label="手机" MinLength="11" EmptyText="手机">
                                            </f:TextBox>
                                            <f:TextBox ID="txtoffice_s_phone" LabelAlign="Right" runat="server" Label="短号" MinLength="3" EmptyText="短号">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow10">
                                        <Items>
                                            <f:TextBox ID="txtzip_code"  LabelAlign="Right" runat="server" Label="邮编" MinLength="3" EmptyText="邮编">
                                            </f:TextBox>
                                            <f:TextBox ID="txtoffice_tel" LabelAlign="Right" runat="server" Label="办公电话" MinLength="3" EmptyText="办公电话">
                                            </f:TextBox>
                                            <f:TextBox ID="txtoffice_fax" LabelAlign="Right" runat="server" Label="办公传真" MinLength="3" EmptyText="办公传真">
                                            </f:TextBox>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow11">
                                        <Items>
                                            <f:TextBox ID="txtoffice_address" LabelAlign="Right" runat="server" Label="办公地址" MinLength="3" EmptyText="办公地址">
                                            </f:TextBox>
                                            <f:CheckBox ID="txtflag" LabelAlign="Right" runat="server" Label="是否离职">
                                            </f:CheckBox>
                                            <f:DatePicker ID="dpflag_date" LabelAlign="Right" runat="server" Required="false" Label="离职日期" EmptyText="离职日期"
                                                ShowRedStar="false">
                                            </f:DatePicker>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow12">
                                        <Items>
                                            <f:TextArea ID="txtflag_cause" LabelAlign="Right" runat="server" Label="离职去向" MinLength="3" EmptyText="离职去向">
                                            </f:TextArea>
                                        </Items>
                                    </f:FormRow>
                                    <f:FormRow ID="FormRow13">
                                        <Items>
                                            <f:TextArea ID="txtflag_direction" LabelAlign="Right" runat="server" Label="离职原因" MinLength="3" EmptyText="离职原因">
                                            </f:TextArea>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                        </Items>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
