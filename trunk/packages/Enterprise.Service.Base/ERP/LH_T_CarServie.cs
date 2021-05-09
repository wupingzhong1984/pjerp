using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据审批过程
    /// </summary>
    public class LH_T_CarServie : EntityRepositoryBase<DbContext, LH_T_Car>
    {
        public LH_T_CarServie(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LH_T_CarServie()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}