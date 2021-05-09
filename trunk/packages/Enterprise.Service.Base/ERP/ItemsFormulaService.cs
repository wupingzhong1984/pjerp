using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     配方
    /// </summary>
    public class ItemsFormulaService: EntityRepositoryBase<DbContext, LHItemsFormula>
    {
        public ItemsFormulaService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ItemsFormulaService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}