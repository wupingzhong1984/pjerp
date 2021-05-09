using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class ViewTubePriceService: EntityRepositoryBase<DbContext, vm_TubePrice>
    {
        public ViewTubePriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ViewTubePriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}