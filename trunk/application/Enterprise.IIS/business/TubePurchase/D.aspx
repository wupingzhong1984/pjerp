<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="D.aspx.cs" Inherits="Enterprise.IIS.business.TubePurchase.D" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Icon="SystemSaveNew"
                        ValidateForms="SimpleForm1" OnClick="btnSubmit_Click" Size="Medium">
                    </f:Button>

                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose"
                        Size="Medium">
                    </f:Button>

                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                        runat="server">
                        <Rows>
                            <f:FormRow ID="FormRow1">
                                <Items>
                                    <f:Label ID="txtFName" runat="server" Label="客户名称" LabelAlign="Right">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow2">
                                <Items>
                                    <f:Label ID="txtFItemName" runat="server" Label="存货名称" LabelAlign="Right">
                                    </f:Label>
                                    <f:Label ID="txtFUnit" runat="server" Label="计量名称" LabelAlign="Right">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow3">
                                <Items>
                                    <f:DropDownList ID="ddlDevice" runat="server" Label="存运设备" DataTextField="FKey" AutoPostBack="true"
                                        DataValueField="FValue" LabelAlign="Right" OnSelectedIndexChanged="ddlDevice_SelectedIndexChanged">
                                    </f:DropDownList>

                                    <f:TextBox ID="txtFDeviceValue" runat="server" Label="水溶积(m³)"
                                        LabelAlign="Right" Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow4">
                                <Items>
                                    <f:TextBox ID="txtFInTemperature" runat="server" Label="进厂湿度(°C)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFInPressure" runat="server" Label="进厂压力(MPa)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow5">
                                <Items>
                                    <f:TextBox ID="txtFOutTemperature" runat="server" Label="出厂湿度(°C)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFOutPressure" runat="server" Label="出厂压力(MPa)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow6">
                                <Items>
                                    <f:TextBox ID="txtFPayTemperature" runat="server" Label="交付湿度(°C)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFPayPressure" runat="server" Label="交付压力(MPa)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow7">
                                <Items>
                                    <f:TextBox ID="txtFReceiveTemperature" runat="server" Label="接收湿度(°C)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFReceivePressure" runat="server" Label="接收压力(MPa)"
                                        LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow8">
                                <Items>
                                    <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                                        <hr />
                                        <font color="red">注：自提单据进厂参数=接收参数，出厂参数=交付参数，1MPa=10Bar=10.2kgf/cm²</font><br />
                                        <font color="red">$W1：进厂温度、$W2：出厂温度、$W3:交付温度、$W4:接收温度、$V:水容积</font><br />
                                        <font color="red">$Y1：进厂压力、$Y2：出厂压力、$Y2:交付压力、$Y2:接收压力</font><br />
                                    </f:ContentPanel>
                                </Items>
                            </f:FormRow>


                        </Rows>
                    </f:Form>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
   <script language="javascript" type="text/javascript">
       Ext.onReady(function () {
           //var map = new Ext.util.KeyMap({
           //    target: 'adRuleView',
           //    binding: [
           //        {
           //            key: Ext.EventObject.ESC,
           //            fn: function() { brUnitRuleGrid.addRuleWindow.close(); }
           //        }
           //    ]
           //});
           Ext.get("text").on("keypress", function (e) {
               if (e.getKey() == Ext.EventObject.ESC) {
               }
           });
       });
   </script>

</body>
</html>