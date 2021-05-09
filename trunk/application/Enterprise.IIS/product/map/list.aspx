<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="Enterprise.IIS.product.map.list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowHeader="False" Split="true" 
                Width="200px" Position="Left" Layout="Fit" runat="server" Title="地理位置">
                <Items>
                    <f:Grid ID="Grid1" Title="表格" ShowBorder="False" ShowHeader="False" runat="server" IsDatabasePaging="true" EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowClick">
                        <Columns>
                            <f:BoundField Width="50px" DataField="CityName" DataFormatString="{0}" HeaderText="地区" />
                            <f:BoundField Width="65px" DataField="pointX" DataFormatString="{0}" HeaderText="经度" />
                            <f:BoundField Width="65px" DataField="pointY" DataFormatString="{0}" HeaderText="纬度" />
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Title="百度地图" Position="Center" ShowHeader="False"
                ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <f:Button runat="server" Size="Medium" ID="Button1" Icon="Map" Text="当前城市" EnablePostBack="false" OnClientClick="panTo();">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                Layout="Fit" runat="server" BodyPadding="0px">
                                <Items>
                                    <f:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false">
                                        <div id="container1" style="width: 100%; height: 800px; background-color: Blue;">
                                        </div>
                                    </f:ContentPanel>
                                </Items>
                            </f:Region>
                        </Regions>
                    </f:RegionPanel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.5&ak=C2c1257ee55b0cdb7d7df0ae4c3f4521"></script>
    <script src="//js.maxmind.com/js/country.js" type="text/javascript"></script>
    <script src="//js.maxmind.com/js/geoip.js" type="text/javascript" ></script>
    <script type="text/javascript">
        var map;
        var gridClientID = '<%= Grid1.ClientID %>';
        var gc;
        F.ready(function () {
            var lat = geoip_latitude();
            var lon = geoip_longitude();
            map = new BMap.Map("container1"); // 创建地图实例   
            var point = new BMap.Point(lat, lon); // 创建点坐标   
            map.centerAndZoom(point, 15);
            map.addEventListener("click", showInfo); //百度API注册单击事件
            gc = new BMap.Geocoder();
            //var myCity = new BMap.LocalCity();
            //myCity.get(myFun);
        });
        //移动到当前城市
        function panTo() {
            var myCity = new BMap.LocalCity();
            myCity.get(myFun);
        }
        //移动到某个经度和纬度
        function PanToCity(x, y) {
            map.panTo(new BMap.Point(x, y));
        }
        function myFun(result) {
            var cityName = result.name;
            map.setCenter(cityName);
        }
        //百度API点击回调事件
        function showInfo(e) {
            //可以把获取到的经度和纬度保存起来,或者动态添加到Grid中 
            var pt = e.point;
            gc.getLocation(pt, function (rs) {
                var addComp = rs.addressComponents;
                __doPostBack(addComp.city + "," + e.point.lng + "," + e.point.lat, 'specialkey'); //获取坐标简单拼装发送给服务端
            });
        }  
     </script>
</body>
</html>