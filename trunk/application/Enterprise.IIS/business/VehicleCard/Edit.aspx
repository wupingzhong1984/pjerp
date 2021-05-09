<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.VehicleCard.Edit" %>


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
                        <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="false" Icon="SystemSaveNew" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab3" Title="车辆证照" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Form ShowBorder="false" ShowHeader="False" LabelWidth="130px"
                                    runat="server" LabelAlign="Right">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" />
                                                    <f:Label runat="server" Hidden="True"/>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow13">
                                            <Items>
                                                <f:DatePicker ID="dpCertificateDate" runat="server" Width="140px" Required="false" Label="营运证年审时间" EmptyText="营运证年审时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtCertificateMoney" runat="server" Width="140px" Label="营运证年审费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow15">
                                            <Items>
                                                <f:DatePicker ID="dpDrivingDate" runat="server" Required="false" Label="行驶证年审时间" EmptyText="行驶证年审时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDrivingMoney" runat="server" Label="行驶证年审费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow16">
                                            <Items>
                                                <f:DatePicker ID="dpDeadlineDate" runat="server" Required="false" Label="交强险截止时间" EmptyText="交强险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDeadlineMoney" runat="server" Label="交强险年审费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow17">
                                            <Items>
                                                <f:DatePicker ID="dpCommercialDate" runat="server" Required="false" Label="商业险截止时间" EmptyText="商业险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtCommercialMoney" runat="server" Label="商业险年审费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow18">
                                            <Items>
                                                <f:DatePicker ID="dpDangerousDate" runat="server" Required="false" Label="危险品承运险截止时间" EmptyText="危险品承运险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDangerousMoney" runat="server" Label="危险品承运险年审费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow19">
                                            <Items>
                                                <f:DatePicker ID="dpTankDate" runat="server" Required="false" Label="罐体检测时间" EmptyText="罐体检测时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtTankMoney" runat="server" Label="罐体检测费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow20">
                                            <Items>
                                                <f:DatePicker ID="dpTwostageDate" runat="server" Required="false" Label="二级维护时间" EmptyText="二级维护时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtTwostageMoney" runat="server" Label="二级维护费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow21">
                                            <Items>
                                                <f:DatePicker ID="dpRegistrationDate" runat="server" Required="false" Label="登记评定时间" EmptyText="登记评定时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtRegistrationMoney" runat="server" Label="登记评定费用" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow22">
                                            <Items>
                                                <f:TextBox ID="txtContext" runat="server" Label="备注"   EmptyText="备注">
                                                </f:TextBox>
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
