<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="D.aspx.cs" Inherits="Enterprise.IIS.business.Dispatch.D" %>

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
                                    <f:Label ID="txtFVehicleNum" runat="server" Label="车牌号" LabelAlign="Right">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow2">
                                <Items>
                                    <f:TextBox ID="txtFTonnage" runat="server" Label="吨位"
                                        LabelAlign="Right"  Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFOperationCertificateNo" runat="server" Label="营运证号"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow3">
                                <Items>
                                    <f:TextBox ID="txtFRiskType" runat="server" Label="危险类别"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFItem" runat="server" Label="货品名称"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow4">
                                <Items>
                                    <f:TextBox ID="txtFNumber" runat="server" Label="件数"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFActual" runat="server" Label="实载吨位"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow5">
                                <Items>
                                    <f:TextBox ID="txtFMileage" runat="server" Label="里程"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFDriver" runat="server" Label="驾驶员"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true"> 
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>

                            <f:FormRow ID="FormRow6">
                                <Items>
                                   <f:TextBox ID="txtFSupercargo" runat="server" Label="押运员"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="txtFTimes" runat="server" Label="出车次数"
                                        LabelAlign="Right"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow7">
                                <Items>
                                   <f:TextBox ID="txtFUnit" runat="server" Label="托运单位"
                                        LabelAlign="Right"  Text="浦江气体"   Required="true" ShowRedStar="true">
                                    </f:TextBox>
                                    <f:TextBox ID="TextBox2" runat="server" Label="出车次数"
                                        LabelAlign="Right" Hidden="true">
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