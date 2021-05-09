using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    public class AccountService : EntityRepositoryBase<DbContext, base_account>
    {
        public AccountService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public AccountService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}