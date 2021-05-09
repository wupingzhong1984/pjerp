using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class GasService: EntityRepositoryBase<DbContext, vm_Gas>
    {
        public GasService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public GasService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}