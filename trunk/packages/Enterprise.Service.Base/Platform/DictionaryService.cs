using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class DictionaryService : EntityRepositoryBase<DbContext, base_dictionary>
    {
        public DictionaryService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }

        public DictionaryService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            base.IsOwnContext = true;
        }
    }
}
