using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     客户供应商
    /// </summary>
    public class ViewUnitAllService: EntityRepositoryBase<DbContext, vm_UnitAll>
    {
        public ViewUnitAllService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewUnitAllService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}