<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Safe.Edit" %>


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
                        <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="True" Icon="SystemSaveNew" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="安全隐患排查" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:TextBox ID="txtOrderCode" runat="server" Label="编号"  Required="True" ShowRedStar="True"
                                                    EmptyText="编号">
                                                </f:TextBox>
                                               
                                               <f:DatePicker ID="dpCheckDate" runat="server" Label="检查时间"></f:DatePicker>       
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:DropDownList ID="ddlTypeNum" runat="server" Label="类型"  ShowRedStar="True">                                                   
                                                </f:DropDownList>                                                
                                               <f:DatePicker ID="dpReformDate" runat="server" Label="整改日期"></f:DatePicker>       
                                                
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow3">
                                            <Items>                                                                                        
                                                <f:TextArea ID="txtContext" runat="server" Label="隐患内容"  Height="70px"  EmptyText="隐患内容"></f:TextArea>
                                            </Items>
                                        </f:FormRow>                                       
                                        <f:FormRow ID="FormRow5">
                                            <Items>
                                                <f:TextArea ID="txtReason" runat="server" Label="原因分析" EmptyText="原因分析" Height="70px">
                                                </f:TextArea>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow6">
                                            <Items>
                                                <f:TextArea ID="txtMarkContext" runat="server" Label="整改措施" EmptyText="整改措施" Height="70px">
                                                </f:TextArea>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow7">
                                            <Items>
                                                <f:NumberBox ID="txtFunds" runat="server" Label="经费(元)"  Text="0"></f:NumberBox>
                                                
                                            </Items>
                                        </f:FormRow>
                                          <f:FormRow ID="FormRow8">
                                            <Items>
                                                <f:DropDownList ID="ddlDept" runat="server" Label="部门"  ShowRedStar="True">                                                   
                                                </f:DropDownList>   
                                               <f:DropDownList ID="ddlPerson" runat="server" Label="责任人"  ShowRedStar="True">                                                   
                                                </f:DropDownList>   
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
