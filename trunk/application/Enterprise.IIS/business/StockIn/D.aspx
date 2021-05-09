<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="D.aspx.cs" Inherits="Enterprise.IIS.business.StockIn.D" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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
                            <f:FormRow ID="FormRow4" >
                                <Items>
                                    <f:TextBox ID="txtFInTemperature" runat="server" Label="进厂温度(°C)"
                                        LabelAlign="Right" Text="0">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFInPressure" runat="server" Label="进厂压力(MPa)"
                                        LabelAlign="Right"  Text="0">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow5" >
                                <Items>
                                    <f:TextBox ID="txtoutTemperature" runat="server" Label="出厂温度(°C)"
                                        LabelAlign="Right" Text="0">
                                    </f:TextBox>
                                    <f:TextBox ID="txtoutPressure" runat="server" Label="出厂压力(MPa)"
                                        LabelAlign="Right"  Text="0">
                                    </f:TextBox>
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
