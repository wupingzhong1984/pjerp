<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Vehicle.Edit" %>


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
                        <f:Tab ID="Tab1" Title="基本信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server" LabelAlign="Right">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:TextBox ID="txtcph" runat="server" Label="车牌号" Required="True" ShowRedStar="True"
                                                    EmptyText="车牌号">
                                                </f:TextBox>
                                                <f:TextBox ID="txtTypeNo" runat="server" Label="型号" EmptyText="型号" Readonly="False">
                                                </f:TextBox>
                                                <f:TextBox ID="txtBrand" runat="server" Label="品牌" EmptyText="品牌" Readonly="False">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:DropDownList ID="ddlTypeNum" runat="server" Label="车辆种类" Required="true" ShowRedStar="True">
                                                </f:DropDownList>
                                                <f:NumberBox ID="txttonner" runat="server" Label="额定载重" Required="true" ShowRedStar="True" EmptyText="额定载重" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtQuality" runat="server" Label="车辆自重" EmptyText="吨" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow3">
                                            <Items>
                                                <f:DropDownList ID="ddllx" runat="server" Label="车辆类型" Required="true" ShowRedStar="True">
                                                </f:DropDownList>
                                                <f:TextBox ID="txtwear" runat="server" Label="油耗" EmptyText="油耗"></f:TextBox>
                                                <f:NumberBox ID="txtFMeteredPrice" Required="True" ShowRedStar="True" runat="server" Label="计价标准" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow4">
                                            <Items>
                                                <f:DropDownList ID="ddlRunStatus" runat="server" Label="运行状态" Required="true" ShowRedStar="True">
                                                </f:DropDownList>
                                                <f:TextBox ID="txtFWaterSpace" Required="true" ShowRedStar="True" runat="server" Label="水溶积" EmptyText="水溶积"></f:TextBox>
                                                <f:DropDownList ID="ddlFLogistics" runat="server" Label="物流公司" Required="true" ShowRedStar="True">
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow401">
                                            <Items>
                                                <f:DropDownList ID="ddlFISMain" runat="server" Label="是否车头" Required="true" ShowRedStar="True">
                                                    <f:ListItem Value="-1" Text="" />
                                                    <f:ListItem Value="是" Text="是" />
                                                    <f:ListItem Value="否" Text="否" />
                                                </f:DropDownList>
                                                <f:Label runat="server" Hidden="true" />
                                                <f:Label runat="server" Hidden="true" />
                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow5">
                                            <Items>
                                                <f:TextArea ID="txtFMemo" runat="server" Label="描述" EmptyText="描述" Height="160px">
                                                </f:TextArea>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab2" Title="详细信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm2" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server" LabelAlign="Right">
                                    <Rows>
                                        <f:FormRow ID="FormRow7">
                                            <Items>
                                                <f:TextBox ID="txtColor" runat="server" Label="车辆颜色" EmptyText="车辆颜色">
                                                </f:TextBox>
                                                <f:DatePicker ID="dpFactoryDate" runat="server" Label="出厂日期"></f:DatePicker>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow8">
                                            <Items>
                                                <f:TextBox ID="txtFEngineType" runat="server" Label="发动机型号" EmptyText="发动机型号">
                                                </f:TextBox>
                                                <f:TextBox ID="txtFEngineNo" runat="server" Label="发动机号" EmptyText="发动机号">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow9">
                                            <Items>
                                                <f:TextBox ID="txtVIN" runat="server" Label="大梁号" EmptyText="大梁号">
                                                </f:TextBox>
                                                <f:TextBox ID="txtFuel" runat="server" Label="燃油种类" EmptyText="燃油种类">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow10">
                                            <Items>
                                                <f:TextBox ID="txtDesplaceMent" runat="server" Label="排量" EmptyText="排量">
                                                </f:TextBox>
                                                <f:TextBox ID="txtpower" runat="server" Label="功率" EmptyText="功率">
                                                </f:TextBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow11">
                                            <Items>
                                                <f:TextBox ID="txtFCompanyId" runat="server" Label="客户代码" EmptyText="车辆客户代码">
                                                </f:TextBox>
                                                <f:TextBox ID="txtReginNo" runat="server" Label="登记编号" EmptyText="车辆登记编号">
                                                </f:TextBox>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow22">
                                            <Items>
                                                <f:TextBox ID="txtRegionCompany" runat="server" Label="登记机关" EmptyText="登记机关">
                                                </f:TextBox>
                                                <f:DatePicker ID="dpFRegData" runat="server" Required="false" Label="注册日期" EmptyText="注册日期"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow12">
                                            <Items>
                                                <f:TextBox ID="txtFLicenseNum" runat="server" Label="经营许可证" EmptyText="经营许可证">
                                                </f:TextBox>
                                                <f:TextBox ID="txtoperateNum" runat="server" Label="营运证号" EmptyText="营运证号" Required="true" ShowRedStar="True">
                                                </f:TextBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow32">
                                            <Items>
                                                <f:TextBox ID="txtDrvierNum" runat="server" Label="行驶证号" EmptyText="行驶证号" Required="true" ShowRedStar="True">
                                                </f:TextBox>
                                                <f:Label runat="server" Hidden="True" />
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab3" Title="年检及保险" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Form ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server" LabelAlign="Right">
                                    <Rows>
                                        <f:FormRow ID="FormRow13">
                                            <Items>
                                                <f:DatePicker ID="dpCertificateDate" runat="server" LabelWidth="110px" Required="false" Label="营运证年审时间" EmptyText="营运证年审时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtCertificateMoney" runat="server" Label="营运证年审费用" LabelWidth="110px" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow15">
                                            <Items>
                                                <f:DatePicker ID="dpDrivingDate" runat="server" Required="false" Label="行驶证年审时间" LabelWidth="110px" EmptyText="行驶证年审时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDrivingMoney" runat="server" Label="行驶证年审费用" LabelWidth="110px" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow16">
                                            <Items>
                                                <f:DatePicker ID="dpDeadlineDate" runat="server" Required="false" LabelWidth="110px" Label="交强险截止时间" EmptyText="交强险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDeadlineMoney" runat="server" Label="交强险年审费用" LabelWidth="110px" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow17">
                                            <Items>
                                                <f:DatePicker ID="dpCommercialDate" runat="server" Required="false" LabelWidth="110px" Label="商业险截止时间" EmptyText="商业险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtCommercialMoney" runat="server" Label="商业险年审费用" LabelWidth="110px" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow18">
                                            <Items>
                                                <f:DatePicker ID="dpDangerousDate" runat="server" Required="false" Label="危险品承运险截止时间" LabelWidth="110px" EmptyText="危险品承运险截止时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtDangerousMoney" runat="server" Label="危险品承运险年审费用" LabelWidth="110px" DecimalPrecision="2" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow19">
                                            <Items>
                                                <f:DatePicker ID="dpTankDate" runat="server" Required="false" Label="罐体检测时间" LabelWidth="110px" EmptyText="罐体检测时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtTankMoney" runat="server" Label="罐体检测费用" DecimalPrecision="2" LabelWidth="110px" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow20">
                                            <Items>
                                                <f:DatePicker ID="dpTwostageDate" runat="server" Required="false" Label="二级维护时间" LabelWidth="110px" EmptyText="二级维护时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtTwostageMoney" runat="server" Label="二级维护费用" DecimalPrecision="2" LabelWidth="110px" MaxLength="150">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow21">
                                            <Items>
                                                <f:DatePicker ID="dpRegistrationDate" runat="server" Required="false" Label="登记评定时间" LabelWidth="110px" EmptyText="登记评定时间"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:NumberBox ID="txtRegistrationMoney" runat="server" Label="登记评定费用" DecimalPrecision="2" LabelWidth="110px" MaxLength="150">
                                                </f:NumberBox>

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
