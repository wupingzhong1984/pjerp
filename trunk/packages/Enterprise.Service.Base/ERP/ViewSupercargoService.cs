using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据审批过程
    /// </summary>
    public class ViewSupercargoService : EntityRepositoryBase<DbContext, vm_Supercargo>
    {
        public ViewSupercargoService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewSupercargoService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}