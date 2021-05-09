using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class SalaryDriverSetService: EntityRepositoryBase<DbContext, LHSalaryDriverSet>
    {
        public SalaryDriverSetService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SalaryDriverSetService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}