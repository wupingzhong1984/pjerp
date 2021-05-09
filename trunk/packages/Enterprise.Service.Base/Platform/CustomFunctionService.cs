using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class CustomFunctionService: EntityRepositoryBase<DbContext, base_custom_function>
    {
        public CustomFunctionService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public CustomFunctionService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}