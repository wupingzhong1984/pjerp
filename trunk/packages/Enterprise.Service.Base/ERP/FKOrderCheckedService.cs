using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class FKOrderCheckedService: EntityRepositoryBase<DbContext, LHFKOrderChecked>
    {
        public FKOrderCheckedService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public FKOrderCheckedService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}