using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     初始客户应收账款
    /// </summary>
    public class InitMonthServie: EntityRepositoryBase<DbContext, LHInitMonth>
    {
        public InitMonthServie(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public InitMonthServie()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}