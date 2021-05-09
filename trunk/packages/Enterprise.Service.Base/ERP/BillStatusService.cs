using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据操作状态
    /// </summary>
    public class BillStatusService : EntityRepositoryBase<DbContext, LHBillStatus>
    {
        public BillStatusService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BillStatusService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}