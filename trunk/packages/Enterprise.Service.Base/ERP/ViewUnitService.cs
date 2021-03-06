using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewUnitService: EntityRepositoryBase<DbContext, vm_Unit>
    {
        public ViewUnitService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewUnitService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}