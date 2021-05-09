using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     初始供应商应付款
    /// </summary>
    public class InitSupplierAPService : EntityRepositoryBase<DbContext, LHInitSupplierAP>
    {
        public InitSupplierAPService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public InitSupplierAPService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}