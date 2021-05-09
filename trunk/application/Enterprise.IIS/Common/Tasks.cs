using System.Timers;
using Enterprise.DataAccess.SQLServer;

namespace Enterprise.IIS.Common
{
    /// <summary>
    ///     定时任务
    /// </summary>
    public class Tasks
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private SqlService _sqlSever;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SqlService SqlService
        {
            get { return _sqlSever ?? (_sqlSever = new SqlService()); }
            set { _sqlSever = value; }
        }

        /// <summary>
        ///     定时任务参数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void TimeEvent(object source, ElapsedEventArgs e)
        {

            try
            {
               //SqlService.ExecuteProcedureCommand("proc_Task");
               
            }
            finally
            {


            }
        }
    }
}