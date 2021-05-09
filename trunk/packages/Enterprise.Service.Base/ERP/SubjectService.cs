using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{

    /// <summary>
    ///     会计科目
    /// </summary>
    public class SubjectService: EntityRepositoryBase<DbContext, LHSubject>
    {
        public SubjectService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SubjectService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}