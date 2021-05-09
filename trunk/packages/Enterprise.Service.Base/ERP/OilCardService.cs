using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class OilCardService : EntityRepositoryBase<DbContext, LHOilCard>
    {
        public OilCardService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public OilCardService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}