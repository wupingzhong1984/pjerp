using System;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Enterprise.Data;

namespace Enterprise.IIS.product.serverinfo
{
// ReSharper disable once InconsistentNaming
    public partial class index : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbServerName.Text = "http://" + Request.Url.Host;
                lbIp.Text = Request.ServerVariables["LOCAl_ADDR"];
                lbDomain.Text = Request.ServerVariables["SERVER_NAME"];
                lbPort.Text = Request.ServerVariables["Server_Port"];
                lbIISVer.Text = Request.ServerVariables["Server_SoftWare"];
                lbPhPath.Text = Request.PhysicalApplicationPath;
                lbOperat.Text = Environment.OSVersion.ToString();
                lbSystemPath.Text = Environment.SystemDirectory;
                lbTimeOut.Text = (Server.ScriptTimeout) + "秒";
                lbLan.Text = CultureInfo.InstalledUICulture.EnglishName;
                lbAspnetVer.Text = string.Concat(new object[] { Environment.Version.Major, ".", Environment.Version.Minor, Environment.Version.Build, ".", Environment.Version.Revision });
                lbCurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Version Vector");
                if (key != null) lbIEVer.Text = key.GetValue("IE", "未检测到").ToString();
                lbServerLastStartToNow.Text = ((Environment.TickCount / 0x3e8) / 60) + "分钟";

                string[] achDrives = Directory.GetLogicalDrives();
                for (int i = 0; i < Directory.GetLogicalDrives().Length - 1; i++)
                {
                    lbLogicDriver.Text = lbLogicDriver.Text + achDrives[i];
                }

                lbCpuNum.Text = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
                lbCpuType.Text = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
                lbMemory.Text = (Environment.WorkingSet / 1024) + "M";
                lbMemoryPro.Text = ((Double)GC.GetTotalMemory(false) / 1048576).ToString("N2") + "M";
                lbMemoryNet.Text = ((Double)Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + "M";
                lbCpuNet.Text = Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds.ToString("N2");
                lbSessionNum.Text = Session.Contents.Count.ToString(CultureInfo.InvariantCulture);
                lbSession.Text = Session.Contents.SessionID;
                lbUser.Text = Environment.UserName;
            }
        }
    }
}