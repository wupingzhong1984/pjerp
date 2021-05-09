<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Enterprise.IIS.product.serverinfo.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>环境状态</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="服务器探针" runat="server" ShowBorder="false" Layout="Fit" ShowHeader="False">
        <Items>
            <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                ActiveTabIndex="0" runat="server">
                <Tabs>
                    <f:Tab ID="Tab1" Title="常规属性" BodyPadding="0px" Layout="Fit"
                        runat="server">
                        <Items>
                            <f:SimpleForm ID="SimpleForm1" Width="700px" runat="server" LabelWidth="150px" Title="基本参数"
                                BodyPadding="50px" ShowBorder="False" ShowHeader="False">
                                <Items>
                                    <f:Label ID="lbServerName" runat="server" Label="服务器名">
                                    </f:Label>
                                    <f:Label ID="lbIp" runat="server" Label="IP地址">
                                    </f:Label>
                                    <f:Label ID="lbDomain" runat="server" Label="当前域名">
                                    </f:Label>    
                                    <f:Label ID="lbPort" runat="server" Label="WEB端口">
                                    </f:Label>
                                    <f:Label ID="lbAspnetVer" runat="server" Label=".NET Framework 版本">
                                    </f:Label>
                                    <f:Label ID="lbIISVer" runat="server" Label="IIS版本">
                                    </f:Label>
                                    <f:Label ID="lbPhPath" runat="server" Label="当前目录">
                                    </f:Label>
                                    <f:Label ID="lbOperat" runat="server" Label="服务器操作系统">
                                    </f:Label>
                                    <f:Label ID="lbSystemPath" runat="server" Label="系统所在文件夹">
                                    </f:Label>
                                    <f:Label ID="lbTimeOut" runat="server" Label="脚本默认超时时间">
                                    </f:Label>
                                    <f:Label ID="lbLan" runat="server" Label="服务器的语言种类">
                                    </f:Label>
                                    <f:Label ID="lbCurrentTime" runat="server" Label="服务器当前时间">
                                    </f:Label>
                                    <f:Label ID="lbIEVer" runat="server" Label="服务器IE版本">
                                    </f:Label>
                                    <f:Label ID="lbServerLastStartToNow" runat="server" Label="上次启动到现在已运行">
                                    </f:Label>
                                </Items>
                            </f:SimpleForm>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab2" Title="其它属性" BodyPadding="0px" Layout="Fit"
                        runat="server">
                        <Items>
                            <f:SimpleForm ID="SimpleForm2" Width="700px" runat="server" LabelWidth="150px" Title="基本参数"
                                BodyPadding="50px" ShowBorder="False" ShowHeader="False">
                                <Items>
                                    <f:Label ID="lbLogicDriver" runat="server" Label="逻辑驱动器"></f:Label>
                                    <f:Label ID="lbCpuType" runat="server" Label="CPU 类型"></f:Label>
                                    <f:Label ID="lbCpuNum" runat="server" Label="CPU 总数"></f:Label>
                                    <f:Label ID="lbMemory" runat="server" Label="虚拟内存"></f:Label>
                                    <f:Label ID="lbMemoryPro" runat="server" Label="当前程序占用内存"></f:Label>
                                    <f:Label ID="lbMemoryNet" runat="server" Label="Asp.net所占内存"></f:Label>
                                    <f:Label ID="lbCpuNet" runat="server" Label="Asp.net所占CPU"></f:Label>
                                    <f:Label ID="lbSessionNum" runat="server" Label="当前Session数量"></f:Label>
                                    <f:Label ID="lbSession" runat="server" Label="当前SessionID"></f:Label>
                                    <f:Label ID="lbUser" runat="server" Label="当前系统用户名"></f:Label>
                                </Items>                               
                            </f:SimpleForm>
                        </Items>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
