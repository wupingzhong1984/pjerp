namespace Enterprise.IIS.Common
{
    public class T6Account
    {
        /// <summary>
        ///     Url
        /// </summary>
        public static string Url = System.Configuration.ConfigurationManager.AppSettings["Url"];

        /// <summary>
        ///     Sender
        /// </summary>
        public static string Sender { get; set; }

        /// <summary>
        ///     Receiver 
        /// </summary>
        public static string Receiver { get; set; }

        /// <summary>
        ///     业务账套登录日期格式要求 12/14/2015 ，如果不填，则取U8应用服务器所在操作系统的当前日期，否则取指定的登录日期进行业务账套登录；
        /// </summary>
        public static string Dynamicdate { get; set; }
    }
}