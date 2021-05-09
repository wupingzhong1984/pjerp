<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Printer.aspx.cs" Inherits="Enterprise.IIS.Printer" %>

{<!DOCTYPE html>
<html lang='en'><head><meta charset='UTF-8'><title>打印</title></head>
<body><div style='margin:2px auto;width:850px;height:440px;border:0px solid red' >
<center><b><font face='隶书' size='5'>河南禄恒软件科技有限公司送货单</font></b></center>
<center><font face='隶书' size='3' >地址：郑州市金水区金水东路226号楷林国际B座816  电话：18768868380   </font></center>
<table style='font-size:15px;' border='0' cellspacing='0' align='center' width='685'>
<tr height='22'><td width='68'>客户全称</td><td colspan='3' style='width: 224px'>阳江市广隆工业有限公司</td><td width='64' align='right'>单号</td>
<td width='120'>XS-160712-0002</td></tr><tr height='22'><td>送货地址</td><td colspan='3' style='width: 224px'>北惯(原宝中宝厂)</td><td align='right'>日期</td>
<td>2016-07-12</td></tr><tr height='22'><td>联系人</td><td colspan='3' style='width: 224px'>莫经理</td><td align='right'>联系电话</td>
<td>8866179</td></tr></table><table  border='1' cellspacing='0' width='840'  align='center' style='font-size:15px;'>
        <tr align='center'>
            <td width='46' rowspan='2'>序号</td>
            <td width='120' rowspan='2'>品名</td>
            <td width='74' rowspan='2'>规格</td>
            <td width='37' rowspan='2'>单位</td>
            <td colspan='4' width='290'>气体信息</td>
            <td width='70' rowspan='2'>单价</td>
            <td width='70' rowspan='2'>金额</td>
            <td width='80' rowspan='2'>备注</td>
        </tr>
        <tr align='center'>
            <td width='61'>公斤</td>
            <td width='61'>空瓶</td>
            <td width='55'>实瓶</td>
            <td width='71'>累计欠瓶</td>
        </tr><tr align='center'>
                                        <td>1</td>
                                        <td>混合气(Ar+CO2)</td>
                                        <td>40L</td>
                                        <td>瓶</td>
                                        <td>0.000</td>
                                        <td>0</td>
                                        <td>1</td>
                                        <td>1</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr><tr align='center'>
                                        <td>2</td>
                                        <td>氮气(N2)</td>
                                        <td>40L</td>
                                        <td>瓶</td>
                                        <td>0.000</td>
                                        <td>0</td>
                                        <td>1</td>
                                        <td>1</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr><tr align='center'>
            <td colspan='4'><div align='right'>合计</div></td>
            <td>0.000</td>
            <td>0</td>
            <td>2</td>
            <td>2</td>
            <td colspan='2'><div align='left'>金额合计：70.000</div></td>
        </tr>
        <tr>
            <td colspan='11'>大写金额：柒拾元整</td>
        </tr>
    </table><table style='font-size:15px;' align='left' width='774' cellspacing='0' border='0'>
        <tr>
            <td width='60' height='23'>发货人：</td>
            <td width='80'>超级管理员</td>
            <td width='60'>车号：</td>
            <td width='80'>陕A00000</td>
            <td width='60'>司机：</td>
            <td width='80'>,索虎军</td>
            <td width='60'>押运员:</td>
            <td width='80'></td>
            <td width='80'>客户签字：</td>
            <td width='60'></td>
        </tr>
        <tr>
            <td height='21' colspan='6'>注：白色联—财务 红色联—顾客 绿色联—存根</td>
            <td height='21' >打印时间</td><td height='21' colspan='3'>2016-07-16 15:56:44</td>
        </tr>
    </table>
</div>
</body>
</html>}