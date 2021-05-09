using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     初始客户供应商占用量
    /// </summary>
    public class InitCustomerSupplierBottleService : EntityRepositoryBase<DbContext, LHInitCustomerSupplierBottle>
    {
        public InitCustomerSupplierBottleService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public InitCustomerSupplierBottleService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}