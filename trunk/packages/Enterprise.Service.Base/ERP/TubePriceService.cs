using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class TubePriceService: EntityRepositoryBase<DbContext, LHTubePrice>
    {
        public TubePriceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public TubePriceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}