<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinPrinter.aspx.cs" Inherits="Enterprise.IIS.business.WinPrinter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" width="560px" style="border-style: solid; border-color: inherit; border-width: 0px; border-collapse: collapse; height: 63px;" bordercolor="#000000">
            <tr>
                <td width="100%" height="30px">
                    <p align="center">
                        <font face="隶书" size="5" style="letter-spacing: 10px">河南禄恒软件科技有限公司<br/>气体配送单</font>
                    </p>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>客户名称：</td>
                <td></td>
                <td></td>
                <td></td>
                <td>单号：</td>
                <td></td>
            </tr>
            <tr>
                <td>送货地址：</td>
                <td></td>
                <td></td>
                <td></td>
                <td>日期：</td>
                <td></td>
            </tr>
            <tr>
                <td>联系人：</td>
                <td></td>
                <td></td>
                <td></td>
                <td>联系电话：</td>
                <td></td>
            </tr>
        </table>
        <table border="1" bordercolor="#000000">

            <tr>
                <td rowspan="2">序号</td>
                <td rowspan="2">品名</td>
                <td rowspan="2">规格</td>
                <td rowspan="2">单位</td>
                <td colspan="4">气休信息</td>
                <td rowspan="2">单价</td>
                <td rowspan="2">金额</td>
                <td rowspan="2">备注</td>
            </tr>
            <tr>
                <td>空瓶</td>
                <td>满瓶</td>
                <td>上期欠瓶</td>
                <td>累计欠瓶</td>
            </tr>
            <tr>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>
                <td>10</td>
                <td>11</td>
            </tr>
            <tr>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>
                <td>10</td>
                <td>11</td>
            </tr>
            <tr>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>
                <td>10</td>
                <td>11</td>
            </tr>
            <tr>
                <td colspan="4">合计</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>合计</td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="11">大写金额：</td>
            </tr>
        </table>



         <table>
            <tr>
                <td>开票：</td>
                <td></td>
                <td>配送员:</td>
                <td></td>
                <td>司机：</td>
                <td></td>
                <td>车号：</td>
                <td></td>
                <td>客户签字：</td>
                <td></td>
            </tr>
            <tr>
                <td colspan="10">请保管好您的客户联和产品全口算题卡证，如有质量问题，请及时联系我们，数量负数表示退货。</td>
            </tr>
             <tr>
                <td colspan="10">第一联：存根（白） 第二联：提货（红） 第三联：结算：（黄） 第四联：回单（绿）</td>
            </tr>
        </table>
    </form>
</body>
</html>
