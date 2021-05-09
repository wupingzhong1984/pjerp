using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     转账单
    /// </summary>
    public class TransferAccountsService: EntityRepositoryBase<DbContext, LHTransferAccounts>
    {
        public TransferAccountsService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public TransferAccountsService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}