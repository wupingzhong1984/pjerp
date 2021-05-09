using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class NoteService: EntityRepositoryBase<DbContext, LHNote>
    {
        public NoteService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public NoteService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}