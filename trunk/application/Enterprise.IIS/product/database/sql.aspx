<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="sql.aspx.cs"
    Inherits="Enterprise.IIS.product.webs.sql" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>在线执行SQL</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../ueditor/themes/default/ueditor.css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
       >
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnSubmit" runat="server" CssClass="inline" Icon="DatabaseAdd" Text="执行"
                        OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="0px"
                ShowBorder="False" ShowHeader="false" Title="执行SQL语句">
                <textarea name="UEditor1" id="UEditor1">
                </textarea>
            </f:ContentPanel>
        </Items>
    </f:Panel>
    <br />
    </form>
    <script type="text/javascript">
        window.UEDITOR_HOME_URL = '<%= ResolveUrl("~/ueditor/") %>';
    </script>
    <script type="text/javascript" src="../../ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="../../ueditor/ueditor.all.min.js"></script>
    <script type="text/javascript">
        var editor;
        function onReady() {
            editor = new UE.ui.Editor({
                initialFrameWidth: '100',
                initialFrameHeight: 350,
                minFrameHeight: 350,
                autoFloatEnabled: false,
                focus: true
            });
            editor.render("UEditor1");
        }


        // 提交数据之前同步到表单隐藏字段
        X.util.beforeAjaxPostBackScript = function () {
            editor.sync();
        };

        // 更新编辑器内容
        function updateUEditor(content) {
            window.setTimeout(function () {
                editor.setContent(content);
            }, 100);
        }
    </script>
</body>
</html>
