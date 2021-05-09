using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewLeaseBottleARService: EntityRepositoryBase<DbContext, vm_LeaseBottleAR>
    {
        public ViewLeaseBottleARService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewLeaseBottleARService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}