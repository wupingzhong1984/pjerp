using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;


namespace Enterprise.Service.Base.ERP
{
   public  class PassCardService : EntityRepositoryBase<DbContext, LHPassCard>
    {
        public PassCardService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public PassCardService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}