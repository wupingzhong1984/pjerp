using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class OilCardDetailsService : EntityRepositoryBase<DbContext, LHOilCardDetails>
    {
        public OilCardDetailsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public OilCardDetailsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}