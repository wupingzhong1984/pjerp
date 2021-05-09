using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class SalarySalesmanSetService: EntityRepositoryBase<DbContext, LHSalarySalesmanSet>
    {
        public SalarySalesmanSetService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SalarySalesmanSetService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}